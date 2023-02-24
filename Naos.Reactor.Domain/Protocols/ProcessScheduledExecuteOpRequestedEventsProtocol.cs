// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessScheduledExecuteOpRequestedEventsProtocol.cs" company="Naos Project">
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
    /// Protocol for <see cref="ProcessScheduledExecuteOpRequestedEventsOp"/>.
    /// </summary>
    public partial class ProcessScheduledExecuteOpRequestedEventsProtocol : SyncSpecificVoidProtocolBase<ProcessScheduledExecuteOpRequestedEventsOp>
    {
        private readonly IReadOnlyStream scheduleExecutionReadStream;
        private readonly IWriteOnlyStream scheduleExecutionWriteStream;
        private readonly IProtocolFactory protocolFactory;
        private readonly ISyncAndAsyncReturningProtocol<ComputeNextExecutionFromScheduleOp, DateTime> evaluateScheduleProtocol;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessScheduledExecuteOpRequestedEventsProtocol"/> class.
        /// </summary>
        /// <param name="scheduleExecutionReadStream">The schedule execution read stream.</param>
        /// <param name="scheduleExecutionWriteStream">The schedule execution write stream.</param>
        /// <param name="protocolFactory">The factory to determine the appropriate protocol to execute the scheduled operation.</param>
        /// <param name="evaluateScheduleProtocol">The protocol to evaluate schedules.</param>
        public ProcessScheduledExecuteOpRequestedEventsProtocol(
            IReadOnlyStream scheduleExecutionReadStream,
            IWriteOnlyStream scheduleExecutionWriteStream,
            IProtocolFactory protocolFactory,
            ISyncAndAsyncReturningProtocol<ComputeNextExecutionFromScheduleOp, DateTime> evaluateScheduleProtocol)
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
            ProcessScheduledExecuteOpRequestedEventsOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
        }
    }
}
