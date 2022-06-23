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
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Protocol for <see cref="EvaluateScheduleOp"/>.
    /// </summary>
    public partial class EvaluateScheduleProtocol : SyncSpecificReturningProtocolBase<EvaluateScheduleOp, bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluateScheduleProtocol"/> class.
        /// </summary>
        public EvaluateScheduleProtocol()
        {
        }

        /// <inheritdoc />
        public override bool Execute(
            EvaluateScheduleOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            if (operation.Schedule is DailyScheduleInUtc dailySchedule)
            {
                var result = (operation.PreviousExecutionTimestampUtc == null
                           || (operation.EvaluationTimestampUtc.Subtract((DateTime)operation.PreviousExecutionTimestampUtc) >= TimeSpan.FromDays(1)))
                          && dailySchedule.Hour   <= operation.EvaluationTimestampUtc.Hour
                          && dailySchedule.Minute <= operation.EvaluationTimestampUtc.Minute;

                return result;
            }
            else
            {
                throw new NotSupportedException(Invariant($"{nameof(operation)}.{nameof(operation.Schedule)} type '{operation.Schedule.GetType().ToStringReadable()}' is not a supported schedule type."));
            }
        }
    }
}
