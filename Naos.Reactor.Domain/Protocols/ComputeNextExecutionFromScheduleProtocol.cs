// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvaluateScheduleProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using Naos.Cron;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.DateTime.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Protocol for <see cref="ComputeNextExecutionFromScheduleOp"/>.
    /// </summary>
    public partial class ComputeNextExecutionFromScheduleProtocol : SyncSpecificReturningProtocolBase<ComputeNextExecutionFromScheduleOp, DateTime>
    {
        private readonly Func<DateTime> nowProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputeNextExecutionFromScheduleProtocol"/> class.
        /// </summary>
        public ComputeNextExecutionFromScheduleProtocol(Func<DateTime> nowProvider = null)
        {
            this.nowProvider = nowProvider ?? (() => DateTime.UtcNow);
        }

        /// <inheritdoc />
        public override DateTime Execute(
            ComputeNextExecutionFromScheduleOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();


            var baseTimeRaw = operation.PreviousExecutionTimestampUtc ?? this.nowProvider();
            var baseTime = baseTimeRaw.RoundDownToEvenMinute();
            DateTime result;
            if (operation.Schedule is DailyScheduleInUtc dailySchedule)
            {
                result = baseTime.AdvanceToNextMatchingHourAndMinute(dailySchedule.Hour, dailySchedule.Minute);
            }
            else if (operation.Schedule is HourlySchedule hourlySchedule)
            {
                result = baseTime.AdvanceToNextMatchingMinute(hourlySchedule.Minute);
            }
            else
            {
                throw new NotSupportedException(Invariant($"{nameof(operation)}.{nameof(operation.Schedule)} type '{operation.Schedule.GetType().ToStringReadable()}' is not a supported schedule type."));
            }

            return result;
        }
    }
}
