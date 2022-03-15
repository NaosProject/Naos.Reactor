﻿// --------------------------------------------------------------------------------------------------------------------
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
    public partial class ExecuteOpOnScheduleOp : VoidOperationBase, IHaveStringId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckRecordExistsOp"/> class.
        /// </summary>
        /// <param name="id">The identifier of the scheduled operation.</param>
        /// <param name="operation">The operation to run on a schedule.</param>
        /// <param name="schedule">The schedule to execute the operation on.</param>
        public ExecuteOpOnScheduleOp(
            string id,
            IVoidOperation operation,
            ISchedule schedule)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            schedule.MustForArg(nameof(schedule)).NotBeNull();

            this.Id = id;
            this.Operation = operation;
            this.Schedule = schedule;
        }

        /// <inheritdoc />
        public string Id { get; private set; }

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