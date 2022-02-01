// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteRecordOnHandlingCompletedProtocol{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Check on sagas, write records under certain handling scenarios of groups of records.
    /// </summary>
    /// <typeparam name="TId">Type of the identifier.</typeparam>
    public partial class WriteRecordOnHandlingCompletedProtocol<TId> : SyncSpecificVoidProtocolBase<WriteRecordOnHandlingCompletedOp<TId>>
    {
        private readonly ISyncAndAsyncReturningProtocol<CheckRecordHandlingOp, CheckRecordHandlingResult> checkSingleRecordHandlingProtocol;
        private readonly ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteRecordOnHandlingCompletedProtocol{TId}"/> class.
        /// </summary>
        /// <param name="checkSingleRecordHandlingProtocol">The check single record handling protocol.</param>
        /// <param name="streamFactory">The protocol to get <see cref="IStream"/> from a <see cref="StreamRepresentation"/>.</param>
        public WriteRecordOnHandlingCompletedProtocol(
            ISyncAndAsyncReturningProtocol<CheckRecordHandlingOp, CheckRecordHandlingResult> checkSingleRecordHandlingProtocol,
            ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory)
        {
            this.checkSingleRecordHandlingProtocol = checkSingleRecordHandlingProtocol;
            this.streamFactory = streamFactory;
        }

        /// <inheritdoc />
        public override void Execute(
            WriteRecordOnHandlingCompletedOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var results = new Dictionary<CheckRecordHandlingOp, CheckRecordHandlingResult>();
            foreach (var operationCheckSingleRecordHandlingOp in operation.CheckRecordHandlingOps)
            {
                var result = this.checkSingleRecordHandlingProtocol.Execute(operationCheckSingleRecordHandlingOp);
                results.Add(operationCheckSingleRecordHandlingOp, result);
            }

            if (results.Any())
            {
                var compositeHandlingStatus = results
                                             .SelectMany(_ => _.Value.InternalRecordIdToHandlingStatusMap.Values)
                                             .ToList()
                                             .ToCompositeHandlingStatus();

                if (operation.StatusToRecordToWriteMap.TryGetValue(compositeHandlingStatus, out var action))
                {
                    var targetStream = this.streamFactory.Execute(new GetStreamFromRepresentationOp(action.StreamRepresentation));
                    targetStream.MustForOp("targetStreamMustBeIWriteOnlyStream").BeOfType<IWriteOnlyStream>();

                    var objectToPut = action.UpdateTimestampOnPut ? action.EventToPut.DeepCloneWithTimestampUtc(DateTime.UtcNow) : action.EventToPut;
                    ((IWriteOnlyStream)targetStream).PutWithId(action.Id, objectToPut, action.Tags);
                }
            }
        }
    }
}
