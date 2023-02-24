// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessExecuteOnScheduleEventsProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// Protocol for <see cref="ProcessScheduledOpRegistrationsOp"/>.
    /// </summary>
    public partial class ProcessScheduledOpRegistrationsProtocol : SyncSpecificVoidProtocolBase<ProcessScheduledOpRegistrationsOp>
    {
        private readonly IStandardStream registeredScheduleStream;
        private readonly ISyncAndAsyncVoidProtocol<ProcessScheduledExecuteOpRequestedEventsOp> executeOpOnScheduleProtocol;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessScheduledOpRegistrationsProtocol"/> class.
        /// </summary>
        /// <param name="registeredScheduleStream">The registered reaction stream.</param>
        /// <param name="executeOpOnScheduleProtocol">The evaluate registered reaction protocol.</param>
        public ProcessScheduledOpRegistrationsProtocol(
            IStandardStream registeredScheduleStream,
            ISyncAndAsyncVoidProtocol<ProcessScheduledExecuteOpRequestedEventsOp> executeOpOnScheduleProtocol)
        {
            registeredScheduleStream.MustForArg(nameof(registeredScheduleStream)).NotBeNull();
            executeOpOnScheduleProtocol.MustForArg(nameof(executeOpOnScheduleProtocol)).NotBeNull();

            this.registeredScheduleStream = registeredScheduleStream;
            this.executeOpOnScheduleProtocol = executeOpOnScheduleProtocol;
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public override void Execute(
            ProcessScheduledOpRegistrationsOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var executeOnScheduleEvents = this.registeredScheduleStream
                                              .GetDistinctIds<ScheduledOpRegistration>(
                deprecatedIdTypes: new[]
                                   {
                                       operation.DeprecatedIdentifierType,
                                   });
        }
    }
}
