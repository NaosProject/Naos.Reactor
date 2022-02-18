// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvaluateScheduleProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

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

            //TODO: do here...
            throw new NotImplementedException();
        }
    }
}
