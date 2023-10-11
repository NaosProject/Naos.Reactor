// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandleAndReQueueExecuteOpRequestedProtocol{TOperation}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Handler for a <see cref="ExecuteOpRequestedEvent{TOperation}"/> which will requeue the same request with a sleep before doing so then exiting.
    /// </summary>
    /// <remarks>This allows for perpetual running of logic and also allows for redundant handlers while maintaining single execution.</remarks>
    /// <remarks>Since the event to run itself again is scheduled before exiting then it assures that you will either have a failed, running, or not run event to monitor for issues and keep online.</remarks>
    [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "AndRe", Justification = NaosSuppressBecause.CA1702_CompoundWordsShouldBeCasedCorrectly_AnalyzerIsIncorrectlyDetectingCompoundWords)]
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Re", Justification = NaosSuppressBecause.CA1709_IdentifiersShouldBeCasedCorrectly_CasingIsAsPreferred)]
    public class HandleAndReQueueExecuteOpRequestedProtocol<TOperation> : HandleRecordSyncSpecificProtocolBase<ExecuteOpRequestedEvent<TOperation>>
        where TOperation : IVoidOperation
    {
        /// <summary>
        /// Method signature for <see cref="Func{TResult}" /> that can be optionally provided to prepare the event before re-queuing.
        /// </summary>
        /// <param name="executedOperationEvent">The event containing the operation that was executed.</param>
        /// <param name="executionStartTimestampUtc">Start timestamp in UTC format of the operation execution.</param>
        /// <param name="executionEndTimestampUtc">End timestamp in UTC format of the operation execution.</param>
        /// <returns>Tuple of Operation event to re-queue; augmented if necessary and optional tags for insertion into stream.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Re", Justification = NaosSuppressBecause.CA1709_IdentifiersShouldBeCasedCorrectly_CasingIsAsPreferred)]
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "Name is correct.")]
        public delegate Tuple<ExecuteOpRequestedEvent<TOperation>, IReadOnlyCollection<NamedValue<string>>> PrepareEventBeforeReQueue(
            ExecuteOpRequestedEvent<TOperation> executedOperationEvent,
            DateTime executionStartTimestampUtc,
            DateTime executionEndTimestampUtc);

        private readonly ISyncVoidProtocol<TOperation> executeOperationProtocol;
        private readonly IStandardStream requeueStream;
        private readonly ExistingRecordStrategy existingRecordStrategyOnRequeue;
        private readonly int? recordRetentionCountOnRequeue;
        private readonly PrepareEventBeforeReQueue prepareEventBeforeReQueueFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="RunReactorProtocol"/> class.
        /// </summary>
        /// <param name="requeueStream">The stream to write the <see cref="ExecuteOpRequestedEvent{TId,TOperation}"/> back to.</param>
        /// <param name="executeOperationProtocol">The backing protocol to execute the operation.</param>
        /// <param name="existingRecordStrategyOnRequeue">The strategy to use during the Put of the operation when re-queued.</param>
        /// <param name="recordRetentionCountOnRequeue">The record retention count (if applicable) to use during the Put of the operation when re-queued.</param>
        /// <param name="prepareEventBeforeReQueueFunc">Optional lambda to make changes to the event/wait/perform additional logic/etc before it is re-queued; if null passed then DEFAULT will be to DeepCloneWithTimestampUtc passing DateTime.UtcNow so that the new <see cref="ExecuteOpRequestedEvent{TId,TOperation}" /> will have the correct timestamp.</param>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Requeue", Justification = NaosSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "requeue", Justification = NaosSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Re", Justification = NaosSuppressBecause.CA1709_IdentifiersShouldBeCasedCorrectly_CasingIsAsPreferred)]
        public HandleAndReQueueExecuteOpRequestedProtocol(
            IStandardStream requeueStream,
            ISyncVoidProtocol<TOperation> executeOperationProtocol,
            ExistingRecordStrategy existingRecordStrategyOnRequeue,
            int? recordRetentionCountOnRequeue,
            PrepareEventBeforeReQueue prepareEventBeforeReQueueFunc = null)
        {
            requeueStream.MustForArg(nameof(requeueStream)).NotBeNull();
            executeOperationProtocol.MustForArg(nameof(executeOperationProtocol)).NotBeNull();
            existingRecordStrategyOnRequeue.MustForArg(nameof(existingRecordStrategyOnRequeue)).NotBeEqualTo(ExistingRecordStrategy.Unknown);

            this.requeueStream = requeueStream;
            this.executeOperationProtocol = executeOperationProtocol;
            this.existingRecordStrategyOnRequeue = existingRecordStrategyOnRequeue;
            this.recordRetentionCountOnRequeue = recordRetentionCountOnRequeue;
            this.prepareEventBeforeReQueueFunc = prepareEventBeforeReQueueFunc ?? DefaultPrepareEventBeforeReQueue;
        }

        /// <summary>
        /// Default logic to prepare event before re-queuing; will perform a <see cref="EventBase.DeepCloneWithTimestampUtc" /> with <see cref="DateTime.UtcNow" /> and return the produced event.
        /// </summary>
        /// <param name="executedOperationEvent">The event containing the operation that was executed.</param>
        /// <param name="executionStartTimestampUtc">Start timestamp in UTC format of the operation execution.</param>
        /// <param name="executionEndTimestampUtc">End timestamp in UTC format of the operation execution.</param>
        /// <returns>Event deep-cloned with now timestamp.</returns>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = NaosSuppressBecause.CA1000_DoNotDeclareStaticMembersOnGenericTypes_StaticPropertyReturnsInstanceOfContainingGenericClassAndIsConvenientAndMostDiscoverableWhereDeclared)]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Prefer exact passing to match existing contract.")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Re", Justification = NaosSuppressBecause.CA1709_IdentifiersShouldBeCasedCorrectly_CasingIsAsPreferred)]
        public static Tuple<ExecuteOpRequestedEvent<TOperation>, IReadOnlyCollection<NamedValue<string>>> DefaultPrepareEventBeforeReQueue(
            ExecuteOpRequestedEvent<TOperation> executedOperationEvent,
            DateTime executionStartTimestampUtc,
            DateTime executionEndTimestampUtc)
        {
            var eventWithUpdatedTimestamp = (ExecuteOpRequestedEvent<TOperation>)executedOperationEvent.DeepCloneWithTimestampUtc(DateTime.UtcNow);
            var result = new Tuple<ExecuteOpRequestedEvent<TOperation>, IReadOnlyCollection<NamedValue<string>>>(eventWithUpdatedTimestamp, null);
            return result;
        }

        /// <inheritdoc />
        public override void Execute(
            HandleRecordOp<ExecuteOpRequestedEvent<TOperation>> operation)
        {
            var start = DateTime.UtcNow;
            var operationToExecute = operation.RecordToHandle.Payload.Operation;
            this.executeOperationProtocol.Execute(operationToExecute);
            var end = DateTime.UtcNow;

            var preparedEventAndTagsToRequeue = this.prepareEventBeforeReQueueFunc(operation.RecordToHandle.Payload, start, end);
            var preparedEventToRequeue = preparedEventAndTagsToRequeue.Item1;
            var preparedTagsToRequeue = preparedEventAndTagsToRequeue.Item2;

            this.requeueStream.Put(
                preparedEventToRequeue,
                existingRecordStrategy: this.existingRecordStrategyOnRequeue,
                recordRetentionCount: this.recordRetentionCountOnRequeue,
                tags: preparedTagsToRequeue);
        }
    }
}
