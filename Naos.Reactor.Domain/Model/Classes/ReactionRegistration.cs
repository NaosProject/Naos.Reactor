﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReactionRegistration.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Registered reaction criteria to evaluate for possible <see cref="ReactionEvent"/>'s.
    /// </summary>
    public partial class ReactionRegistration : IModelViaCodeGen, IHaveStringId, IHaveTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactionRegistration"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="reactionContext">The context to provide to the <see cref="ReactionEvent"/> when written.</param>
        /// <param name="dependencies">The dependencies to check; any dependencies will produce a <see cref="ReactionEvent"/>.</param>
        /// <param name="idealWaitTimeBetweenEvaluations">The ideal time to wait between evaluations.</param>
        /// <param name="tags">The tags to write on the <see cref="ReactionEvent"/>.</param>
        public ReactionRegistration(
            string id,
            IReactionContext reactionContext,
            IReadOnlyList<IReactorDependency> dependencies,
            TimeSpan idealWaitTimeBetweenEvaluations,
            IReadOnlyCollection<NamedValue<string>> tags = null)
        {
            id.MustForArg(nameof(id)).NotBeNullNorWhiteSpace();
            reactionContext.MustForArg(nameof(reactionContext)).NotBeNull();
            dependencies.MustForArg(nameof(dependencies)).NotBeNullNorEmptyEnumerableNorContainAnyNulls();

            this.Id = id;
            this.ReactionContext = reactionContext;
            this.Dependencies = dependencies;
            this.IdealWaitTimeBetweenEvaluations = idealWaitTimeBetweenEvaluations;
            this.Tags = tags;
        }

        /// <inheritdoc />
        public string Id { get; private set; }

        /// <summary>
        /// Gets the reaction context to provide to the <see cref="ReactionEvent"/>.
        /// </summary>
        public IReactionContext ReactionContext { get; private set; }

        /// <summary>
        /// Gets the dependencies to check; any dependencies will produce a <see cref="ReactionEvent"/>..
        /// </summary>
        public IReadOnlyList<IReactorDependency> Dependencies { get; private set; }

        /// <summary>
        /// Gets the ideal time to wait between evaluations.
        /// </summary>
        public TimeSpan IdealWaitTimeBetweenEvaluations { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }
    }
}
