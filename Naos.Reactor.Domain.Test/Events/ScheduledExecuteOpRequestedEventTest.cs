// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScheduledExecuteOpRequestedEventTest.cs" company="Naos Project">
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
    using OBeautifulCode.DateTime.Recipes;
    using OBeautifulCode.Math.Recipes;
    using OBeautifulCode.Type;
    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class ScheduledExecuteOpRequestedEventTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static ScheduledExecuteOpRequestedEventTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<ScheduledExecuteOpRequestedEvent>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'operationToExecute' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<ScheduledExecuteOpRequestedEvent>();

                                                   var result = new ScheduledExecuteOpRequestedEvent(
                                                       referenceObject.Id,
                                                       null,
                                                       referenceObject.TargetExecutionUtc,
                                                       referenceObject.TimestampUtc,
                                                       referenceObject.Details,
                                                       referenceObject.Tags);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "operationToExecute",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<ScheduledExecuteOpRequestedEvent>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'targetExecutionUtc' is not UTC scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<ScheduledExecuteOpRequestedEvent>();

                                                   var result = new ScheduledExecuteOpRequestedEvent(
                                                       referenceObject.Id,
                                                       referenceObject.OperationToExecute,
                                                       referenceObject.TargetExecutionUtc.ToUnspecified(),
                                                       referenceObject.TimestampUtc,
                                                       referenceObject.Details,
                                                       referenceObject.Tags);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "targetExecutionUtc",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<ScheduledExecuteOpRequestedEvent>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'tags' contains a null element scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<ScheduledExecuteOpRequestedEvent>();

                                                   var result = new ScheduledExecuteOpRequestedEvent(
                                                       referenceObject.Id,
                                                       referenceObject.OperationToExecute,
                                                       referenceObject.TargetExecutionUtc,
                                                       referenceObject.TimestampUtc,
                                                       referenceObject.Details,
                                                       new NamedValue<string>[0].Concat(referenceObject.Tags)
                                                                                .Concat(
                                                                                     new NamedValue<string>[]
                                                                                     {
                                                                                         null
                                                                                     })
                                                                                .Concat(referenceObject.Tags)
                                                                                .ToList());

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "tags",
                                                                   "contains at least one null element",
                                                               },
                        });
        }
    }
}