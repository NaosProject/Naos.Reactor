// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringIdentifiedReactorDependency.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Job dependency interface.
    /// </summary>
    public partial class RecordFilterReactorDependency : IReactorDependency, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordFilterReactorDependency"/> class.
        /// </summary>
        /// <param name="streamRepresentation">The <see cref="IStreamRepresentation"/> that will resolve to a <see cref="IStream"/> that should examined for events to produce a <see cref="ReactionEvent"/>.</param>
        /// <param name="recordFilter">The <see cref="RecordFilter"/> to use when looking for records in the <see cref="IStream"/> from <paramref name="streamRepresentation"/> which, if any new records found, will produce a <see cref="ReactionEvent"/>.</param>
        public RecordFilterReactorDependency(
            IStreamRepresentation streamRepresentation,
            RecordFilter recordFilter)
        {
            streamRepresentation.MustForArg(nameof(streamRepresentation)).NotBeNull();
            recordFilter.MustForArg(nameof(recordFilter)).NotBeNull();

            this.StreamRepresentation = streamRepresentation;
            this.RecordFilter = recordFilter;
        }

        /// <summary>
        /// Gets the <see cref="IStreamRepresentation"/> that will resolve to a <see cref="IStream"/> that should examined for events to produce a <see cref="ReactionEvent"/>.
        /// </summary>
        public IStreamRepresentation StreamRepresentation { get; private set; }

        /// <summary>
        /// Gets the <see cref="RecordFilter"/> to use when looking for records in the <see cref="IStream"/> from <see cref="StreamRepresentation"/> which, if any new records found, will produce a <see cref="ReactionEvent"/>.
        /// </summary>
        public RecordFilter RecordFilter { get; private set; }
    }
}
