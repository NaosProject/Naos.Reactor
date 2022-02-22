// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecuteOpOnScheduleProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using Naos.Cron;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Protocol for <see cref="ExecuteOpOnScheduleOp"/>.
    /// </summary>
    public partial class ExecuteOpOnScheduleProtocol : SyncSpecificVoidProtocolBase<ExecuteOpOnScheduleOp>
    {
        private readonly IReadOnlyStream scheduleExecutionReadStream;
        private readonly IWriteOnlyStream scheduleExecutionWriteStream;
        private readonly IProtocolFactory protocolFactory;
        private readonly ISyncAndAsyncReturningProtocol<EvaluateScheduleOp, bool> evaluateScheduleProtocol;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteOpOnScheduleProtocol"/> class.
        /// </summary>
        /// <param name="scheduleExecutionReadStream">The schedule execution read stream.</param>
        /// <param name="scheduleExecutionWriteStream">The schedule execution write stream.</param>
        /// <param name="protocolFactory">The factory to determine the appropriate protocol to execute the scheduled operation.</param>
        /// <param name="evaluateScheduleProtocol">The protocol to evaluate schedules.</param>
        public ExecuteOpOnScheduleProtocol(
            IReadOnlyStream scheduleExecutionReadStream,
            IWriteOnlyStream scheduleExecutionWriteStream,
            IProtocolFactory protocolFactory,
            ISyncAndAsyncReturningProtocol<EvaluateScheduleOp, bool> evaluateScheduleProtocol)
        {
            scheduleExecutionReadStream.MustForArg(nameof(scheduleExecutionReadStream)).NotBeNull();
            scheduleExecutionWriteStream.MustForArg(nameof(scheduleExecutionWriteStream)).NotBeNull();
            protocolFactory.MustForArg(nameof(protocolFactory)).NotBeNull();
            evaluateScheduleProtocol.MustForArg(nameof(evaluateScheduleProtocol)).NotBeNull();

            this.scheduleExecutionReadStream = scheduleExecutionReadStream;
            this.scheduleExecutionWriteStream = scheduleExecutionWriteStream;
            this.protocolFactory = protocolFactory;
            this.evaluateScheduleProtocol = evaluateScheduleProtocol;
        }

        /// <inheritdoc />
        public override void Execute(
            ExecuteOpOnScheduleOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var previousExecution = this.scheduleExecutionReadStream.GetLatestObjectById<string, ScheduledExecutionEvent>(operation.Id);

            var utcNow = DateTime.Now;
            var evaluateScheduleOp = new EvaluateScheduleOp(operation.Schedule, utcNow, previousExecution?.TimestampUtc);
            var evaluationResult = this.evaluateScheduleProtocol.Execute(evaluateScheduleOp);
            if (evaluationResult)
            {
                var protocol = this.protocolFactory.Execute(new GetProtocolOp(operation.Operation));
                protocol.ExecuteViaReflection(operation.Operation);
                var executedEvent = new ScheduledExecutionEvent(operation.Id, operation.Operation, operation.Schedule, evaluateScheduleOp.PreviousExecutionTimestampUtc, utcNow);
                this.scheduleExecutionWriteStream.PutWithId(operation.Id, executedEvent);
            }
        }
    }
}
