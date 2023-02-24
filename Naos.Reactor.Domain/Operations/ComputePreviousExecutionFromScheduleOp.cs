// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComputePreviousExecutionFromScheduleOp.cs" company="Naos Project">
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
    /// Operation to check a schedule a reference time, returns the prior execution time.
    /// </summary>
    public partial class ComputePreviousExecutionFromScheduleOp : ReturningOperationBase<DateTime>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComputePreviousExecutionFromScheduleOp"/> class.
        /// </summary>
        /// <param name="schedule">The schedule to evaluate.</param>
        /// <param name="referenceTimestampUtc">The reference timestamp to compute from in UTC.</param>
        public ComputePreviousExecutionFromScheduleOp(
            ISchedule schedule,
            DateTime? referenceTimestampUtc)
        {
            schedule.MustForArg(nameof(schedule)).NotBeNull();
            referenceTimestampUtc.MustForArg(nameof(referenceTimestampUtc)).BeUtcDateTimeWhenNotNull();

            this.Schedule = schedule;
            this.ReferenceTimestampUtc = referenceTimestampUtc;
        }

        /// <summary>
        /// Gets the schedule to evaluate.
        /// </summary>
        public ISchedule Schedule { get; private set; }

        /// <summary>
        /// Gets the timestamp in UTC to base the computation on.
        /// </summary>
        public DateTime? ReferenceTimestampUtc { get; private set; }
    }
}
