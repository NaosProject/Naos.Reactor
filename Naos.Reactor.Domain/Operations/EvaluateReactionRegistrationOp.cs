// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvaluateReactionRegistrationOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Operation to evaluate a <see cref="ReactionRegistration"/> and get any applicable.
    /// <remarks>Null is the expected result if there are no records handled.</remarks>
    /// </summary>
    public partial class EvaluateReactionRegistrationOp : ReturningOperationBase<EvaluateReactionRegistrationResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluateReactionRegistrationOp"/> class.
        /// </summary>
        /// <param name="reactionRegistration">The <see cref="ReactionRegistration"/> to evaluate.</param>
        /// <param name="overrideRequired">Treat the reaction dependencies as if everything is optional (e.g. if you are triggering a reaction).</param>
        public EvaluateReactionRegistrationOp(
            ReactionRegistration reactionRegistration,
            bool overrideRequired = false)
        {
            reactionRegistration.MustForArg(nameof(reactionRegistration)).NotBeNull();

            this.ReactionRegistration = reactionRegistration;
            this.OverrideRequired = overrideRequired;
        }

        /// <summary>
        /// Gets the <see cref="ReactionRegistration"/> to evaluate.
        /// </summary>
        /// <value>The <see cref="ReactionRegistration"/> to evaluate.</value>
        public ReactionRegistration ReactionRegistration { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [override required].  Treat the reaction dependencies as if everything is optional (e.g. if you are triggering a reaction).
        /// </summary>
        public bool OverrideRequired { get; private set; }
    }
}
