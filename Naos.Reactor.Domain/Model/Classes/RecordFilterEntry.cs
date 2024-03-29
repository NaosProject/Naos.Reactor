﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordFilterEntry.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Entry to use in <see cref="RecordFilterReactorDependency"/>.
    /// </summary>
    public partial class RecordFilterEntry : IHaveStringId, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordFilterEntry"/> class.
        /// </summary>
        /// <param name="id">The identifier of the entry.</param>
        /// <param name="streamRepresentation">The stream representation.</param>
        /// <param name="recordFilter">The record filter.</param>
        /// <param name="requiredForReaction">if set to <c>true</c> [required for reaction].</param>
        /// <param name="includeInReaction">if set to <c>true</c> [include in reaction].</param>
        public RecordFilterEntry(
            string id,
            IStreamRepresentation streamRepresentation,
            RecordFilter recordFilter,
            bool requiredForReaction,
            bool includeInReaction)
        {
            id.MustForArg(nameof(id)).NotBeNullNorWhiteSpace();
            streamRepresentation.MustForArg(nameof(streamRepresentation)).NotBeNull();
            recordFilter.MustForArg(nameof(recordFilter)).NotBeNull();

            this.Id = id;
            this.StreamRepresentation = streamRepresentation;
            this.RecordFilter = recordFilter;
            this.RequiredForReaction = requiredForReaction;
            this.IncludeInReaction = includeInReaction;
        }

        /// <inheritdoc />
        public string Id { get; private set; }

        /// <summary>
        /// Gets the stream representation.
        /// </summary>
        public IStreamRepresentation StreamRepresentation { get; private set; }

        /// <summary>
        /// Gets the record filter.
        /// </summary>
        public RecordFilter RecordFilter { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [required for reaction].
        /// </summary>
        public bool RequiredForReaction { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [include in reaction].
        /// </summary>
        public bool IncludeInReaction { get; private set; }
    }
}
