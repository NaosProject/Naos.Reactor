// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckRecordHandlingOp.cs" company="Naos Project">
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
    /// Operation to check the handling status of a record filter.
    /// </summary>
    public partial class CheckRecordHandlingOp : ReturningOperationBase<CheckRecordHandlingResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckRecordHandlingOp"/> class.
        /// </summary>
        /// <param name="streamRepresentation">The <see cref="IStreamRepresentation"/> to resolve the <see cref="IStream"/> to check handling on.</param>
        /// <param name="concern">The concerns that require handling.</param>
        /// <param name="recordFilter">The filter for the records to examine.</param>
        /// <param name="handlingFilter">The filter for the handling entries to examine.</param>
        /// <param name="expectedCount">Optional expected count of records to be found via filter.</param>
        public CheckRecordHandlingOp(
            IStreamRepresentation streamRepresentation,
            string concern,
            RecordFilter recordFilter,
            HandlingFilter handlingFilter,
            int? expectedCount = null)
        {
            streamRepresentation.MustForArg(nameof(streamRepresentation)).NotBeNull();
            concern.MustForArg(nameof(concern)).NotBeNullNorWhiteSpace();
            recordFilter.MustForArg(nameof(recordFilter)).NotBeNull();
            handlingFilter.MustForArg(nameof(handlingFilter)).NotBeNull();

            this.StreamRepresentation = streamRepresentation;
            this.Concern = concern;
            this.RecordFilter = recordFilter;
            this.HandlingFilter = handlingFilter;
            this.ExpectedCount = expectedCount;
        }

        /// <summary>
        /// Gets the <see cref="IStreamRepresentation"/> to resolve the <see cref="IStream"/> to check handling on.
        /// </summary>
        /// <value>The <see cref="IStreamRepresentation"/> to resolve the <see cref="IStream"/> to check handling on.</value>
        public IStreamRepresentation StreamRepresentation { get; private set; }

        /// <summary>
        /// Gets the concerns that require handling.
        /// </summary>
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the filter for the records to examine.
        /// </summary>
        public RecordFilter RecordFilter { get; private set; }

        /// <summary>
        /// Gets the filter for the handling entries to examine.
        /// </summary>
        public HandlingFilter HandlingFilter { get; private set; }

        /// <summary>
        /// Gets the optional expected count of records to be found via filter.
        /// </summary>
        public int? ExpectedCount { get; private set; }
    }
}
