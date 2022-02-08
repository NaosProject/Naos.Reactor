// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteRecordOnHandlingCompletedProtocol{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Enum.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Check on sagas, write records under certain handling scenarios of groups of records.
    /// </summary>
    /// <typeparam name="TId">Type of the identifier.</typeparam>
    public partial class WriteRecordOnHandlingCompletedProtocol<TId> : SyncSpecificVoidProtocolBase<WriteRecordOnMatchingHandlingStatusOp<TId>>
    {
        private readonly ISyncAndAsyncReturningProtocol<CheckRecordHandlingOp, CheckRecordHandlingResult> checkSingleRecordHandlingProtocol;
        private readonly ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteRecordOnHandlingCompletedProtocol{TId}"/> class.
        /// </summary>
        /// <param name="checkSingleRecordHandlingProtocol">The check single record handling protocol.</param>
        /// <param name="streamFactory">The protocol to get <see cref="IStream"/> from a <see cref="StreamRepresentation"/>.</param>
        public WriteRecordOnHandlingCompletedProtocol(
            ISyncAndAsyncReturningProtocol<CheckRecordHandlingOp, CheckRecordHandlingResult> checkSingleRecordHandlingProtocol,
            ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory)
        {
            this.checkSingleRecordHandlingProtocol = checkSingleRecordHandlingProtocol;
            this.streamFactory = streamFactory;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public override void Execute(
            WriteRecordOnMatchingHandlingStatusOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var results = new Dictionary<CheckRecordHandlingOp, CheckRecordHandlingResult>();
            foreach (var operationCheckSingleRecordHandlingOp in operation.CheckRecordHandlingOps)
            {
                var result = this.checkSingleRecordHandlingProtocol.Execute(operationCheckSingleRecordHandlingOp);
                results.Add(operationCheckSingleRecordHandlingOp, result);
            }

            if (results.Any())
            {
                var actualCompositeHandlingStatus = results
                                             .SelectMany(_ => _.Value.InternalRecordIdToHandlingStatusMap.Values)
                                             .ToList()
                                             .ToCompositeHandlingStatus();

                foreach (var eventToPutWithIdOnMatch in operation.EventToPutOnMatchChainOfResponsibility)
                {
                    var matches = actualCompositeHandlingStatus.MatchesAccordingToStrategy(
                        eventToPutWithIdOnMatch.StatusToMatch,
                        eventToPutWithIdOnMatch.CompositeHandlingStatusMatchStrategy);

                    if (matches)
                    {
                        var eventToPutWithId = eventToPutWithIdOnMatch.EventToPut;
                        var targetStream = this.streamFactory.Execute(new GetStreamFromRepresentationOp(eventToPutWithId.StreamRepresentation));
                        targetStream.MustForOp("targetStreamMustBeIWriteOnlyStream").BeOfType<IWriteOnlyStream>();

                        IEvent eventToPut;
                        if (eventToPutWithId.UpdateTimestampOnPut)
                        {
                            var eventBase = eventToPutWithId.EventToPut as EventBase;
                            eventBase
                               .MustForOp(Invariant($"{nameof(eventToPutWithId)}.{nameof(eventToPutWithId.EventToPut)}"))
                               .NotBeNull(
                                    Invariant(
                                        $"Only {nameof(EventBase)} is supported, this was {eventToPutWithId.EventToPut.GetType().ToStringReadable()}."));

                            // ReSharper disable once PossibleNullReferenceException - checked with Must above
                            eventToPut = eventBase.DeepCloneWithTimestampUtc(DateTime.Now);
                        }
                        else
                        {
                            eventToPut = eventToPutWithId.EventToPut;
                        }

                        ((IWriteOnlyStream)targetStream).PutWithId(eventToPutWithId.Id, eventToPut, eventToPutWithId.Tags);

                        if (!eventToPutWithIdOnMatch.MatchTerminatesExecution)
                        {
                            return;
                        }

                        if (eventToPutWithIdOnMatch.MatchTerminatesChain)
                        {
                            Thread.Sleep(operation.WaitTimeBeforeRetry);
                            throw new SelfCancelRunningExecutionException("Matched status and wrote record, terminating chain but not terminating.");
                        }
                    }
                }

                Thread.Sleep(operation.WaitTimeBeforeRetry);
                throw new SelfCancelRunningExecutionException("No matches found or the matches did not terminate the chain or execution.");
            }
            else
            {
                Thread.Sleep(operation.WaitTimeBeforeRetry);
                throw new SelfCancelRunningExecutionException("No records found to test.");
            }
        }
    }
}
