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
    /// Operation to check the handling status of a record.
    /// </summary>
    public class CheckRecordHandlingOp : ReturningOperationBase<CheckRecordHandlingResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckRecordHandlingOp"/> class.
        /// </summary>
        /// <param name="streamRepresentation">The <see cref="IStreamRepresentation"/> to resolve the <see cref="IStream"/> to check handling on.</param>
        /// <param name="concerns">The concerns that require handling.</param>
        /// <param name="internalRecordId">The internal record identifier of the record to examine.</param>
        public CheckRecordHandlingOp(
            IStreamRepresentation streamRepresentation,
            IReadOnlyCollection<string> concerns,
            long internalRecordId)
        {
            streamRepresentation.MustForArg(nameof(streamRepresentation)).NotBeNull();
            concerns.MustForArg(nameof(concerns)).NotBeNullNorEmptyEnumerable();

            this.StreamRepresentation = streamRepresentation;
            this.Concerns = concerns;
            this.InternalRecordId = internalRecordId;
        }

        /// <summary>
        /// Gets the <see cref="IStreamRepresentation"/> to resolve the <see cref="IStream"/> to check handling on.
        /// </summary>
        /// <value>The <see cref="IStreamRepresentation"/> to resolve the <see cref="IStream"/> to check handling on.</value>
        public IStreamRepresentation StreamRepresentation { get; private set; }

        /// <summary>
        /// Gets the concerns that require handling.
        /// </summary>
        /// <value>The concerns that require handling.</value>
        public IReadOnlyCollection<string> Concerns { get; private set; }

        /// <summary>
        /// Gets the internal record identifier of the record to examine.
        /// </summary>
        /// <value>The internal record identifier of the record to examine.</value>
        public long InternalRecordId { get; private set; }
    }
}
