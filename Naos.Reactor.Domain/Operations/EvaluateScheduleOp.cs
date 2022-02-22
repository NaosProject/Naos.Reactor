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
        /// <param name="evaluationTimestampUtc">The timestamp in UTC to evaluate.</param>
        /// <param name="previousExecutionTimestampUtc">The timestamp in UTC of the previous execution.</param>
        public EvaluateScheduleOp(
            ISchedule schedule,
            DateTime evaluationTimestampUtc,
            DateTime? previousExecutionTimestampUtc)
        {
            schedule.MustForArg(nameof(schedule)).NotBeNull();
            evaluationTimestampUtc.MustForArg(nameof(evaluationTimestampUtc)).BeUtcDateTime();
            previousExecutionTimestampUtc.MustForArg(nameof(previousExecutionTimestampUtc)).BeUtcDateTimeWhenNotNull();

            this.Schedule = schedule;
            this.EvaluationTimestampUtc = evaluationTimestampUtc;
            this.PreviousExecutionTimestampUtc = previousExecutionTimestampUtc;
        }

        /// <summary>
        /// Gets the schedule to evaluate.
        /// </summary>
        public ISchedule Schedule { get; private set; }

        /// <summary>
        /// Gets the timestamp in UTC to evaluate.
        /// </summary>
        public DateTime EvaluationTimestampUtc { get; private set; }

        /// <summary>
        /// Gets the timestamp in UTC of the previous execution.
        /// </summary>
        public DateTime? PreviousExecutionTimestampUtc { get; private set; }
    }
}
