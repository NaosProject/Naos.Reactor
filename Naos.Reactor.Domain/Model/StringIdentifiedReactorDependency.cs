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
    public class StringIdentifiedReactorDependency : IReactorDependency, IHaveStringId, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringIdentifiedReactorDependency"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="streamRepresentation">The <see cref="IStreamRepresentation"/> that will resolve to a <see cref="IStream"/> that should examined for events to produce a <see cref="ReactionEvent"/>.</param>
        /// <param name="tryHandleRecordOp">The <see cref="TryHandleRecordOp"/> to execute against the <see cref="IStream"/> which, if any events are handled, will produce a <see cref="ReactionEvent"/>.</param>
        public StringIdentifiedReactorDependency(
            string id,
            IStreamRepresentation streamRepresentation,
            StandardTryHandleRecordOp tryHandleRecordOp)
        {
            id.MustForArg(nameof(id)).NotBeNullNorWhiteSpace();
            streamRepresentation.MustForArg(nameof(streamRepresentation)).NotBeNull();
            tryHandleRecordOp.MustForArg(nameof(tryHandleRecordOp)).NotBeNull();

            this.Id = id;
            this.StreamRepresentation = streamRepresentation;
            this.TryHandleRecordOp = tryHandleRecordOp;
        }

        /// <inheritdoc />
        public string Id { get; private set; }

        /// <inheritdoc />
        public IStreamRepresentation StreamRepresentation { get; private set; }

        /// <inheritdoc />
        public StandardTryHandleRecordOp TryHandleRecordOp { get; private set; }
    }
}
