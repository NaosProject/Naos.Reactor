// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteRecordOnHandlingCompletedProtocol{TId}.cs" company="Naos Project">
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
            foreach (var operationCheckSingleRecordHandlingOp in operation.CheckSingleRecordHandlingOps)
            {
                var result = this.checkSingleRecordHandlingProtocol.Execute(operationCheckSingleRecordHandlingOp);
                results.Add(operationCheckSingleRecordHandlingOp, result);
            }

            if (results.Any())
            {
                var statusByRecordMap = results.Select(
                                                    _ =>
                                                        new KeyValuePair<CheckRecordHandlingOp, HandlingStatus>(
                                                            _.Key,
                                                            _.Value.ConcernToHandlingStatusMap.Values.ToList()
                                                             .ReduceToCompositeHandlingStatus(operation.HandlingStatusCompositionStrategyForConcerns)))
                                               .ToDictionary(k => k.Key, v => v.Value);

                var singleStatus = statusByRecordMap.Values.ReduceToCompositeHandlingStatus(operation.HandlingStatusCompositionStrategyForRecords);

                if (operation.StatusToRecordToWriteMap.TryGetValue(singleStatus, out var action))
                {
                    var targetStream = this.streamFactory.Execute(new GetStreamFromRepresentationOp(action.StreamRepresentation));
                    targetStream.MustForOp("targetStreamMustBeIWriteOnlyStream").BeOfType<IWriteOnlyStream>();

                    ((IWriteOnlyStream)targetStream).PutWithId(action.Id, action.ObjectToPut, action.Tags);
                }
            }
        }
    }
}
