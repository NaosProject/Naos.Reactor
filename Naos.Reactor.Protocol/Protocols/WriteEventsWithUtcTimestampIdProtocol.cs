// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteEventsWithUtcTimestampIdProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Protocol
{
    using System;
    using Naos.Database.Domain;
    using Naos.Reactor.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.String.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;
    using IStream = Naos.Database.Domain.IStream;

    /// <summary>
    /// Protocol for <see cref="WriteEventsWithUtcTimestampIdOp"/>.
    /// </summary>
    public partial class WriteEventsWithUtcTimestampIdProtocol : SyncSpecificVoidProtocolBase<WriteEventsWithUtcTimestampIdOp>
    {
        private readonly ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteEventsWithUtcTimestampIdProtocol"/> class.
        /// </summary>
        /// <param name="streamFactory">The protocol to get <see cref="IStream"/> from a <see cref="StreamRepresentation"/>.</param>
        public WriteEventsWithUtcTimestampIdProtocol(
            ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory)
        {
            streamFactory.MustForArg(nameof(streamFactory)).NotBeNull();
            this.streamFactory = streamFactory;
        }

        /// <inheritdoc />
        public override void Execute(
            WriteEventsWithUtcTimestampIdOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var utcNow = DateTime.UtcNow;
            var identifier = operation.IdPrefix + utcNow.ToStringInvariantPreferred();
            foreach (var eventToPutWithId in operation.EventsToPut)
            {
                var targetStream = this.streamFactory.Execute(new GetStreamFromRepresentationOp(eventToPutWithId.StreamRepresentation));
                targetStream.MustForOp("targetStreamMustBeIWriteOnlyStream").BeAssignableToType<IWriteOnlyStream>();
                var eventBase = eventToPutWithId.EventToPut as EventBase<string>;
                eventBase
                   .MustForOp(Invariant($"{nameof(eventToPutWithId)}.{nameof(eventToPutWithId.EventToPut)}"))
                   .NotBeNull(
                        Invariant(
                            $"Only {nameof(EventBase<string>)} is supported, this was {eventToPutWithId.EventToPut.GetType().ToStringReadable()}."));

                // ReSharper disable once PossibleNullReferenceException - checked with Must above
                var eventToPut = eventBase.DeepCloneWithId(identifier);

                if (eventToPutWithId.UpdateTimestampOnPut)
                {
                    eventToPut = (EventBase<string>)eventToPut.DeepCloneWithTimestampUtc(utcNow);
                }

                ((IWriteOnlyStream)targetStream).PutWithId(eventToPutWithId.Id, eventToPut, eventToPutWithId.Tags);
            }
        }
    }
}
