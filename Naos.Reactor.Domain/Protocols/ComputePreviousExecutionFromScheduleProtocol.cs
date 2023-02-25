// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComputePreviousExecutionFromScheduleProtocol.cs" company="Naos Project">
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
    /// Protocol for <see cref="ComputePreviousExecutionFromScheduleOp"/>.
    /// </summary>
    public partial class ComputePreviousExecutionFromScheduleProtocol : SyncSpecificReturningProtocolBase<ComputePreviousExecutionFromScheduleOp, DateTime>
    {
        /// <inheritdoc />
        public override DateTime Execute(
            ComputePreviousExecutionFromScheduleOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();


            var baseTimeRaw = operation.ReferenceTimestampUtc;
            var baseTime = baseTimeRaw.RewindToEvenMinute();
            DateTime result;
            if (operation.Schedule is DailyScheduleInUtc dailySchedule)
            {
                result = baseTime.RewindToNextMatchingHourAndMinute(dailySchedule.Hour, dailySchedule.Minute);
            }
            else if (operation.Schedule is HourlySchedule hourlySchedule)
            {
                result = baseTime.RewindToNextMatchingMinute(hourlySchedule.Minute);
            }
            else
            {
                throw new NotSupportedException(Invariant($"{nameof(operation)}.{nameof(operation.Schedule)} type '{operation.Schedule.GetType().ToStringReadable()}' is not a supported schedule type."));
            }

            return result;
        }
    }
}
