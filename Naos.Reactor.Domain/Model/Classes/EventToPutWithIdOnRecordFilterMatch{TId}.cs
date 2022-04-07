// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventToPutWithIdOnMatch{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// A model to contain an <see cref="EventToPutWithId{TId}"/> to be used when specified match criteria is met around record existence matching a filter.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier used in <see cref="EventToPut"/>.</typeparam>
    public partial class EventToPutWithIdOnRecordFilterMatch<TId> : IModelViaCodeGen, IChainOfResponsibilityLink
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventToPutWithIdOnRecordFilterMatch{TId}"/> class.
        /// </summary>
        /// <param name="recordExistsMatchStrategy">The <see cref="RecordExistsMatchStrategy"/> to use with supplied filter.</param>
        /// <param name="eventToPut">The event to put on a match.</param>
        /// <param name="chainOfResponsibilityLinkMatchStrategy">OPTIONAL strategy override to determine what to do in the execution on a match; DEFAULT is Halt and Complete.</param>
        public EventToPutWithIdOnRecordFilterMatch(
            RecordExistsMatchStrategy recordExistsMatchStrategy,
            EventToPutWithId<TId> eventToPut,
            ChainOfResponsibilityLinkMatchStrategy chainOfResponsibilityLinkMatchStrategy = ChainOfResponsibilityLinkMatchStrategy.MatchHaltsEvaluationOfChainAndCompletes)
        {
            recordExistsMatchStrategy.MustForArg(nameof(recordExistsMatchStrategy)).NotBeEqualTo(CompositeHandlingStatusMatchStrategy.Unknown);
            eventToPut.MustForArg(nameof(eventToPut)).NotBeNull();
            chainOfResponsibilityLinkMatchStrategy.MustForArg(nameof(chainOfResponsibilityLinkMatchStrategy)).NotBeEqualTo(ChainOfResponsibilityLinkMatchStrategy.Unknown);

            this.RecordExistsMatchStrategy = recordExistsMatchStrategy;
            this.EventToPut = eventToPut;
            this.ChainOfResponsibilityLinkMatchStrategy = chainOfResponsibilityLinkMatchStrategy;
        }

        /// <summary>
        /// Gets the <see cref="RecordExistsMatchStrategy"/>.
        /// </summary>
        public RecordExistsMatchStrategy RecordExistsMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the event to put on a match.
        /// </summary>
        public EventToPutWithId<TId> EventToPut { get; private set; }

        /// <inheritdoc />
        public ChainOfResponsibilityLinkMatchStrategy ChainOfResponsibilityLinkMatchStrategy { get; private set; }
    }
}
