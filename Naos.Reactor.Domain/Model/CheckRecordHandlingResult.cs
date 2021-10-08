// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckRecordHandlingResult.cs" company="Naos Project">
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
    /// Result of <see cref="CheckRecordHandlingOp"/>.
    /// </summary>
    public partial class CheckRecordHandlingResult : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckRecordHandlingResult"/> class.
        /// </summary>
        /// <param name="concernToHandlingStatusMap">The map of the handling concern to <see cref="HandlingStatus"/>.</param>
        public CheckRecordHandlingResult(
            IReadOnlyDictionary<string, HandlingStatus> concernToHandlingStatusMap)
        {
            concernToHandlingStatusMap.MustForArg(nameof(concernToHandlingStatusMap)).NotBeNullNorEmptyDictionary();

            this.ConcernToHandlingStatusMap = concernToHandlingStatusMap;
        }

        /// <summary>
        /// Gets the map of the handling concern to <see cref="HandlingStatus"/>.
        /// </summary>
        /// <value>The map of the handling concern to <see cref="HandlingStatus"/>.</value>
        public IReadOnlyDictionary<string, HandlingStatus> ConcernToHandlingStatusMap { get; private set; }
    }
}
