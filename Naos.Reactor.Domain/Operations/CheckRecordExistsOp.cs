// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckRecordExistsOp.cs" company="Naos Project">
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
    /// Operation to check the existence of a record filter.
    /// </summary>
    public partial class CheckRecordExistsOp : ReturningOperationBase<CheckRecordExistsResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckRecordExistsOp"/> class.
        /// </summary>
        /// <param name="streamRepresentation">The <see cref="IStreamRepresentation"/> to resolve the <see cref="IStream"/> to check handling on.</param>
        /// <param name="recordFilter">The filter for the records to examine.</param>
        public CheckRecordExistsOp(
            IStreamRepresentation streamRepresentation,
            RecordFilter recordFilter)
        {
            streamRepresentation.MustForArg(nameof(streamRepresentation)).NotBeNull();
            recordFilter.MustForArg(nameof(recordFilter)).NotBeNull();

            this.StreamRepresentation = streamRepresentation;
            this.RecordFilter = recordFilter;
        }

        /// <summary>
        /// Gets the <see cref="IStreamRepresentation"/> to resolve the <see cref="IStream"/> to check handling on.
        /// </summary>
        /// <value>The <see cref="IStreamRepresentation"/> to resolve the <see cref="IStream"/> to check handling on.</value>
        public IStreamRepresentation StreamRepresentation { get; private set; }

        /// <summary>
        /// Gets the filter for the records to examine.
        /// </summary>
        public RecordFilter RecordFilter { get; private set; }
    }
}
