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
        /// <param name="streamRepresentation">Stream results are from.</param>
        /// <param name="internalRecordIdToHandlingStatusMap">The map of the handling concern to <see cref="HandlingStatus"/>.</param>
        public CheckRecordHandlingResult(
            IStreamRepresentation streamRepresentation,
            IReadOnlyDictionary<long, HandlingStatus> internalRecordIdToHandlingStatusMap)
        {
            streamRepresentation.MustForArg(nameof(streamRepresentation)).NotBeNull();
            internalRecordIdToHandlingStatusMap.MustForArg(nameof(internalRecordIdToHandlingStatusMap)).NotBeNullNorEmptyDictionary();

            this.StreamRepresentation = streamRepresentation;
            this.InternalRecordIdToHandlingStatusMap = internalRecordIdToHandlingStatusMap;
        }

        /// <summary>
        /// Gets the stream representation.
        /// </summary>
        public IStreamRepresentation StreamRepresentation { get; private set; }

        /// <summary>
        /// Gets the map of the handling concern to <see cref="HandlingStatus"/>.
        /// </summary>
        /// <value>The map of the handling concern to <see cref="HandlingStatus"/>.</value>
        public IReadOnlyDictionary<long, HandlingStatus> InternalRecordIdToHandlingStatusMap { get; private set; }
    }
}
