// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecuteOpOnScheduleProtocol.cs" company="Naos Project">
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
    /// Protocol for <see cref="EvaluateScheduleOp"/>.
    /// </summary>
    public partial class ExecuteOpOnScheduleProtocol : SyncSpecificReturningProtocolBase<EvaluateScheduleOp, bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteOpOnScheduleProtocol"/> class.
        /// </summary>
        public ExecuteOpOnScheduleProtocol()
        {
        }

        /// <inheritdoc />
        public override bool Execute(
            EvaluateScheduleOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            if (operation.Schedule is DailyScheduleInUtc dailySchedule)
            {
                //TODO: how to calc and ensure it doesn't double publish?
                throw new NotImplementedException();
            }
            else
            {
                throw new NotSupportedException(Invariant($"{nameof(operation)}.{nameof(operation.Schedule)} type '{operation.Schedule.GetType().ToStringReadable()}' is not a supported schedule type."));
            }
        }
    }
}
