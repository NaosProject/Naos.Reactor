// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvaluateScheduleProtocolTest.cs" company="Naos Project">
//   Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain.Test
{
    using System;
    using CLAP;
    using Naos.Cron;
    using OBeautifulCode.Assertion.Recipes;
    using Xunit;

    public static class EvaluateScheduleProtocolTest
    {
        [Fact]
        public static void EvaluateScheduleProtocol___Execute_Daily_just_after_time___Returns_prior_time()
        {
            var referenceTimestampUtc = new DateTime(
                2020,
                10,
                10,
                16,
                20,
                33,
                DateTimeKind.Utc);

            var expected = new DateTime(
                2020,
                10,
                10,
                16,
                15,
                0,
                DateTimeKind.Utc);

            var op = new ComputePreviousExecutionFromScheduleOp(
                new DailyScheduleInUtc
                {
                    Hour = 16,
                    Minute = 15,
                },
                referenceTimestampUtc);

            var protocol = new ComputePreviousExecutionFromScheduleProtocol();

            var nextExecutionTimestampUtc = protocol.Execute(op);

            nextExecutionTimestampUtc.MustForTest().BeEqualTo(expected);
        }

        [Fact]
        public static void EvaluateScheduleProtocol___Execute_Daily_just_before_time___Returns_prior_time()
        {
            var referenceTimestampUtc = new DateTime(
                2020,
                10,
                10,
                16,
                15,
                33,
                DateTimeKind.Utc);

            var expected = new DateTime(
                2020,
                10,
                09,
                16,
                20,
                0,
                DateTimeKind.Utc);

            var op = new ComputePreviousExecutionFromScheduleOp(
                new DailyScheduleInUtc
                {
                    Hour = 16,
                    Minute = 20,
                },
                referenceTimestampUtc);

            var protocol = new ComputePreviousExecutionFromScheduleProtocol();

            var nextExecutionTimestampUtc = protocol.Execute(op);

            nextExecutionTimestampUtc.MustForTest().BeEqualTo(expected);
        }
 
        [Fact]
        public static void EvaluateScheduleProtocol___Execute_Hourly_just_after_time___Returns_prior_time()
        {
            var referenceTimestampUtc = new DateTime(
                2020,
                10,
                10,
                16,
                20,
                33,
                DateTimeKind.Utc);

            var expected = new DateTime(
                2020,
                10,
                10,
                16,
                15,
                0,
                DateTimeKind.Utc);

            var op = new ComputePreviousExecutionFromScheduleOp(
                new HourlySchedule
                {
                    Minute = 15,
                },
                referenceTimestampUtc);

            var protocol = new ComputePreviousExecutionFromScheduleProtocol();

            var nextExecutionTimestampUtc = protocol.Execute(op);

            nextExecutionTimestampUtc.MustForTest().BeEqualTo(expected);
        }

        [Fact]
        public static void EvaluateScheduleProtocol___Execute_Hourly_just_before_time___Returns_prior_time()
        {
            var referenceTimestampUtc = new DateTime(
                2020,
                10,
                10,
                16,
                15,
                33,
                DateTimeKind.Utc);

            var expected = new DateTime(
                2020,
                10,
                10,
                15,
                20,
                0,
                DateTimeKind.Utc);

            var op = new ComputePreviousExecutionFromScheduleOp(
                new HourlySchedule
                {
                    Minute = 20,
                },
                referenceTimestampUtc);

            var protocol = new ComputePreviousExecutionFromScheduleProtocol();

            var nextExecutionTimestampUtc = protocol.Execute(op);

            nextExecutionTimestampUtc.MustForTest().BeEqualTo(expected);
        }
    }
}