// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReactionFailedEvent.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using System.Collections.Generic;
    using OBeautifulCode.Type;

    /// <summary>
    /// Event indicating that a <see cref="ReactionEvent"/>'s triggered processing has failed.
    /// </summary>
    public partial class ReactionFailedEvent : EventBase<string>, IHaveTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactionFailedEvent"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="timestampUtc">The timestamp of the event in UTC format.</param>
        /// <param name="tags">The tags associated with the reaction.</param>
        public ReactionFailedEvent(
            string id,
            DateTime timestampUtc,
            IReadOnlyCollection<NamedValue<string>> tags = null) : base(id, timestampUtc)
        {
            this.Tags = tags;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }
    }
}
