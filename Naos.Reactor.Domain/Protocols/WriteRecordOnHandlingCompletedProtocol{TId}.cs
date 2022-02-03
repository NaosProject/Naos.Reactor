// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteRecordOnHandlingCompletedProtocol{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

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
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
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

                    IEvent eventToPut;
                    if (action.UpdateTimestampOnPut)
                    {
                        var eventBase = action.EventToPut as EventBase;
                        eventBase
                           .MustForOp(Invariant($"{nameof(action)}.{nameof(action.EventToPut)}"))
                           .NotBeNull(Invariant($"Only {nameof(EventBase)} is supported, this was {action.EventToPut.GetType().ToStringReadable()}."));

                        // ReSharper disable once PossibleNullReferenceException - checked with Must above
                        eventToPut = eventBase.DeepCloneWithTimestampUtc(DateTime.Now);
                    }
                    else
                    {
                        eventToPut = action.EventToPut;
                    }

                    ((IWriteOnlyStream)targetStream).PutWithId(action.Id, eventToPut, action.Tags);
                }
            }
        }
    }
}
