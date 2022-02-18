// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvaluateScheduleOp.cs" company="Naos Project">
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
    /// Operation to check a schedule against a prior time and current time, returns TRUE if schedule was triggered.
    /// </summary>
    public partial class EvaluateScheduleOp : ReturningOperationBase<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluateScheduleOp"/> class.
        /// </summary>
        /// <param name="schedule">The schedule to evaluate.</param>
        /// <param name="utcTimeToEvaluate">The timestamp in UTC to evaluate.</param>
        /// <param name="utcTimePreviousExecution">The timestamp in UTC of the previous execution.</param>
        public EvaluateScheduleOp(
            ISchedule schedule,
            DateTime utcTimeToEvaluate,
            DateTime? utcTimePreviousExecution)
        {
            schedule.MustForArg(nameof(schedule)).NotBeNull();
            utcTimeToEvaluate.MustForArg(nameof(utcTimeToEvaluate)).BeUtcDateTime();
            utcTimePreviousExecution.MustForArg(nameof(utcTimePreviousExecution)).BeUtcDateTimeWhenNotNull();

            this.Schedule = schedule;
            this.UtcTimeToEvaluate = utcTimeToEvaluate;
            this.UtcTimePreviousExecution = utcTimePreviousExecution;
        }

        /// <summary>
        /// Gets the schedule to evaluate.
        /// </summary>
        public ISchedule Schedule { get; private set; }

        /// <summary>
        /// Gets the timestamp in UTC to evaluate.
        /// </summary>
        public DateTime UtcTimeToEvaluate { get; private set; }

        /// <summary>
        /// Gets the timestamp in UTC of the previous execution.
        /// </summary>
        public DateTime? UtcTimePreviousExecution { get; private set; }
    }
}
