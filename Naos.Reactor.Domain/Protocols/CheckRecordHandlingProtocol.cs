// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckSingleRecordHandlingProtocol.cs" company="Naos Project">
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
    /// Protocol for <see cref="CheckRecordHandlingOp"/>.
    /// </summary>
    public partial class CheckRecordHandlingProtocol : SyncSpecificReturningProtocolBase<CheckRecordHandlingOp, CheckRecordHandlingResult>
    {
        private readonly ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckRecordHandlingProtocol"/> class.
        /// </summary>
        /// <param name="streamFactory">The protocol to get <see cref="IStream"/> from a <see cref="StreamRepresentation"/>.</param>
        public CheckRecordHandlingProtocol(
            ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory)
        {
            streamFactory.MustForArg(nameof(streamFactory)).NotBeNull();
            this.streamFactory = streamFactory;
        }

        /// <inheritdoc />
        public override CheckRecordHandlingResult Execute(
            CheckRecordHandlingOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var concernToHandlingStatusMap = new Dictionary<string, HandlingStatus>();
            var stream = this.streamFactory.Execute(new GetStreamFromRepresentationOp(operation.StreamRepresentation));
            stream.MustForOp("streamMustBeIProtocol").BeOfType<IProtocol>();
            foreach (var concern in operation.Concerns)
            {
                var status = ((IProtocol)stream).ExecuteViaReflection<HandlingStatus>(
                    new GetHandlingStatusOfRecordByInternalRecordIdOp(operation.InternalRecordId, concern));
                concernToHandlingStatusMap.Add(concern, status);
            }

            var result = new CheckRecordHandlingResult(concernToHandlingStatusMap);
            return result;
        }
    }
}
