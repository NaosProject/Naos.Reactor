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
    using OBeautifulCode.Representation.System;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="RunReactorProtocol"/> class.
        /// </summary>
        /// <param name="requeueStream">The stream to write the <see cref="ExecuteOpRequestedEvent{TId,TOperation}"/> back to.</param>
        /// <param name="executeOperationProtocol">The backing protocol to execute the operation.</param>
        /// <param name="waitTimeBeforeQueuing">The time to wait before requeue the .</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "requeue", Justification = NaosSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public HandleAndReQueueExecuteOpRequestedProtocol(
            IStandardStream requeueStream,
            ISyncVoidProtocol<TOperation> executeOperationProtocol,
            TimeSpan waitTimeBeforeQueuing)
        {
            requeueStream.MustForArg(nameof(requeueStream)).NotBeNull();
            executeOperationProtocol.MustForArg(nameof(executeOperationProtocol)).NotBeNull();

            this.requeueStream = requeueStream;
            this.executeOperationProtocol = executeOperationProtocol;
            this.waitTimeBeforeQueuing = waitTimeBeforeQueuing;
        }

        /// <inheritdoc />
        public override void Execute(
            HandleRecordOp<ExecuteOpRequestedEvent<TOperation>> operation)
        {
            var operationToExecute = operation.RecordToHandle.Payload.Operation;
            this.executeOperationProtocol.Execute(operationToExecute);
            Thread.Sleep(this.waitTimeBeforeQueuing);

            var operationClone = operation.DeepClone();
            this.requeueStream.Put(operationClone);
        }
    }
}
