// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReactorDependency.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Registered reaction criteria to evaluate for possible <see cref="Reaction"/>'s.
    /// </summary>
    public partial class RegisteredReaction : IModelViaCodeGen, IHaveStringId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredReaction"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="dependencies">The dependencies.</param>
        public RegisteredReaction(
            string id,
            IReadOnlyList<IReactorDependency> dependencies)
        {
            id.MustForArg(nameof(id)).NotBeNullNorWhiteSpace();
            dependencies.MustForArg(nameof(dependencies)).NotBeNullNorEmptyEnumerable();

            this.Id = id;
            this.Dependencies = dependencies;
        }

        /// <inheritdoc />
        public string Id { get; private set; }

        /// <summary>
        /// Gets the dependencies.
        /// </summary>
        /// <value>The dependencies.</value>
        public IReadOnlyList<IReactorDependency> Dependencies { get; private set; }
    }
}
