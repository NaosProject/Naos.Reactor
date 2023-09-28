// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandleAndReQueueExecuteOpRequestedProtocol{TOperation}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using System.Threading;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Cloning.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Handler for a <see cref="ExecuteOpRequestedEvent{TOperation}"/> which will requeue the same request with a sleep before doing so then exiting.
    /// </summary>
    /// <remarks>This allows for perpetual running of logic and also allows for redundant handlers while maintaining single execution.</remarks>
    /// <remarks>Since the event to run itself again is scheduled before exiting then it assures that you will either have a failed, running, or not run event to monitor for issues and keep online.</remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Re", Justification = NaosSuppressBecause.CA1709_IdentifiersShouldBeCasedCorrectly_CasingIsAsPreferred)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "AndRe", Justification = NaosSuppressBecause.CA1702_CompoundWordsShouldBeCasedCorrectly_AnalyzerIsIncorrectlyDetectingCompoundWords)]
    public partial class HandleAndReQueueExecuteOpRequestedProtocol<TOperation> : HandleRecordSyncSpecificProtocolBase<ExecuteOpRequestedEvent<TOperation>>
        where TOperation : IVoidOperation
    {
        private readonly ISyncVoidProtocol<TOperation> executeOperationProtocol;
        private readonly TimeSpan waitTimeBeforeQueuing;
        private readonly IStandardStream requeueStream;
        private readonly ExistingRecordStrategy existingRecordStrategyOnRequeue;
        private readonly int? recordRetentionCountOnRequeue;
        private readonly Func<ExecuteOpRequestedEvent<TOperation>, ExecuteOpRequestedEvent<TOperation>> prepareEventBeforeRequeueFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="RunReactorProtocol"/> class.
        /// </summary>
        /// <param name="requeueStream">The stream to write the <see cref="ExecuteOpRequestedEvent{TId,TOperation}"/> back to.</param>
        /// <param name="executeOperationProtocol">The backing protocol to execute the operation.</param>
        /// <param name="waitTimeBeforeQueuing">The time to wait before operation is re-queued.</param>
        /// <param name="existingRecordStrategyOnRequeue">The strategy to use during the Put of the operation when re-queued.</param>
        /// <param name="recordRetentionCountOnRequeue">The record retention count (if applicable) to use during the Put of the operation when re-queued.</param>
        /// <param name="prepareEventBeforeRequeueFunc">Optional lambda to make changes to the event before it is re-queued; if null passed then DEFAULT will be to DeepCloneWithTimestampUtc passing DateTime.UtcNow so that the new <see cref="ExecuteOpRequestedEvent{TId,TOperation}" /> will have the correct timestamp.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Requeue", Justification = NaosSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "requeue", Justification = NaosSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public HandleAndReQueueExecuteOpRequestedProtocol(
            IStandardStream requeueStream,
            ISyncVoidProtocol<TOperation> executeOperationProtocol,
            TimeSpan waitTimeBeforeQueuing,
            ExistingRecordStrategy existingRecordStrategyOnRequeue,
            int? recordRetentionCountOnRequeue,
            Func<ExecuteOpRequestedEvent<TOperation>, ExecuteOpRequestedEvent<TOperation>> prepareEventBeforeRequeueFunc = null)
        {
            requeueStream.MustForArg(nameof(requeueStream)).NotBeNull();
            executeOperationProtocol.MustForArg(nameof(executeOperationProtocol)).NotBeNull();
            existingRecordStrategyOnRequeue.MustForArg(nameof(existingRecordStrategyOnRequeue)).NotBeEqualTo(ExistingRecordStrategy.Unknown);

            this.requeueStream = requeueStream;
            this.executeOperationProtocol = executeOperationProtocol;
            this.waitTimeBeforeQueuing = waitTimeBeforeQueuing;
            this.existingRecordStrategyOnRequeue = existingRecordStrategyOnRequeue;
            this.recordRetentionCountOnRequeue = recordRetentionCountOnRequeue;
            this.prepareEventBeforeRequeueFunc = prepareEventBeforeRequeueFunc ?? DefaultPrepareEventBeforeRequeue;
        }

        private static ExecuteOpRequestedEvent<TOperation> DefaultPrepareEventBeforeRequeue(
            ExecuteOpRequestedEvent<TOperation> providedOperation)
        {
            return (ExecuteOpRequestedEvent<TOperation>)providedOperation.DeepCloneWithTimestampUtc(DateTime.UtcNow);
        }

        /// <inheritdoc />
        public override void Execute(
            HandleRecordOp<ExecuteOpRequestedEvent<TOperation>> operation)
        {
            var operationToExecute = operation.RecordToHandle.Payload.Operation;
            this.executeOperationProtocol.Execute(operationToExecute);
            Thread.Sleep(this.waitTimeBeforeQueuing);

            var preparedEventToRequeue =
                this.prepareEventBeforeRequeueFunc(operation.RecordToHandle.Payload);

            this.requeueStream.Put(
                preparedEventToRequeue,
                existingRecordStrategy: this.existingRecordStrategyOnRequeue,
                recordRetentionCount: this.recordRetentionCountOnRequeue);
        }
    }
}
