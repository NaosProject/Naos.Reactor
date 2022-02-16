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
        /// <param name="matchTerminatesChain">OPTIONAL value indicating whether or not to terminate the larger execution context on match; DEFAULT is true.</param>
        /// <param name="matchTerminatesExecution">OPTIONAL value indicating whether or not to terminate the larger execution context on match; DEFAULT is true.</param>
        public EventToPutWithIdOnRecordFilterMatch(
            RecordExistsMatchStrategy recordExistsMatchStrategy,
            EventToPutWithId<TId> eventToPut,
            bool matchTerminatesChain = true,
            bool matchTerminatesExecution = true)
        {
            recordExistsMatchStrategy.MustForArg(nameof(recordExistsMatchStrategy)).NotBeEqualTo(CompositeHandlingStatusMatchStrategy.Unknown);
            eventToPut.MustForArg(nameof(eventToPut)).NotBeNull();

            this.RecordExistsMatchStrategy = recordExistsMatchStrategy;
            this.EventToPut = eventToPut;
            this.MatchTerminatesChain = matchTerminatesChain;
            this.MatchTerminatesExecution = matchTerminatesExecution;
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
        public bool MatchTerminatesChain { get; private set; }

        /// <inheritdoc />
        public bool MatchTerminatesExecution { get; private set; }
    }
}
