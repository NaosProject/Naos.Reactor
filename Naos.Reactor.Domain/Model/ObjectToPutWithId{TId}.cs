// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectToPutWithId{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System.Collections.Generic;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// An object with associated information to be put into a stream using the <see cref="IStreamRepresentation"/> as the target.
    /// </summary>
    public partial class ObjectToPutWithId<TId> : IModelViaCodeGen, IHaveId<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectToPutWithId{TId}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="streamRepresentation">The stream representation.</param>
        /// <param name="tags">The optional tags.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public ObjectToPutWithId(
            TId id,
            object objectToPut,
            IStreamRepresentation streamRepresentation,
            IReadOnlyCollection<NamedValue<string>> tags = null)
        {
            streamRepresentation.MustForArg(nameof(streamRepresentation)).NotBeNull();

            this.Id = id;
            this.ObjectToPut = objectToPut;
            this.StreamRepresentation = streamRepresentation;
            this.Tags = tags;
        }

        /// <inheritdoc />
        public TId Id { get; private set; }

        /// <summary>
        /// Gets the object to put.
        /// </summary>
        /// <value>The object to put.</value>
        public object ObjectToPut { get; private set; }

        /// <summary>
        /// Gets the stream representation.
        /// </summary>
        /// <value>The stream representation.</value>
        public IStreamRepresentation StreamRepresentation { get; private set; }

        /// <summary>
        /// Gets the tags.
        /// </summary>
        /// <value>The tags.</value>
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }
    }
}
