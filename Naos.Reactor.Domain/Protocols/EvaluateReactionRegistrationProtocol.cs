// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvaluateReactionRegistrationProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.String.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;
    
    /// <summary>
    /// Process all new reactions.
    /// </summary>
    public partial class EvaluateReactionRegistrationProtocol
        : SyncSpecificReturningProtocolBase<EvaluateReactionRegistrationOp, EvaluateReactionRegistrationResult>
    {
        private readonly ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluateReactionRegistrationProtocol"/> class.
        /// </summary>
        /// <param name="streamFactory">The protocol to get <see cref="IStream"/> from a <see cref="StreamRepresentation"/>.</param>
        public EvaluateReactionRegistrationProtocol(
            ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory)
        {
            streamFactory.MustForArg(nameof(streamFactory)).NotBeNull();

            this.streamFactory = streamFactory;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public override EvaluateReactionRegistrationResult Execute(
            EvaluateReactionRegistrationOp operation)
        {
            if (operation.ReactionRegistration.Dependencies.Count != 1)
            {
                throw new NotSupportedException(Invariant($"Only 1 single {typeof(RecordFilterReactorDependency)} is supported, {operation.ReactionRegistration.Dependencies.Count} were supplied."));
            }

            var dependency = operation.ReactionRegistration.Dependencies.Single();
            var recordFilterDependency = dependency as RecordFilterReactorDependency;
            if (recordFilterDependency == null)
            {
                throw new NotSupportedException(Invariant($"Only {typeof(RecordFilterReactorDependency)} is supported, {dependency?.GetType().ToStringReadable()}."));
            }

            var reactionId = Invariant($"{operation.ReactionRegistration.Id}___{DateTime.UtcNow.ToStringInvariantPreferred()}");

            var records = new Dictionary<IStreamRepresentation, HashSet<long>>();
            var handledRecordMementos = new List<RecordSetHandlingMemento>();
            var allRequiredSeen = true;
            foreach (var recordFilterEntry in recordFilterDependency.Entries)
            {
                try
                {
                    var stream = this.streamFactory.Execute(new GetStreamFromRepresentationOp(recordFilterEntry.StreamRepresentation));
                    stream.MustForOp(nameof(stream)).BeAssignableToType<ISyncReturningProtocol<StandardTryHandleRecordOp, TryHandleRecordResult>>();
                    var streamProtocol = (ISyncReturningProtocol<StandardTryHandleRecordOp, TryHandleRecordResult>)stream;

                    var tryHandleConcern = Invariant($"{operation.ReactionRegistration.Id}_{recordFilterEntry.Id}");

                    var handledIds = new HashSet<long>(); // this is because we could get duplicates...
                    StreamRecord currentRecord = null;
                    do
                    {
                        var tryHandleRecordOp = new StandardTryHandleRecordOp(
                            tryHandleConcern,
                            recordFilterEntry.RecordFilter,
                            streamRecordItemsToInclude: StreamRecordItemsToInclude.MetadataOnly,
                            tags: new[]
                                  {
                                      new NamedValue<string>(TagNames.PotentialReactionId, reactionId),
                                  });

                        var tryHandleRecordResult = streamProtocol.Execute(tryHandleRecordOp);

                        currentRecord = tryHandleRecordResult?.RecordToHandle;
                        if (currentRecord != null)
                        {
                            handledIds.Add(currentRecord.InternalRecordId);
                        }
                    }
                    while (currentRecord != null);

                    if (handledIds.Any())
                    {
                        var streamProtocolFactoryForActions = (IStreamRecordHandlingProtocolFactory)stream;
                        var streamHandlingProtocolForActions = streamProtocolFactoryForActions.GetStreamRecordHandlingProtocols();

                        void CompleteAction()
                        {
                            foreach (var handledInternalRecordId in handledIds)
                            {
                                var completeHandlingOp = new CompleteRunningHandleRecordOp(
                                    handledInternalRecordId,
                                    tryHandleConcern);
                                streamHandlingProtocolForActions.Execute(
                                    completeHandlingOp);
                            }
                        }

                        void CancelAction()
                        {
                            foreach (var handledInternalRecordId in handledIds)
                            {
                                var cancelHandlingOp = new CancelRunningHandleRecordOp(
                                    handledInternalRecordId,
                                    tryHandleConcern,
                                    "Not all required dependencies present.");
                                streamHandlingProtocolForActions.Execute(
                                    cancelHandlingOp);
                            }
                        }

                        var actionableSetForTracking = new RecordSetHandlingMemento(CompleteAction, CancelAction);
                        handledRecordMementos.Add(actionableSetForTracking);

                        if (recordFilterEntry.IncludeInReaction)
                        {
                            if (records.ContainsKey(recordFilterEntry.StreamRepresentation))
                            {
                                records[recordFilterEntry.StreamRepresentation].AddRange(handledIds);
                            }
                            else
                            {
                                records.Add(recordFilterEntry.StreamRepresentation, handledIds);
                            }
                        }
                    }
                    else
                    {
                        if (recordFilterEntry.RequiredForReaction)
                        {
                            allRequiredSeen = false;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ReactorException(Invariant($"Failed to process entry: {recordFilterEntry}."), ex, operation);
                }
            }

            EvaluateReactionRegistrationResult result;
            if (!allRequiredSeen)
            {
                foreach (var recordSetHandlingMemento in handledRecordMementos)
                {
                    recordSetHandlingMemento.CancelSet();
                }

                // no reaction created since not all requirement met.
                result = new EvaluateReactionRegistrationResult(null, new List<RecordSetHandlingMemento>());
            }
            else if (!handledRecordMementos.Any())
            {
                // no reaction created since there wasn't anything to react to.
                result = new EvaluateReactionRegistrationResult(null, new List<RecordSetHandlingMemento>());
            }
            else
            {
                var readonlyRecords = records.ToDictionary(k => k.Key, v => (IReadOnlyList<long>)v.Value);
                var reactionEvent = new ReactionEvent(
                    reactionId,
                    operation.ReactionRegistration.Id,
                    operation.ReactionRegistration.ReactionContext,
                    readonlyRecords,
                    DateTime.UtcNow,
                    operation.ReactionRegistration.Tags);

                result = new EvaluateReactionRegistrationResult(reactionEvent, handledRecordMementos);
            }

            return result;
        }
    }
}
