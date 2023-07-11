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
    public partial class WriteEventOnMatchingHandlingStatusProtocol<TId> : SyncSpecificVoidProtocolBase<WriteEventOnMatchingHandlingStatusOp<TId>>
    {
        private static readonly IStreamRepresentation NullStreamRepresentation = new NullStandardStream().StreamRepresentation;
        private readonly ISyncAndAsyncReturningProtocol<CheckRecordHandlingOp, CheckRecordHandlingResult> checkRecordHandlingProtocol;
        private readonly ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteEventOnMatchingHandlingStatusProtocol{TId}"/> class.
        /// </summary>
        /// <param name="checkRecordHandlingProtocol">The check record handling protocol.</param>
        /// <param name="streamFactory">The protocol to get <see cref="IStream"/> from a <see cref="StreamRepresentation"/>.</param>
        public WriteEventOnMatchingHandlingStatusProtocol(
            ISyncAndAsyncReturningProtocol<CheckRecordHandlingOp, CheckRecordHandlingResult> checkRecordHandlingProtocol,
            ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory)
        {
            checkRecordHandlingProtocol.MustForArg(nameof(checkRecordHandlingProtocol)).NotBeNull();
            streamFactory.MustForArg(nameof(streamFactory)).NotBeNull();

            this.checkRecordHandlingProtocol = checkRecordHandlingProtocol;
            this.streamFactory = streamFactory;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public override void Execute(
            WriteEventOnMatchingHandlingStatusOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var results = new Dictionary<CheckRecordHandlingOp, CheckRecordHandlingResult>();
            foreach (var checkRecordHandlingOp in operation.CheckRecordHandlingOps)
            {
                var result = this.checkRecordHandlingProtocol.Execute(checkRecordHandlingOp);
                if (checkRecordHandlingOp.ExpectedCount              != null
                 && result.InternalRecordIdToHandlingStatusMap.Count != checkRecordHandlingOp.ExpectedCount)
                {
                    throw new SelfCancelRunningExecutionException(
                        Invariant(
                            $"Expected {checkRecordHandlingOp.ExpectedCount} statuses and only got back {result.InternalRecordIdToHandlingStatusMap.Count}."));
                }

                results.Add(checkRecordHandlingOp, result);
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
                            eventToPut = eventBase.DeepCloneWithTimestampUtc(DateTime.UtcNow);
                        }
                        else
                        {
                            eventToPut = eventToPutWithId.EventToPut;
                        }

                        var eventToPutTags = eventToPut is IHaveTags eventToPutWithTags
                            ? eventToPutWithTags.Tags
                            : null;

                        if (!NullStreamRepresentation.Equals(eventToPutWithId.StreamRepresentation))
                        {
                            var targetStream = this.streamFactory.Execute(new GetStreamFromRepresentationOp(eventToPutWithId.StreamRepresentation));
                            targetStream.MustForOp("targetStreamMustBeIWriteOnlyStream").BeAssignableToType<IWriteOnlyStream>();
                            ((IWriteOnlyStream)targetStream).PutWithId(eventToPutWithId.Id, eventToPut, eventToPutTags);
                        }

                        switch (eventToPutWithIdOnMatch.ChainOfResponsibilityLinkMatchStrategy)
                        {
                            case ChainOfResponsibilityLinkMatchStrategy.MatchHaltsEvaluationOfChainAndCompletes:
                                return;
                            case ChainOfResponsibilityLinkMatchStrategy.MatchHaltsEvaluationOfChainAndSelfCancels:
                                Thread.Sleep(operation.WaitTimeBeforeRetry);
                                throw new SelfCancelRunningExecutionException(Invariant($"Matched status and wrote record, terminating chain but not terminating execution; details: {eventToPutWithIdOnMatch.Details}."));
                            case ChainOfResponsibilityLinkMatchStrategy.Continue:
                                // Keep going through links...
                                break;
                            case ChainOfResponsibilityLinkMatchStrategy.MatchHaltsEvaluationOfChainAndFails:
                                throw new OpExecutionFailedException(Invariant($"Matched status and wrote record, terminating chain and failing execution; details: {eventToPutWithIdOnMatch.Details}."));
                            default:
                                throw new NotSupportedException(Invariant($"Unsupported {nameof(ChainOfResponsibilityLinkMatchStrategy)}: {eventToPutWithIdOnMatch.ChainOfResponsibilityLinkMatchStrategy}."));
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
