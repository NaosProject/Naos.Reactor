// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnyDependenciesReactorDependency.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// A set of dependencies to be evaluated together, will return if any of the dependencies have records to handle.
    /// </summary>
    public partial class AnyDependenciesReactorDependency : IReactorDependency, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllDependenciesReactorDependency"/> class.
        /// </summary>
        /// <param name="dependencies"></param>
        public AnyDependenciesReactorDependency(
            IReadOnlyCollection<IReactorDependency> dependencies)
        {
            dependencies.MustForArg(nameof(dependencies)).NotBeNullNorEmptyEnumerableNorContainAnyNulls();

            this.Dependencies = dependencies;
        }

        /// <summary>
        /// Gets the dependencies.
        /// </summary>
        public IReadOnlyCollection<IReactorDependency> Dependencies { get; private set; }

    }
}
