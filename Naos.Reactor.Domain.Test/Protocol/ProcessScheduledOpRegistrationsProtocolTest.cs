// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessScheduledOpRegistrationsProtocolTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain.Test
{
    using System;
    using System.Collections.Generic;
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
                DateTime.UtcNow,
                ScheduledOpAlreadyRunningStrategy.Skip);

            registrationStream.PutWithId(scheduledOpRegistration.Id, scheduledOpRegistration);

            var streamFactory = new GetStreamFromRepresentationByNameProtocolFactory(
                new Dictionary<string, Func<IStream>>
                {
                    { eventStream.Name, () => eventStream },
                });

            var realNow = DateTime.UtcNow;
            var minuteCounter = 0;
            var hourCounter = 1;
            DateTime NowProvider()
            {
                if (minuteCounter == 3)
                {
                    minuteCounter = 1;
                    if (hourCounter == 23)
                    {
                        hourCounter = 0;
                    }
                    else
                    {
                        hourCounter = hourCounter + 1;
                    }
                }
                else
                {
                    minuteCounter = minuteCounter + 1;
                }

                return new DateTime(realNow.Year, realNow.Month, realNow.Day, hourCounter, minuteCounter, 0, DateTimeKind.Utc);
            }

            var op = new ProcessScheduledOpRegistrationsOp();
            var computeProtocol = new ComputePreviousExecutionFromScheduleProtocol();
            var protocol = new ProcessScheduledOpRegistrationsProtocol(
                registrationStream,
                computeProtocol,
                streamFactory,
                TimeSpan.FromMinutes(2),
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
    }
}