// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScheduledOpRegistrationTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using FakeItEasy;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using OBeautifulCode.Math.Recipes;
    using OBeautifulCode.Type;
    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class ScheduledOpRegistrationTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static ScheduledOpRegistrationTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
            .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<ScheduledOpRegistration>
                {
                    Name = "constructor should throw ArgumentNullException when parameter 'operationToExecute' is null scenario",
                    ConstructionFunc = () =>
                    {
                        var referenceObject = A.Dummy<ScheduledOpRegistration>();

                        var result = new ScheduledOpRegistration(
                                             referenceObject.Id,
                                             null,
                                             referenceObject.Schedule,
                                             referenceObject.StreamRepresentation,
                                             referenceObject.TimestampUtc,
                                             referenceObject.ScheduledOpAlreadyRunningStrategy,
                                             referenceObject.ScheduleImmediatelyWhenMissed,
                                             referenceObject.Details,
                                             referenceObject.Tags);

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentNullException),
                    ExpectedExceptionMessageContains = new[] { "operationToExecute", },
                })
            .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<ScheduledOpRegistration>
                {
                    Name = "constructor should throw ArgumentNullException when parameter 'schedule' is null scenario",
                    ConstructionFunc = () =>
                    {
                        var referenceObject = A.Dummy<ScheduledOpRegistration>();

                        var result = new ScheduledOpRegistration(
                                             referenceObject.Id,
                                             referenceObject.OperationToExecute,
                                             null,
                                             referenceObject.StreamRepresentation,
                                             referenceObject.TimestampUtc,
                                             referenceObject.ScheduledOpAlreadyRunningStrategy,
                                             referenceObject.ScheduleImmediatelyWhenMissed,
                                             referenceObject.Details,
                                             referenceObject.Tags);

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentNullException),
                    ExpectedExceptionMessageContains = new[] { "schedule", },
                })
            .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<ScheduledOpRegistration>
                {
                    Name = "constructor should throw ArgumentNullException when parameter 'streamRepresentation' is null scenario",
                    ConstructionFunc = () =>
                    {
                        var referenceObject = A.Dummy<ScheduledOpRegistration>();

                        var result = new ScheduledOpRegistration(
                                             referenceObject.Id,
                                             referenceObject.OperationToExecute,
                                             referenceObject.Schedule,
                                             null,
                                             referenceObject.TimestampUtc,
                                             referenceObject.ScheduledOpAlreadyRunningStrategy,
                                             referenceObject.ScheduleImmediatelyWhenMissed,
                                             referenceObject.Details,
                                             referenceObject.Tags);

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentNullException),
                    ExpectedExceptionMessageContains = new[] { "streamRepresentation", },
                })
            .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<ScheduledOpRegistration>
                {
                    Name = "constructor should throw ArgumentOutOfRangeException when parameter 'scheduledOpAlreadyRunningStrategy' is 'Unknown' scenario",
                    ConstructionFunc = () =>
                    {
                        var referenceObject = A.Dummy<ScheduledOpRegistration>();

                        var result = new ScheduledOpRegistration(
                                             referenceObject.Id,
                                             referenceObject.OperationToExecute,
                                             referenceObject.Schedule,
                                             referenceObject.StreamRepresentation,
                                             referenceObject.TimestampUtc,
                                             ScheduledOpAlreadyRunningStrategy.Unknown,
                                             referenceObject.ScheduleImmediatelyWhenMissed,
                                             referenceObject.Details,
                                             referenceObject.Tags);

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                    ExpectedExceptionMessageContains = new[] { "scheduledOpAlreadyRunningStrategy", },
                })
            .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<ScheduledOpRegistration>
                {
                    Name = "constructor should throw ArgumentException when parameter 'tags' contains a null element scenario",
                    ConstructionFunc = () =>
                    {
                        var referenceObject = A.Dummy<ScheduledOpRegistration>();

                        var result = new ScheduledOpRegistration(
                                             referenceObject.Id,
                                             referenceObject.OperationToExecute,
                                             referenceObject.Schedule,
                                             referenceObject.StreamRepresentation,
                                             referenceObject.TimestampUtc,
                                             referenceObject.ScheduledOpAlreadyRunningStrategy,
                                             referenceObject.ScheduleImmediatelyWhenMissed,
                                             referenceObject.Details,
                                             new NamedValue<string>[0].Concat(referenceObject.Tags).Concat(new NamedValue<string>[] { null }).Concat(referenceObject.Tags).ToList());

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentException),
                    ExpectedExceptionMessageContains = new[] { "tags", "contains at least one null element", },
                });
        }
    }
}