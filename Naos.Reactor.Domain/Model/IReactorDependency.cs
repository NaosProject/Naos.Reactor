// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReactorDependency.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using Naos.Database.Domain;

    /// <summary>
    /// Job dependency interface.
    /// </summary>
    public interface IReactorDependency
    {
        /// <summary>
        /// Gets the <see cref="IStreamRepresentation"/> that will resolve to a <see cref="IStream"/> that should examined for events to produce a <see cref="ReactionEvent"/>.
        /// </summary>
        /// <value>The <see cref="IStreamRepresentation"/> that will resolve to a <see cref="IStream"/> that should examined for events to produce a <see cref="ReactionEvent"/>.</value>
        IStreamRepresentation StreamRepresentation { get; }

        /// <summary>
        /// Gets the <see cref="TryHandleRecordOp"/> to execute against the <see cref="IStream"/> which, if any events are handled, will produce a <see cref="ReactionEvent"/>.
        /// </summary>
        /// <value>The <see cref="TryHandleRecordOp"/> to execute against the <see cref="IStream"/> which, if any events are handled, will produce a <see cref="ReactionEvent"/>.</value>
        StandardTryHandleRecordOp TryHandleRecordOp { get; }
    }
}
