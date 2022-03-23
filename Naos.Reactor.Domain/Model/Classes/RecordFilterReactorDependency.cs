// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordFilterReactorDependency.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Entry to use in <see cref="RecordFilterReactorDependency"/>.
    /// </summary>
    public partial class RecordFilterReactorDependency : IReactorDependency, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordFilterReactorDependency"/> class.
        /// </summary>
        /// <param name="entries">The entries to evaluate.</param>
        public RecordFilterReactorDependency(
            IReadOnlyList<RecordFilterEntry> entries)
        {
            entries.MustForArg(nameof(entries)).NotBeNullNorEmptyEnumerableNorContainAnyNulls();

            this.Entries = entries;
        }

        /// <summary>
        /// Gets the entries to evaluate.
        /// </summary>
        public IReadOnlyList<RecordFilterEntry> Entries { get; private set; }
    }
}