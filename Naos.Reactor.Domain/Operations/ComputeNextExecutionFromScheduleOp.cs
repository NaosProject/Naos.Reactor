// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComputeNextExecutionFromScheduleOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using Naos.Cron;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Operation to check a schedule against a prior time and current time, returns next execution time.
    /// </summary>
    public partial class ComputeNextExecutionFromScheduleOp : ReturningOperationBase<DateTime>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComputeNextExecutionFromScheduleOp"/> class.
        /// </summary>
        /// <param name="schedule">The schedule to evaluate.</param>
        /// <param name="previousExecutionTimestampUtc">The timestamp in UTC of the previous execution.</param>
        public ComputeNextExecutionFromScheduleOp(
            ISchedule schedule,
            DateTime? previousExecutionTimestampUtc)
        {
            schedule.MustForArg(nameof(schedule)).NotBeNull();
            previousExecutionTimestampUtc.MustForArg(nameof(previousExecutionTimestampUtc)).BeUtcDateTimeWhenNotNull();

            this.Schedule = schedule;
            this.PreviousExecutionTimestampUtc = previousExecutionTimestampUtc;
        }

        /// <summary>
        /// Gets the schedule to evaluate.
        /// </summary>
        public ISchedule Schedule { get; private set; }

        /// <summary>
        /// Gets the timestamp in UTC of the previous execution.
        /// </summary>
        public DateTime? PreviousExecutionTimestampUtc { get; private set; }
    }
}
