// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckRecordExistsResult.cs" company="Naos Project">
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
    public partial class CheckRecordExistsResult : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckRecordExistsResult"/> class.
        /// </summary>
        /// <param name="streamRepresentation">Stream results are from.</param>
        /// <param name="recordExists">A value indicating whether the stream has records matching the filter used.</param>
        public CheckRecordExistsResult(
            IStreamRepresentation streamRepresentation,
            bool recordExists)
        {
            streamRepresentation.MustForArg(nameof(streamRepresentation)).NotBeNull();

            this.StreamRepresentation = streamRepresentation;
            this.RecordExists = recordExists;
        }

        /// <summary>
        /// Gets the stream representation.
        /// </summary>
        public IStreamRepresentation StreamRepresentation { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the stream has records matching the filter used.
        /// </summary>
        public bool RecordExists { get; private set; }
    }
}
