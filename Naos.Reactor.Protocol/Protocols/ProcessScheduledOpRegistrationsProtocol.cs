// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessScheduledOpRegistrationsProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Protocol
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using Naos.Reactor.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.String.Recipes;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Protocol for <see cref="ProcessScheduledOpRegistrationsOp"/>.
    /// </summary>
    public partial class ProcessScheduledOpRegistrationsProtocol : SyncSpecificVoidProtocolBase<ProcessScheduledOpRegistrationsOp>
    {
        private readonly IStandardStream registeredScheduleStream;
        private readonly ISyncAndAsyncReturningProtocol<ComputePreviousExecutionFromScheduleOp, DateTime?> computePreviousExecutionFromScheduleProtocol;
        private readonly ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory;
        private readonly TimeSpan timeThresholdToScheduleAnExecution;
        private readonly Func<DateTime> nowProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessScheduledOpRegistrationsProtocol"/> class.
        /// </summary>
        /// <param name="registeredScheduleStream">The registered reaction stream.</param>
        /// <param name="computePreviousExecutionFromScheduleProtocol">The protocol to evaluate schedules into execution times.</param>
        /// <param name="streamFactory">The factory to provide target stream for events yielded by schedule evaluation.</param>
        /// <param name="timeThresholdToScheduleAnExecution">The amount of time after a target execution time that the operation will still be scheduled (unless the <see cref="ScheduledOpRegistration.ScheduleImmediatelyWhenMissed" /> is set to true in which case it is ignored).</param>
        /// <param name="nowProvider">The optional provider for "now"; default is <see cref="DateTime.UtcNow" />.</param>
        public ProcessScheduledOpRegistrationsProtocol(
            IStandardStream registeredScheduleStream,
            ISyncAndAsyncReturningProtocol<ComputePreviousExecutionFromScheduleOp, DateTime?> computePreviousExecutionFromScheduleProtocol,
            ISyncAndAsyncReturningProtocol<GetStreamFromRepresentationOp, IStream> streamFactory,
            TimeSpan timeThresholdToScheduleAnExecution,
            Func<DateTime> nowProvider = null)
        {
            registeredScheduleStream.MustForArg(nameof(registeredScheduleStream)).NotBeNull();
            computePreviousExecutionFromScheduleProtocol.MustForArg(nameof(computePreviousExecutionFromScheduleProtocol)).NotBeNull();
            streamFactory.MustForArg(nameof(streamFactory)).NotBeNull();
            timeThresholdToScheduleAnExecution.MustForArg(nameof(timeThresholdToScheduleAnExecution)).BeGreaterThan(TimeSpan.Zero);

            this.registeredScheduleStream = registeredScheduleStream;
            this.computePreviousExecutionFromScheduleProtocol = computePreviousExecutionFromScheduleProtocol;
            this.streamFactory = streamFactory;
            this.timeThresholdToScheduleAnExecution = timeThresholdToScheduleAnExecution;
            this.nowProvider = nowProvider ?? (() => DateTime.UtcNow);
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public override void Execute(
            ProcessScheduledOpRegistrationsOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var scheduledOpRegistrationIds = this.registeredScheduleStream
                                                 .GetDistinctIds<string>(
                                                      deprecatedIdTypes: operation.DeprecatedIdentifierType == null
                                                          ? null
                                                          : new[]
                                                            {
                                                                operation.DeprecatedIdentifierType,
                                                            });

            var referenceTimestampUtc = this.nowProvider();
            foreach (var registrationId in scheduledOpRegistrationIds)
            {
                var registration = this.registeredScheduleStream.GetLatestObjectById<string, ScheduledOpRegistration>(registrationId);
                var previousTimeOp = new ComputePreviousExecutionFromScheduleOp(registration.Schedule, referenceTimestampUtc);
                var previousExecutionTime = this.computePreviousExecutionFromScheduleProtocol.Execute(previousTimeOp);
                if (previousExecutionTime != null
                  && (referenceTimestampUtc.Subtract((DateTime)previousExecutionTime) <= this.timeThresholdToScheduleAnExecution
                      || registration.ScheduleImmediatelyWhenMissed))
                {
                    var eventId = BuildEventId(registration.Id, (DateTime)previousExecutionTime);
                    var targetStreamOp = new GetStreamFromRepresentationOp(registration.StreamRepresentation);
                    var targetStreamRaw = this.streamFactory.Execute(targetStreamOp);
                    var targetStream = (IStandardStream)targetStreamRaw;
                    var existingRecord = targetStream.GetLatestRecordById<string, ScheduledExecuteOpRequestedEvent>(eventId);
                    if (existingRecord == null)
                    {
                        void WriteRecord(bool skipExecution, bool hasFailure)
                        {
                            var recordTags = (registration.Tags ?? new List<NamedValue<string>>())
                                            .Concat(
                                                 new[]
                                                 {
                                                     new NamedValue<string>(TagNames.ScheduledOpRegistrationId, registration.Id),
                                                     new NamedValue<string>(TagNames.ScheduledOpExecutionSkipped, skipExecution.ToStringInvariantPreferred()),
                                                     new NamedValue<string>(TagNames.ScheduledOpExecutionInFailedState, hasFailure.ToStringInvariantPreferred()),
                                                 })
                                            .ToList();

                            var recordOperation = skipExecution ? new NullVoidOp() : registration.OperationToExecute;

                            var eventToWrite = new ScheduledExecuteOpRequestedEvent(
                                eventId,
                                recordOperation,
                                (DateTime)previousExecutionTime,
                                referenceTimestampUtc,
                                registration.Details,
                                recordTags);

                            targetStream.PutWithId(eventToWrite.Id, eventToWrite, eventToWrite.Tags, ExistingRecordStrategy.ThrowIfFoundById);
                        }

                        var existingRecordStatusOp = new GetCompositeHandlingStatusByTagsOp(
                            Concerns.DefaultExecutionConcern,
                            new[]
                            {
                                new NamedValue<string>(TagNames.ScheduledOpRegistrationId, registration.Id),
                            });

                        var existingRecordStatus = targetStream
                                                  .GetStreamRecordHandlingProtocols()
                                                  .Execute(existingRecordStatusOp);

                        if (existingRecordStatus.HasFlag(CompositeHandlingStatus.SomeFailed))
                        {
                            WriteRecord(skipExecution: true, hasFailure: true);
                        }
                        else if (registration.ScheduledOpAlreadyRunningStrategy == ScheduledOpAlreadyRunningStrategy.ExecuteNewInParallel)
                        {
                            WriteRecord(skipExecution: false, hasFailure: false);
                        }
                        else if (registration.ScheduledOpAlreadyRunningStrategy == ScheduledOpAlreadyRunningStrategy.Skip)
                        {
                            if (existingRecordStatus.HasFlag(CompositeHandlingStatus.SomeRunning))
                            {
                                WriteRecord(skipExecution: true, hasFailure: false);
                            }
                            else
                            {
                                WriteRecord(skipExecution: false, hasFailure: false);
                            }
                        }
                        else
                        {
                            throw new NotSupportedException(
                                Invariant(
                                    $"Unsupported {nameof(ScheduledOpAlreadyRunningStrategy)} '{registration.ScheduledOpAlreadyRunningStrategy}'."));
                        }
                    }
                }
            }
        }

        private static string BuildEventId(string registrationId, DateTime executionTime)
        {
            var result = Invariant($"{registrationId}___{executionTime.ToString("yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture)}Z");
            return result;
        }
    }
}
