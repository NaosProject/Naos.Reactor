// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventToPutWithId{TId}Test.cs" company="Naos Project">
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
    public static partial class EventToPutWithIdTIdTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static EventToPutWithIdTIdTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<EventToPutWithId<Version>>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'streamRepresentation' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<EventToPutWithId<Version>>();

                                                   var result = new EventToPutWithId<Version>(
                                                       referenceObject.Id,
                                                       referenceObject.EventToPut,
                                                       null,
                                                       referenceObject.UpdateTimestampOnPut,
                                                       referenceObject.Tags);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "streamRepresentation",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<EventToPutWithId<Version>>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'tags' contains a null element scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<EventToPutWithId<Version>>();

                                                   var result = new EventToPutWithId<Version>(
                                                       referenceObject.Id,
                                                       referenceObject.EventToPut,
                                                       referenceObject.StreamRepresentation,
                                                       referenceObject.UpdateTimestampOnPut,
                                                       new NamedValue<string>[0].Concat(referenceObject.Tags)
                                                                                .Concat(
                                                                                     new NamedValue<string>[]
                                                                                     {
                                                                                         null,
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