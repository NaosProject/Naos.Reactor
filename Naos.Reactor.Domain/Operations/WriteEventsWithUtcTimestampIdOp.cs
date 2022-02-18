// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteEventsWithUtcTimestampIdOp.cs" company="Naos Project">
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
    /// Operation to write a set of events to various streams with a prefix and timestamp in UTC format as the identifier for all events.
    /// </summary>
    public partial class WriteEventsWithUtcTimestampIdOp : VoidOperationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteEventsWithUtcTimestampIdOp"/> class.
        /// </summary>
        /// <param name="idPrefix">The identifier prefix to append the timestamp in UTC onto.</param>
        /// <param name="eventsToPut">The events to put in streams.</param>
        public WriteEventsWithUtcTimestampIdOp(
            string idPrefix,
            IReadOnlyCollection<EventToPutWithId<string>> eventsToPut)
        {
            this.IdPrefix = idPrefix;
            this.EventsToPut = eventsToPut;
        }

        /// <summary>
        /// Gets the identifier prefix to append the timestamp in UTC onto.
        /// </summary>
        public string IdPrefix { get; private set; }

        /// <summary>
        /// Gets the events to put in streams.
        /// </summary>
        public IReadOnlyCollection<EventToPutWithId<string>> EventsToPut { get; private set; }
    }
}
