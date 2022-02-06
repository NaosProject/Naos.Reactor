// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventToPutWithIdOnMatch{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// A model to contain an <see cref="EventToPutWithId{TId}"/> to be used when specified match criteria is met around a <see cref="CompositeHandlingStatus"/>.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier used in <see cref="EventToPut"/>.</typeparam>
    public partial class EventToPutWithIdOnMatch<TId> : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventToPutWithIdOnMatch{TId}"/> class.
        /// </summary>
        /// <param name="statusToMatch">The <see cref="CompositeHandlingStatus"/> to match.</param>
        /// <param name="compositeHandlingStatusMatchStrategy">The <see cref="CompositeHandlingStatusMatchStrategy"/> to use with <paramref name="statusToMatch"/>.</param>
        /// <param name="eventToPut">The event to put on a match.</param>
        /// <param name="matchTerminatesChain">OPTIONAL value indicating whether or not to terminate the larger execution context on match; DEFAULT is true.</param>
        /// <param name="matchTerminatesExecution">OPTIONAL value indicating whether or not to terminate the larger execution context on match; DEFAULT is true.</param>
        public EventToPutWithIdOnMatch(
            CompositeHandlingStatus statusToMatch,
            CompositeHandlingStatusMatchStrategy compositeHandlingStatusMatchStrategy,
            EventToPutWithId<TId> eventToPut,
            bool matchTerminatesChain = true,
            bool matchTerminatesExecution = true)
        {
            statusToMatch.MustForArg(nameof(statusToMatch)).NotBeEqualTo(CompositeHandlingStatus.Unknown);
            compositeHandlingStatusMatchStrategy.MustForArg(nameof(statusToMatch)).NotBeEqualTo(CompositeHandlingStatus.Unknown);
            eventToPut.MustForArg(nameof(eventToPut)).NotBeNull();

            this.StatusToMatch = statusToMatch;
            this.CompositeHandlingStatusMatchStrategy = compositeHandlingStatusMatchStrategy;
            this.EventToPut = eventToPut;
            this.MatchTerminatesChain = matchTerminatesChain;
            this.MatchTerminatesExecution = matchTerminatesExecution;
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

        /// <summary>
        /// Gets a value indicating whether or not to terminate the chain of responsibility on match.
        /// </summary>
        public bool MatchTerminatesChain { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not to terminate the larger execution context on match.
        /// </summary>
        public bool MatchTerminatesExecution { get; private set; }
    }
}
