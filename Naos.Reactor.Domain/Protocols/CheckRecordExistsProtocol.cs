// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckRecordExistsProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Protocol for <see cref="CheckRecordExistsOp"/>.
    /// </summary>
    public partial class CheckRecordExistsProtocol : SyncSpecificReturningProtocolBase<CheckRecordExistsOp, CheckRecordExistsResult>
    {
        private readonly ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckRecordExistsProtocol"/> class.
        /// </summary>
        /// <param name="streamFactory">The protocol to get <see cref="IStream"/> from a <see cref="StreamRepresentation"/>.</param>
        public CheckRecordExistsProtocol(
            ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory)
        {
            streamFactory.MustForArg(nameof(streamFactory)).NotBeNull();
            this.streamFactory = streamFactory;
        }

        /// <inheritdoc />
        public override CheckRecordExistsResult Execute(
            CheckRecordExistsOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var stream = this.streamFactory.Execute(new GetStreamFromRepresentationOp(operation.StreamRepresentation));
            stream.MustForOp(nameof(stream))
                  .BeAssignableToType<ISyncReturningProtocol<StandardGetHandlingStatusOp, IReadOnlyDictionary<long, HandlingStatus>>>();
            var streamProtocol = (ISyncReturningProtocol<StandardGetLatestRecordOp, StreamRecord>)stream;

            var getLatestRecordOp = new StandardGetLatestRecordOp(
                operation.RecordFilter,
                streamRecordItemsToInclude: StreamRecordItemsToInclude.MetadataOnly);
            var latestMatchingRecord = streamProtocol.Execute(getLatestRecordOp);

            var result = new CheckRecordExistsResult(operation.StreamRepresentation, latestMatchingRecord != null);
            return result;
        }
    }
}
