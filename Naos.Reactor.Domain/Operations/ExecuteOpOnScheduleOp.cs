// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecuteOpOnScheduleOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using Naos.Cron;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Operation to execute another operation on a schedule.
    /// </summary>
    public partial class ExecuteOpOnScheduleOp : VoidOperationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckRecordExistsOp"/> class.
        /// </summary>
        /// <param name="operation">The operation to run on a schedule.</param>
        /// <param name="schedule">The schedule to execute the operation on.</param>
        public ExecuteOpOnScheduleOp(
            IVoidOperation operation,// => WriteEventToStream
            ISchedule schedule)
        {
            this.Operation = operation;
            this.Schedule = schedule;
            operation.MustForArg(nameof(operation)).NotBeNull();
            schedule.MustForArg(nameof(schedule)).NotBeNull();
        }

        /// <summary>
        /// Gets the operation.
        /// </summary>
        public IVoidOperation Operation { get; private set; }

        /// <summary>
        /// Gets the schedule.
        /// </summary>
        public ISchedule Schedule { get; private set; }
    }
}
