// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectToPutWithId{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// An object with associated information to be put into a stream using the <see cref="IStreamRepresentation"/> as the target.
    /// </summary>
    public partial class EventToPutWithId<TId> : IModelViaCodeGen, IHaveId<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventToPutWithId{TId}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="eventToPut">The event to put in the stream retrieved by <see cref="StreamRepresentation"/>.</param>
        /// <param name="streamRepresentation">The stream representation to retrieve the stream to write the <paramref name="eventToPut"/> into.</param>
        /// <param name="updateTimestampOnPut">The optional switch to deep clone the <paramref name="eventToPut"/> with a new <see cref="DateTime.UtcNow"/>.</param>
        /// <param name="tags">The optional tags.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public EventToPutWithId(
            TId id,
            EventBase eventToPut,
            IStreamRepresentation streamRepresentation,
            bool updateTimestampOnPut = true,
            IReadOnlyCollection<NamedValue<string>> tags = null)
        {
            streamRepresentation.MustForArg(nameof(streamRepresentation)).NotBeNull();
            eventToPut.MustForArg(nameof(eventToPut)).NotBeNull();

            this.Id = id;
            this.EventToPut = eventToPut;
            this.StreamRepresentation = streamRepresentation;
            this.UpdateTimestampOnPut = updateTimestampOnPut;
            this.Tags = tags;
        }

        /// <inheritdoc />
        public TId Id { get; private set; }

        /// <summary>
        /// Gets the event to put in the stream retrieved by <see cref="StreamRepresentation"/>.
        /// </summary>
        public EventBase EventToPut { get; private set; }

        /// <summary>
        /// Gets the stream representation to retrieve the stream to write the <see cref="EventToPut"/> into.
        /// </summary>
        public IStreamRepresentation StreamRepresentation { get; private set; }

        /// <summary>
        /// Gets a value indicating to deep clone the <see cref="EventToPut"/> with a new <see cref="DateTime.UtcNow"/>.
        /// </summary>
        public bool UpdateTimestampOnPut { get; private set; }

        /// <summary>
        /// Gets the tags.
        /// </summary>
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }
    }
}
