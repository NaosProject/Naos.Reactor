// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvaluateScheduleProtocolTest.cs" company="Naos Project">
//   Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Protocol.Test
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using CLAP;
    using Naos.Cron;
    using Naos.Reactor.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using Xunit;

    public static class EvaluateScheduleProtocolTest
    {
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "aftertime", Justification = "Preferred.")]
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

            DateTime? expected = new DateTime(
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

            var priorExecutionTimestampUtc = protocol.Execute(op);

            priorExecutionTimestampUtc.MustForTest().BeEqualTo(expected);
        }

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "beforetime", Justification = "Preferred.")]
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

            DateTime? expected = new DateTime(
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

            var priorExecutionTimestampUtc = protocol.Execute(op);

            priorExecutionTimestampUtc.MustForTest().BeEqualTo(expected);
        }

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "aftertime", Justification = "Preferred.")]
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

            DateTime? expected = new DateTime(
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

            var priorExecutionTimestampUtc = protocol.Execute(op);

            priorExecutionTimestampUtc.MustForTest().BeEqualTo(expected);
        }

        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "beforetime", Justification = "Preferred.")]
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

            DateTime? expected = new DateTime(
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

            var priorExecutionTimestampUtc = protocol.Execute(op);

            priorExecutionTimestampUtc.MustForTest().BeEqualTo(expected);
        }
    }
}