// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RunReactorProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Protocol for <see cref="RunReactorOp"/>.
    /// </summary>
    public partial class RunReactorProtocol : SyncSpecificVoidProtocolBase<RunReactorOp>
    {
        private readonly IStandardStream reactionRegistrationStream;
        private readonly IStandardStream reactionStream;
        private readonly ISyncAndAsyncReturningProtocol<EvaluateReactionRegistrationOp, EvaluateReactionRegistrationResult> evaluateReactionRegistrationProtocol;
        private static readonly TypeRepresentation ReactionRegistrationTypeRepWithoutVersion = typeof(ReactionRegistration).ToRepresentation().RemoveAssemblyVersions();

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
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
                                           operation.DeprecatedIdentifierType
                                       }));
            var distinctIds = this.reactionRegistrationStream.Execute(getDistinctStringSerializedIdsOp);
            foreach (var distinctId in distinctIds)
            {
                try
                {
                    var getLatestRecordOp = new StandardGetLatestRecordOp(
                        new RecordFilter(
                            ids: new[]
                                 {
                                     distinctId,
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
                    var evaluateReactionRegistrationResult = this.evaluateReactionRegistrationProtocol.Execute(evaluateReactionRegistrationOp);
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
                }
                catch (Exception ex)
                {
                    throw new ReactorException(Invariant($"Failed to process {nameof(ReactionRegistration)} Id: {distinctId}."), ex, operation);
                }
            }
        }
    }
}
