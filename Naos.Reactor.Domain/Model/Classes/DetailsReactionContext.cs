// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DetailsReactionContext.cs" company="Naos Project">
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
    /// Result of <see cref="CheckRecordExistsOp"/>.
    /// </summary>
    public partial class DetailsReactionContext : IReactionContext, IHaveDetails, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DetailsReactionContext"/> class.
        /// </summary>
        /// <param name="details">The details.</param>
        public DetailsReactionContext(
            string details)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
            this.Details = details;
        }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
