// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RunReactorProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Protocol
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using Naos.Logging.Domain;
    using Naos.Reactor.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.String.Recipes;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Protocol for <see cref="RunReactorOp"/>.
    /// </summary>
    public partial class RunReactorProtocol : SyncSpecificVoidProtocolBase<RunReactorOp>
    {
        private static readonly TypeRepresentation ReactionRegistrationTypeRepWithoutVersion = typeof(ReactionRegistration).ToRepresentation().RemoveAssemblyVersions();
        private static readonly ConcurrentDictionary<StringSerializedIdentifier, DateTime> ReactionRegistrationIdToNextEvaluationCutoffMap = new ConcurrentDictionary<StringSerializedIdentifier, DateTime>();

        private readonly IStandardStream reactionRegistrationStream;
        private readonly IStandardStream reactionStream;
        private readonly ISyncAndAsyncReturningProtocol<EvaluateReactionRegistrationOp, EvaluateReactionRegistrationResult> evaluateReactionRegistrationProtocol;

        /// <summary>
        /// Initializes a new instance of the <see cref="RunReactorProtocol"/> class.
        /// </summary>
        /// <param name="reactionRegistrationStream">The registered reaction stream.</param>
        /// <param name="reactionStream">The reaction stream.</param>
        /// <param name="evaluateReactionRegistrationProtocol">The evaluate registered reaction protocol.</param>
        public RunReactorProtocol(
            IStandardStream reactionRegistrationStream,
            IStandardStream reactionStream,
            ISyncAndAsyncReturningProtocol<EvaluateReactionRegistrationOp, EvaluateReactionRegistrationResult> evaluateReactionRegistrationProtocol)
        {
            reactionRegistrationStream.MustForArg(nameof(reactionRegistrationStream)).NotBeNull();
            reactionStream.MustForArg(nameof(reactionStream)).NotBeNull();
            evaluateReactionRegistrationProtocol.MustForArg(nameof(evaluateReactionRegistrationProtocol)).NotBeNull();

            this.reactionRegistrationStream = reactionRegistrationStream;
            this.reactionStream = reactionStream;
            this.evaluateReactionRegistrationProtocol = evaluateReactionRegistrationProtocol;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public override void Execute(
            RunReactorOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var getDistinctStringSerializedIdsOp = new StandardGetDistinctStringSerializedIdsOp(
                new RecordFilter(
                    objectTypes: new[]
                                 {
                                     typeof(ReactionRegistration).ToRepresentation(),
                                 },
                    deprecatedIdTypes: new[]
                                       {
                                           operation.DeprecatedIdentifierType,
                                       }));

            var processedIds = new ConcurrentDictionary<StringSerializedIdentifier, Exception>();
            void ProcessReactionRegistration(
                StringSerializedIdentifier reactionRegistrationId)
            {
                try
                {
                    var getLatestRecordOp = new StandardGetLatestRecordOp(
                        new RecordFilter(
                            ids: new[]
                                 {
                                     reactionRegistrationId,
                                 }));

                    var reactionRegistrationRecord = this.reactionRegistrationStream.Execute(getLatestRecordOp);
                    reactionRegistrationRecord
                       .Payload
                       .PayloadTypeRepresentation
                       .RemoveAssemblyVersions()
                       .MustForOp("recordFromReactionRegistrationStreamExpectedToBeRegisteredReaction")
                       .BeEqualTo(ReactionRegistrationTypeRepWithoutVersion);

                    var reactionRegistration =
                        reactionRegistrationRecord.Payload.DeserializePayloadUsingSpecificFactory<ReactionRegistration>(
                            this.reactionRegistrationStream.SerializerFactory);

                    var evaluateReactionRegistrationOp = new EvaluateReactionRegistrationOp(reactionRegistration);
                    var evaluateReactionRegistrationResult =
                        this.evaluateReactionRegistrationProtocol.Execute(evaluateReactionRegistrationOp);
                    if (evaluateReactionRegistrationResult.ReactionEvent != null)
                    {
                        var reaction = evaluateReactionRegistrationResult.ReactionEvent;

                        this.reactionStream.PutWithId(reaction.Id, reaction, reaction.Tags);

                        // once we have recorded the reaction then we can finalize the handling cycle.
                        foreach (var recordSetHandlingMemento in evaluateReactionRegistrationResult.RecordSetHandlingMementos)
                        {
                            recordSetHandlingMemento.CompleteSet();
                        }
                    }

                    var newNextEvaluationCutoff = DateTime.UtcNow.Add(reactionRegistration.IdealWaitTimeBetweenEvaluations);
                    ReactionRegistrationIdToNextEvaluationCutoffMap.AddOrUpdate(
                        reactionRegistrationId,
                        newNextEvaluationCutoff,
                        (
                            _,
                            __) => newNextEvaluationCutoff);
                    processedIds.TryAdd(reactionRegistrationId, null);
                }
                catch (Exception ex)
                {
                    processedIds.TryAdd(reactionRegistrationId, ex);
                    var wrappedEx = new ReactorException(
                        Invariant($"Failed to process {nameof(ReactionRegistration)} Id: {reactionRegistrationId}."),
                        ex,
                        operation);

                    Log.Write(() => wrappedEx);

                    throw wrappedEx;
                }
            }

            var distinctIds = this.reactionRegistrationStream.Execute(getDistinctStringSerializedIdsOp);
            var identifiersToProcess = new List<StringSerializedIdentifier>();
            foreach (var identifierToCheck in distinctIds)
            {
                var found = ReactionRegistrationIdToNextEvaluationCutoffMap.TryGetValue(identifierToCheck, out var nextEvaluationCutoff);

                if (!found || DateTime.UtcNow > nextEvaluationCutoff)
                {
                    identifiersToProcess.Add(identifierToCheck);
                }
            }

            var parallelLoopResult = Parallel.ForEach(
                identifiersToProcess,
                new ParallelOptions { MaxDegreeOfParallelism = operation.DegreesOfParallelismForDependencyChecks },
                ProcessReactionRegistration);

            // this should only be necessary if stop or break is called which is not happening but is here from a paranoid perspective...
            parallelLoopResult.IsCompleted.MustForOp(nameof(parallelLoopResult)).BeTrue();

            if (processedIds.Count != identifiersToProcess.Count)
            {
                var missedIds = identifiersToProcess.Where(_ => !processedIds.ContainsKey(_)).ToList();
                var missedIdsMixIn = string.Join(",", missedIds);
                throw new ReactorException(
                    Invariant($"Failed to process {nameof(ReactionRegistration)} Missed Ids: {missedIdsMixIn}."),
                    operation);
            }

            if (processedIds.Any(_ => _.Value != null))
            {
                var exceptions = processedIds
                                .Where(_ => _.Value != null)
                                .Select(
                                     _ =>
                                         new ReactorException(
                                             Invariant(
                                                 $"Failed to process {nameof(ReactionRegistration)} Id: {_.Key} had unreported exception: {_.Value}."),
                                             operation))
                                .ToList();
                if (exceptions.Count == 1)
                {
                    throw exceptions.Single();
                }
                else
                {
                    throw new AggregateException(exceptions);
                }
            }
        }
    }
}
