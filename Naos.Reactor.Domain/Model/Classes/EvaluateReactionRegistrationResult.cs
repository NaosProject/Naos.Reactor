// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvaluateReactionRegistrationResult.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Result object for <see cref="EvaluateReactionRegistrationOp"/>.
    /// </summary>
    public class EvaluateReactionRegistrationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluateReactionRegistrationResult"/> class.
        /// </summary>
        /// <param name="reactionEvent">The reaction.</param>
        /// <param name="recordSetHandlingMementos">The mementos for completing or canceling.</param>
        public EvaluateReactionRegistrationResult(
            ReactionEvent reactionEvent,
            IReadOnlyCollection<RecordSetHandlingMemento> recordSetHandlingMementos)
        {
            if (reactionEvent != null)
            {
                recordSetHandlingMementos.MustForArg(nameof(recordSetHandlingMementos)).NotBeNullNorEmptyEnumerableNorContainAnyNulls();
            }
            else
            {
                recordSetHandlingMementos.MustForArg(nameof(recordSetHandlingMementos)).NotBeNull();
            }

            this.ReactionEvent = reactionEvent;
            this.RecordSetHandlingMementos = recordSetHandlingMementos;
        }

        /// <summary>
        /// Gets the reaction.
        /// </summary>
        public ReactionEvent ReactionEvent { get; private set; }

        /// <summary>
        /// Gets the mementos for completing or canceling.
        /// </summary>
        public IReadOnlyCollection<RecordSetHandlingMemento> RecordSetHandlingMementos { get; private set; }
    }
}
