// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteRecordOnMatchingRecordFilterProtocol{TId}.cs" company="Naos Project">
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
    /// Check on sagas, write records when records exists.
    /// </summary>
    public partial class WriteEventOnMatchingRecordFilterProtocol<TId> : SyncSpecificVoidProtocolBase<WriteEventOnMatchingRecordFilterOp<TId>>
    {
        private readonly ISyncAndAsyncReturningProtocol<CheckRecordExistsOp, CheckRecordExistsResult> checkRecordExistsProtocol;
        private readonly ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteEventOnMatchingRecordFilterProtocol{TId}"/> class.
        /// </summary>
        /// <param name="checkRecordExistsProtocol">The check record exists protocol.</param>
        /// <param name="streamFactory">The protocol to get <see cref="IStream"/> from a <see cref="StreamRepresentation"/>.</param>
        public WriteEventOnMatchingRecordFilterProtocol(
            ISyncAndAsyncReturningProtocol<CheckRecordExistsOp, CheckRecordExistsResult> checkRecordExistsProtocol,
            ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory)
        {
            checkRecordExistsProtocol.MustForArg(nameof(checkRecordExistsProtocol)).NotBeNull();
            streamFactory.MustForArg(nameof(streamFactory)).NotBeNull();

            this.checkRecordExistsProtocol = checkRecordExistsProtocol;
            this.streamFactory = streamFactory;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public override void Execute(
            WriteEventOnMatchingRecordFilterOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var results = new Dictionary<CheckRecordExistsOp, CheckRecordExistsResult>();
            foreach (var checkRecordExistsOp in operation.CheckRecordExistsOps)
            {
                var result = this.checkRecordExistsProtocol.Execute(checkRecordExistsOp);
                results.Add(checkRecordExistsOp, result);
            }

            if (results.Any())
            {
                var recordExistsSet = results.Select(_ => _.Value.RecordExists).ToList();

                foreach (var eventToPutWithIdOnMatch in operation.EventToPutOnMatchChainOfResponsibility)
                {
                    var matches =
                        recordExistsSet.MatchesAccordingToStrategy(
                        eventToPutWithIdOnMatch.RecordExistsMatchStrategy);

                    if (matches)
                    {
                        var eventToPutWithId = eventToPutWithIdOnMatch.EventToPut;
                        var targetStream = this.streamFactory.Execute(new GetStreamFromRepresentationOp(eventToPutWithId.StreamRepresentation));
                        targetStream.MustForOp("targetStreamMustBeIWriteOnlyStream").BeAssignableToType<IWriteOnlyStream>();

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

                        ((IWriteOnlyStream)targetStream).PutWithId(eventToPutWithId.Id, eventToPut, eventToPutWithId.Tags);

                        switch (eventToPutWithIdOnMatch.ChainOfResponsibilityLinkMatchStrategy)
                        {
                            case ChainOfResponsibilityLinkMatchStrategy.MatchHaltsEvaluationOfChainAndCompletes:
                                return;
                            case ChainOfResponsibilityLinkMatchStrategy.MatchHaltsEvaluationOfChainAndSelfCancels:
                                Thread.Sleep(operation.WaitTimeBeforeRetry);
                                throw new SelfCancelRunningExecutionException("Matched status and wrote record, terminating chain but not terminating execution.");
                            case ChainOfResponsibilityLinkMatchStrategy.Continue:
                                // Keep going through links...
                                break;
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
