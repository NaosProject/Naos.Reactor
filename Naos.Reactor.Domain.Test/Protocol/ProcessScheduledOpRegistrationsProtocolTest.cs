// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessScheduledOpRegistrationsProtocolTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CLAP;
    using Naos.Cron;
    using Naos.Database.Domain;
    using Naos.Reactor.Serialization.Json;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Type;
    using Xunit;

    public static class ProcessScheduledOpRegistrationsProtocolTest
    {
        [Fact]
        public static void ProcessScheduledOpRegistrationsProtocol___Execute_hourly_schedule___Write_events()
        {
            var registrationStream = new MemoryStandardStream(
                "ScheduledOpRegistrations",
                new SerializerRepresentation(SerializationKind.Json, typeof(ReactorJsonSerializationConfiguration).ToRepresentation()),
                SerializationFormat.String,
                new ObcSimplifyingSerializerFactory(new JsonSerializerFactory()));

            var eventStream = new MemoryStandardStream(
                "ScheduledOpEvents",
                new SerializerRepresentation(SerializationKind.Json, typeof(ReactorJsonSerializationConfiguration).ToRepresentation()),
                SerializationFormat.String,
                new ObcSimplifyingSerializerFactory(new JsonSerializerFactory()));

            var schedule = new HourlySchedule
                           {
                               Minute = 1,
                           };

            var scheduledOpRegistration = new ScheduledOpRegistration(
                "hourly-at-01-after",
                new ThrowOpExecutionAbortedExceptionOp("Just for testing."),
                schedule,
                eventStream.StreamRepresentation,
                ScheduledOpAlreadyRunningStrategy.Skip);

            registrationStream.PutWithId(scheduledOpRegistration.Id, scheduledOpRegistration);

            var streamFactory = new GetStreamFromRepresentationByNameProtocolFactory(
                new Dictionary<string, Func<IStream>>
                {
                    { eventStream.Name, () => eventStream },
                });

            var realNow = DateTime.UtcNow;
            var dateCounter = -1;
            var dates = new[]
                        {
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 1, 1, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 1, 2, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 1, 3, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 2, 1, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 2, 2, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 2, 3, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 3, 1, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 3, 2, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 3, 3, 0, DateTimeKind.Utc),
                        };

            DateTime NowProvider()
            {
                dateCounter = dateCounter + 1;
                return dates[dateCounter];
            }

            var op = new ProcessScheduledOpRegistrationsOp();
            var computeProtocol = new ComputePreviousExecutionFromScheduleProtocol();
            var protocol = new ProcessScheduledOpRegistrationsProtocol(
                registrationStream,
                computeProtocol,
                streamFactory,
                TimeSpan.FromMinutes(5),
                NowProvider);

            // 0101
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(0);

            // 0102
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(1);

            // 0103
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(1);

            // 0201
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(1);

            // 0202
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(2);

            // 0203
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(2);

            // 0301
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(2);

            // 0302
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(3);

            // 0303
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(3);
        }

        [Fact]
        public static void ProcessScheduledOpRegistrationsProtocol___Execute_hourly_schedule___Skips_events_when_set_and_running()
        {
            var registrationStream = new MemoryStandardStream(
                "ScheduledOpRegistrations",
                new SerializerRepresentation(SerializationKind.Json, typeof(ReactorJsonSerializationConfiguration).ToRepresentation()),
                SerializationFormat.String,
                new ObcSimplifyingSerializerFactory(new JsonSerializerFactory()));

            var eventStream = new MemoryStandardStream(
                "ScheduledOpEvents",
                new SerializerRepresentation(SerializationKind.Json, typeof(ReactorJsonSerializationConfiguration).ToRepresentation()),
                SerializationFormat.String,
                new ObcSimplifyingSerializerFactory(new JsonSerializerFactory()));

            var schedule = new HourlySchedule
                           {
                               Minute = 1,
                           };

            var scheduledOpRegistration = new ScheduledOpRegistration(
                "hourly-at-01-after",
                new ThrowOpExecutionAbortedExceptionOp("Just for testing."),
                schedule,
                eventStream.StreamRepresentation,
                ScheduledOpAlreadyRunningStrategy.Skip);

            registrationStream.PutWithId(scheduledOpRegistration.Id, scheduledOpRegistration);

            var streamFactory = new GetStreamFromRepresentationByNameProtocolFactory(
                new Dictionary<string, Func<IStream>>
                {
                    { eventStream.Name, () => eventStream },
                });

            var realNow = DateTime.UtcNow;
            var dateCounter = -1;
            var dates = new[]
                        {
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 1, 1, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 1, 2, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 1, 3, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 2, 1, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 2, 2, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 2, 3, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 3, 1, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 3, 2, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 3, 3, 0, DateTimeKind.Utc),
                        };

            DateTime NowProvider()
            {
                dateCounter = dateCounter + 1;
                return dates[dateCounter];
            }

            var op = new ProcessScheduledOpRegistrationsOp();
            var computeProtocol = new ComputePreviousExecutionFromScheduleProtocol();
            var protocol = new ProcessScheduledOpRegistrationsProtocol(
                registrationStream,
                computeProtocol,
                streamFactory,
                TimeSpan.FromMinutes(5),
                NowProvider);

            // 0101
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(0);

            // 0102
            protocol.Execute(op);
            var recordIds1 = eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter()));
            recordIds1.MustForTest().HaveCount(1);
            var latestRecord1 = eventStream.Execute(
                new StandardGetLatestRecordOp(
                    new RecordFilter(
                        internalRecordIds: new[]
                                           {
                                               recordIds1.Max(),
                                           })));
            var operationInRecord1 =
                latestRecord1.Payload.DeserializePayloadUsingSpecificFactory<ScheduledExecuteOpRequestedEvent>(eventStream.SerializerFactory);
            operationInRecord1.OperationToExecute.MustForTest().BeOfType(scheduledOpRegistration.OperationToExecute.GetType());
            latestRecord1.Metadata.Tags.Single(_ => _.Name == TagNames.ScheduledOpExecutionSkipped).Value
                         .MustForTest()
                         .BeEqualTo(false.ToString().ToLowerInvariant());
            latestRecord1.Metadata.Tags.Single(_ => _.Name == TagNames.ScheduledOpExecutionInFailedState)
                         .Value.MustForTest()
                         .BeEqualTo(false.ToString().ToLowerInvariant());

            // 0103
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(1);

            // 0201
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(1);

            // 0202
            eventStream.Execute(
                new StandardTryHandleRecordOp(
                    Concerns.DefaultExecutionConcern,
                    new RecordFilter(
                        new[]
                        {
                            latestRecord1.InternalRecordId,
                        })));
            protocol.Execute(op);
            var recordIds2 = eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter()));
            recordIds2.MustForTest().HaveCount(2);
            var latestRecord2 = eventStream.Execute(
                new StandardGetLatestRecordOp(
                    new RecordFilter(
                        internalRecordIds: new[]
                                           {
                                               recordIds2.Max(),
                                           })));
            var operationInRecord2 =
                latestRecord2.Payload.DeserializePayloadUsingSpecificFactory<ScheduledExecuteOpRequestedEvent>(eventStream.SerializerFactory);
            operationInRecord2.OperationToExecute.MustForTest().BeOfType(typeof(NullVoidOp));
            latestRecord2.Metadata.Tags.Single(_ => _.Name == TagNames.ScheduledOpExecutionSkipped).Value
                         .MustForTest()
                         .BeEqualTo(true.ToString().ToLowerInvariant());
            latestRecord2.Metadata.Tags.Single(_ => _.Name == TagNames.ScheduledOpExecutionInFailedState)
                         .Value.MustForTest()
                         .BeEqualTo(false.ToString().ToLowerInvariant());
            eventStream.GetStreamRecordHandlingProtocols()
                       .Execute(new CompleteRunningHandleRecordOp(latestRecord1.InternalRecordId, Concerns.DefaultExecutionConcern));

            // 0203
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(2);

            // 0301
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(2);

            // 0302
            protocol.Execute(op);
            var recordIds3 = eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter()));
            recordIds3.MustForTest().HaveCount(3);
            eventStream.Execute(
                            new StandardGetLatestRecordOp(
                                new RecordFilter(
                                    internalRecordIds: new[]
                                                       {
                                                           recordIds3.Max(),
                                                       })))
                       .Payload.DeserializePayloadUsingSpecificFactory<ScheduledExecuteOpRequestedEvent>(eventStream.SerializerFactory)
                       .OperationToExecute.MustForTest()
                       .BeOfType(scheduledOpRegistration.OperationToExecute.GetType());

            // 0303
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(3);
        }

        [Fact]
        public static void ProcessScheduledOpRegistrationsProtocol___Execute_hourly_schedule___Skips_events_when_in_failed_state()
        {
            var registrationStream = new MemoryStandardStream(
                "ScheduledOpRegistrations",
                new SerializerRepresentation(SerializationKind.Json, typeof(ReactorJsonSerializationConfiguration).ToRepresentation()),
                SerializationFormat.String,
                new ObcSimplifyingSerializerFactory(new JsonSerializerFactory()));

            var eventStream = new MemoryStandardStream(
                "ScheduledOpEvents",
                new SerializerRepresentation(SerializationKind.Json, typeof(ReactorJsonSerializationConfiguration).ToRepresentation()),
                SerializationFormat.String,
                new ObcSimplifyingSerializerFactory(new JsonSerializerFactory()));

            var schedule = new HourlySchedule
                           {
                               Minute = 1,
                           };

            var scheduledOpRegistration = new ScheduledOpRegistration(
                "hourly-at-01-after",
                new ThrowOpExecutionAbortedExceptionOp("Just for testing."),
                schedule,
                eventStream.StreamRepresentation,
                ScheduledOpAlreadyRunningStrategy.Skip);

            registrationStream.PutWithId(scheduledOpRegistration.Id, scheduledOpRegistration);

            var streamFactory = new GetStreamFromRepresentationByNameProtocolFactory(
                new Dictionary<string, Func<IStream>>
                {
                    { eventStream.Name, () => eventStream },
                });

            var realNow = DateTime.UtcNow;
            var dateCounter = -1;
            var dates = new[]
                        {
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 1, 1, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 1, 2, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 1, 3, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 2, 1, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 2, 2, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 2, 3, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 3, 1, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 3, 2, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 3, 3, 0, DateTimeKind.Utc),
                        };

            DateTime NowProvider()
            {
                dateCounter = dateCounter + 1;
                return dates[dateCounter];
            }

            var op = new ProcessScheduledOpRegistrationsOp();
            var computeProtocol = new ComputePreviousExecutionFromScheduleProtocol();
            var protocol = new ProcessScheduledOpRegistrationsProtocol(
                registrationStream,
                computeProtocol,
                streamFactory,
                TimeSpan.FromMinutes(5),
                NowProvider);

            // 0101
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(0);

            // 0102
            protocol.Execute(op);
            var recordIds1 = eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter()));
            recordIds1.MustForTest().HaveCount(1);
            var latestRecord1 = eventStream.Execute(
                new StandardGetLatestRecordOp(
                    new RecordFilter(
                        internalRecordIds: new[]
                                           {
                                               recordIds1.Max(),
                                           })));
            var operationInRecord1 =
                latestRecord1.Payload.DeserializePayloadUsingSpecificFactory<ScheduledExecuteOpRequestedEvent>(eventStream.SerializerFactory);
            operationInRecord1.OperationToExecute.MustForTest().BeOfType(scheduledOpRegistration.OperationToExecute.GetType());
            latestRecord1.Metadata.Tags.Single(_ => _.Name == TagNames.ScheduledOpExecutionSkipped).Value
                         .MustForTest()
                         .BeEqualTo(false.ToString().ToLowerInvariant());
            latestRecord1.Metadata.Tags.Single(_ => _.Name == TagNames.ScheduledOpExecutionInFailedState)
                         .Value.MustForTest()
                         .BeEqualTo(false.ToString().ToLowerInvariant());

            // 0103
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(1);

            // 0201
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(1);

            // 0202
            eventStream.Execute(
                new StandardTryHandleRecordOp(
                    Concerns.DefaultExecutionConcern,
                    new RecordFilter(
                        new[]
                        {
                            latestRecord1.InternalRecordId,
                        })));
            eventStream.GetStreamRecordHandlingProtocols()
                       .Execute(new FailRunningHandleRecordOp(latestRecord1.InternalRecordId, Concerns.DefaultExecutionConcern, "Simulate failure."));
            protocol.Execute(op);
            var recordIds2 = eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter()));
            recordIds2.MustForTest().HaveCount(2);
            var latestRecord2 = eventStream.Execute(
                new StandardGetLatestRecordOp(
                    new RecordFilter(
                        internalRecordIds: new[]
                                           {
                                               recordIds2.Max(),
                                           })));
            var operationInRecord2 =
                latestRecord2.Payload.DeserializePayloadUsingSpecificFactory<ScheduledExecuteOpRequestedEvent>(eventStream.SerializerFactory);
            operationInRecord2.OperationToExecute.MustForTest().BeOfType(typeof(NullVoidOp));
            latestRecord2.Metadata.Tags.Single(_ => _.Name == TagNames.ScheduledOpExecutionSkipped)
                         .Value.MustForTest()
                         .BeEqualTo(true.ToString().ToLowerInvariant());
            latestRecord2.Metadata.Tags.Single(_ => _.Name == TagNames.ScheduledOpExecutionInFailedState)
                         .Value.MustForTest()
                         .BeEqualTo(true.ToString().ToLowerInvariant());
            eventStream.GetStreamRecordHandlingProtocols()
                       .Execute(new ArchiveFailureToHandleRecordOp(latestRecord1.InternalRecordId, Concerns.DefaultExecutionConcern, "Simulate failure addressed."));

            // 0203
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(2);

            // 0301
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(2);

            // 0302
            protocol.Execute(op);
            var recordIds3 = eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter()));
            recordIds3.MustForTest().HaveCount(3);
            eventStream.Execute(
                            new StandardGetLatestRecordOp(
                                new RecordFilter(
                                    internalRecordIds: new[]
                                                       {
                                                           recordIds3.Max(),
                                                       })))
                       .Payload.DeserializePayloadUsingSpecificFactory<ScheduledExecuteOpRequestedEvent>(eventStream.SerializerFactory)
                       .OperationToExecute.MustForTest()
                       .BeOfType(scheduledOpRegistration.OperationToExecute.GetType());

            // 0303
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(3);
        }

        [Fact]
        public static void ProcessScheduledOpRegistrationsProtocol___Execute_daily_schedule___Write_events()
        {
            var registrationStream = new MemoryStandardStream(
                "ScheduledOpRegistrations",
                new SerializerRepresentation(SerializationKind.Json, typeof(ReactorJsonSerializationConfiguration).ToRepresentation()),
                SerializationFormat.String,
                new ObcSimplifyingSerializerFactory(new JsonSerializerFactory()));

            var eventStream = new MemoryStandardStream(
                "ScheduledOpEvents",
                new SerializerRepresentation(SerializationKind.Json, typeof(ReactorJsonSerializationConfiguration).ToRepresentation()),
                SerializationFormat.String,
                new ObcSimplifyingSerializerFactory(new JsonSerializerFactory()));

            var schedule = new DailyScheduleInUtc()
                           {
                               Hour = 3,
                               Minute = 1,
                           };

            var scheduledOpRegistration = new ScheduledOpRegistration(
                "daily-at-0301Z",
                new ThrowOpExecutionAbortedExceptionOp("Just for testing."),
                schedule,
                eventStream.StreamRepresentation,
                ScheduledOpAlreadyRunningStrategy.Skip);

            registrationStream.PutWithId(scheduledOpRegistration.Id, scheduledOpRegistration);

            var streamFactory = new GetStreamFromRepresentationByNameProtocolFactory(
                new Dictionary<string, Func<IStream>>
                {
                    { eventStream.Name, () => eventStream },
                });

            var realNow = DateTime.UtcNow;
            var dateCounter = -1;
            var dates = new[]
                        {
                            new DateTime(realNow.Year, realNow.Month, 1, 3, 1, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, 1, 3, 2, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, 1, 3, 3, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, 2, 3, 1, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, 2, 3, 2, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, 2, 3, 3, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, 3, 3, 1, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, 3, 3, 2, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, 3, 3, 3, 0, DateTimeKind.Utc),
                        };

            DateTime NowProvider()
            {
                dateCounter = dateCounter + 1;
                return dates[dateCounter];
            }

            var op = new ProcessScheduledOpRegistrationsOp();
            var computeProtocol = new ComputePreviousExecutionFromScheduleProtocol();
            var protocol = new ProcessScheduledOpRegistrationsProtocol(
                registrationStream,
                computeProtocol,
                streamFactory,
                TimeSpan.FromMinutes(2),
                NowProvider);

            // 1-0301
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(0);

            // 1-0302
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(1);

            // 1-0303
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(1);

            // 2-0301
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(1);

            // 2-0302
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(2);

            // 2-0303
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(2);

            // 3-0301
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(2);

            // 3-0302
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(3);

            // 3-0303
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(3);
        }

        [Fact]
        public static void ProcessScheduledOpRegistrationsProtocol___Execute_specific_time_schedule___Write_events()
        {
            var registrationStream = new MemoryStandardStream(
                "ScheduledOpRegistrations",
                new SerializerRepresentation(SerializationKind.Json, typeof(ReactorJsonSerializationConfiguration).ToRepresentation()),
                SerializationFormat.String,
                new ObcSimplifyingSerializerFactory(new JsonSerializerFactory()));

            var eventStream = new MemoryStandardStream(
                "ScheduledOpEvents",
                new SerializerRepresentation(SerializationKind.Json, typeof(ReactorJsonSerializationConfiguration).ToRepresentation()),
                SerializationFormat.String,
                new ObcSimplifyingSerializerFactory(new JsonSerializerFactory()));

            var realNow = DateTime.UtcNow;
            var schedule = new SpecificDateTimeScheduleInUtc()
                           {
                               SpecificDateTimeInUtc = new DateTime(realNow.Year, realNow.Month, realNow.Day, 1, 1, 0, DateTimeKind.Utc),
                           };

            var scheduledOpRegistration = new ScheduledOpRegistration(
                "specific-time",
                new ThrowOpExecutionAbortedExceptionOp("Just for testing."),
                schedule,
                eventStream.StreamRepresentation,
                ScheduledOpAlreadyRunningStrategy.Skip);

            registrationStream.PutWithId(scheduledOpRegistration.Id, scheduledOpRegistration);

            var streamFactory = new GetStreamFromRepresentationByNameProtocolFactory(
                new Dictionary<string, Func<IStream>>
                {
                    { eventStream.Name, () => eventStream },
                });

            var dateCounter = -1;
            var dates = new[]
                        {
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 1, 0, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 1, 1, 0, DateTimeKind.Utc),
                            new DateTime(realNow.Year, realNow.Month, realNow.Day, 1, 2, 0, DateTimeKind.Utc),
                        };

            DateTime NowProvider()
            {
                dateCounter = dateCounter + 1;
                return dates[dateCounter];
            }

            var op = new ProcessScheduledOpRegistrationsOp();
            var computeProtocol = new ComputePreviousExecutionFromScheduleProtocol();
            var protocol = new ProcessScheduledOpRegistrationsProtocol(
                registrationStream,
                computeProtocol,
                streamFactory,
                TimeSpan.FromMinutes(2),
                NowProvider);

            // 0100
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(0);

            // 0101
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(1);
            
            // 0102
            protocol.Execute(op);
            eventStream.Execute(new StandardGetInternalRecordIdsOp(new RecordFilter())).MustForTest().HaveCount(1);
        }
    }
}