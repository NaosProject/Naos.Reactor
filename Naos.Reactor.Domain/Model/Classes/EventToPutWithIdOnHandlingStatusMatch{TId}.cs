// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventToPutWithIdOnHandlingStatusMatch{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// A model to contain an <see cref="EventToPutWithId{TId}"/> to be used when specified match criteria is met around a <see cref="CompositeHandlingStatus"/>.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier used in <see cref="EventToPut"/>.</typeparam>
    public partial class EventToPutWithIdOnHandlingStatusMatch<TId> : IModelViaCodeGen, IChainOfResponsibilityLink, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventToPutWithIdOnHandlingStatusMatch{TId}"/> class.
        /// </summary>
        /// <param name="statusToMatch">The <see cref="CompositeHandlingStatus"/> to match.</param>
        /// <param name="compositeHandlingStatusMatchStrategy">The <see cref="CompositeHandlingStatusMatchStrategy"/> to use with <paramref name="statusToMatch"/>.</param>
        /// <param name="eventToPut">The event to put on a match.</param>
        /// <param name="chainOfResponsibilityLinkMatchStrategy">OPTIONAL strategy override to determine what to do in the execution on a match; DEFAULT is Halt and Complete.</param>
        /// <param name="details">Optional details about the link.</param>
        public EventToPutWithIdOnHandlingStatusMatch(
            CompositeHandlingStatus statusToMatch,
            CompositeHandlingStatusMatchStrategy compositeHandlingStatusMatchStrategy,
            EventToPutWithId<TId> eventToPut,
            ChainOfResponsibilityLinkMatchStrategy chainOfResponsibilityLinkMatchStrategy = ChainOfResponsibilityLinkMatchStrategy.MatchHaltsEvaluationOfChainAndCompletes,
            string details = null)
        {
            statusToMatch.MustForArg(nameof(statusToMatch)).NotBeEqualTo(CompositeHandlingStatus.Unknown);
            compositeHandlingStatusMatchStrategy.MustForArg(nameof(compositeHandlingStatusMatchStrategy)).NotBeEqualTo(CompositeHandlingStatusMatchStrategy.Unknown);
            eventToPut.MustForArg(nameof(eventToPut)).NotBeNull();
            chainOfResponsibilityLinkMatchStrategy.MustForArg(nameof(chainOfResponsibilityLinkMatchStrategy)).NotBeEqualTo(ChainOfResponsibilityLinkMatchStrategy.Unknown);

            this.StatusToMatch = statusToMatch;
            this.CompositeHandlingStatusMatchStrategy = compositeHandlingStatusMatchStrategy;
            this.EventToPut = eventToPut;
            this.ChainOfResponsibilityLinkMatchStrategy = chainOfResponsibilityLinkMatchStrategy;
            this.Details = details;
        }

        /// <summary>
        /// Gets the <see cref="CompositeHandlingStatus"/> to match.
        /// </summary>
        public CompositeHandlingStatus StatusToMatch { get; private set; }

        /// <summary>
        /// Gets the <see cref="CompositeHandlingStatusMatchStrategy"/>.
        /// </summary>
        public CompositeHandlingStatusMatchStrategy CompositeHandlingStatusMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the event to put on a match.
        /// </summary>
        public EventToPutWithId<TId> EventToPut { get; private set; }

        /// <inheritdoc />
        public ChainOfResponsibilityLinkMatchStrategy ChainOfResponsibilityLinkMatchStrategy { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
