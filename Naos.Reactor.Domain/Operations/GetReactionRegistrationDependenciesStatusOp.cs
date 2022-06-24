// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetReactionRegistrationDependenciesStatusOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System.Collections.Generic;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the reaction registration dependencies report, a map of single dependency entry identifiers to the record to handling status map.
    /// </summary>
    public partial class GetReactionRegistrationDependenciesStatusOp : ReturningOperationBase<IReadOnlyDictionary<string, IReadOnlyDictionary<long, HandlingStatus>>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetReactionRegistrationDependenciesStatusOp"/> class.
        /// </summary>
        /// <param name="reactionRegistration">The reaction registration.</param>
        public GetReactionRegistrationDependenciesStatusOp(
            ReactionRegistration reactionRegistration)
        {
            reactionRegistration.MustForArg(nameof(reactionRegistration)).NotBeNull();

            this.ReactionRegistration = reactionRegistration;
        }

        /// <summary>
        /// Gets the reaction registration.
        /// </summary>
        public ReactionRegistration ReactionRegistration { get; private set; }
    }
}
