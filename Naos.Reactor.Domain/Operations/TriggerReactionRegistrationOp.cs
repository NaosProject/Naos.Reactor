// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TriggerReactionRegistrationOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Explicitly triggers a <see cref="ReactionRegistration"/>, disables the required concept on dependencies.
    /// </summary>
    public partial class TriggerReactionRegistrationOp : VoidOperationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RunReactorOp"/> class.
        /// </summary>
        /// <param name="reactionRegistrationId">The reaction registration identifier to trigger.</param>
        public TriggerReactionRegistrationOp(
            string reactionRegistrationId)
        {
            reactionRegistrationId.MustForOp(nameof(reactionRegistrationId)).NotBeNullNorWhiteSpace();

            this.ReactionRegistrationId = reactionRegistrationId;
        }

        /// <summary>
        /// Gets the reaction registration identifier to trigger.
        /// </summary>
        public string ReactionRegistrationId { get; private set; }
    }
}
