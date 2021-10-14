// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvaluateRegisteredReactionOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Operation to evaluate a <see cref="RegisteredReaction"/> and get any applicable.
    /// <remarks>Null is the expected result if there are no records handled.</remarks>
    /// </summary>
    public partial class EvaluateRegisteredReactionOp : ReturningOperationBase<ReactionEvent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluateRegisteredReactionOp"/> class.
        /// </summary>
        /// <param name="registeredReaction">The <see cref="RegisteredReaction"/> to evaluate.</param>
        public EvaluateRegisteredReactionOp(
            RegisteredReaction registeredReaction)
        {
            registeredReaction.MustForArg(nameof(registeredReaction)).NotBeNull();

            this.RegisteredReaction = registeredReaction;
        }

        /// <summary>
        /// Gets the <see cref="RegisteredReaction"/> to evaluate.
        /// </summary>
        /// <value>The <see cref="RegisteredReaction"/> to evaluate.</value>
        public RegisteredReaction RegisteredReaction { get; private set; }
    }
}
