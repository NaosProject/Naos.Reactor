// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RunScheduleProtocol.cs" company="Naos Project">
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
    /// Protocol for <see cref="RunScheduleOp"/>.
    /// </summary>
    public partial class RunScheduleProtocol : SyncSpecificVoidProtocolBase<RunScheduleOp>
    {
        private readonly IStandardStream registeredScheduleStream;
        private readonly ISyncAndAsyncVoidProtocol<ExecuteOpOnScheduleOp> executeOpOnScheduleProtocol;

        /// <summary>
        /// Initializes a new instance of the <see cref="RunScheduleProtocol"/> class.
        /// </summary>
        /// <param name="registeredScheduleStream">The registered reaction stream.</param>
        /// <param name="executeOpOnScheduleProtocol">The evaluate registered reaction protocol.</param>
        public RunScheduleProtocol(
            IStandardStream registeredScheduleStream,
            ISyncAndAsyncVoidProtocol<ExecuteOpOnScheduleOp> executeOpOnScheduleProtocol)
        {
            registeredScheduleStream.MustForArg(nameof(registeredScheduleStream)).NotBeNull();
            executeOpOnScheduleProtocol.MustForArg(nameof(executeOpOnScheduleProtocol)).NotBeNull();

            this.registeredScheduleStream = registeredScheduleStream;
            this.executeOpOnScheduleProtocol = executeOpOnScheduleProtocol;
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public override void Execute(
            RunScheduleOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var getDistinctStringSerializedIdsOp = new StandardGetDistinctStringSerializedIdsOp(
                new RecordFilter(
                    objectTypes: new[]
                                 {
                                     typeof(ExecuteOpRequestedEvent<ExecuteOpOnScheduleOp>).ToRepresentation(),
                                 },
                    deprecatedIdTypes: new[]
                                       {
                                           operation.DeprecatedIdentifierType
                                       }));
            var distinctIds = this.registeredScheduleStream.Execute(getDistinctStringSerializedIdsOp);
            foreach (var distinctId in distinctIds)
            {
                var getLatestRecordOp = new StandardGetLatestRecordOp(
                    new RecordFilter(
                        ids: new[]
                             {
                                 distinctId,
                             }));

                var runOnScheduleRecord = this.registeredScheduleStream.Execute(getLatestRecordOp);
                runOnScheduleRecord
                   .Payload
                   .PayloadTypeRepresentation
                   .MustForOp("recordFromRegisteredScheduleStreamExpectedToBeExecuteOpRequestedOfExecuteOpOnSchedule")
                   .BeEqualTo(typeof(ExecuteOpRequestedEvent<ExecuteOpOnScheduleOp>).ToRepresentation());

                var executeOpOnScheduleOpEvent =
                    runOnScheduleRecord.Payload.DeserializePayloadUsingSpecificFactory<ExecuteOpRequestedEvent<ExecuteOpOnScheduleOp>>(
                        this.registeredScheduleStream.SerializerFactory);
                var executeOpOnScheduleOp = executeOpOnScheduleOpEvent.Operation;
                this.executeOpOnScheduleProtocol.Execute(executeOpOnScheduleOp);
            }
        }
    }
}
