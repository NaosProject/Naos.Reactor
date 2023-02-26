// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReactionEventTest.cs" company="Naos Project">
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
    using Naos.Database.Domain;
    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using OBeautifulCode.Math.Recipes;
    using OBeautifulCode.Type;
    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class ReactionEventTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static ReactionEventTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<ReactionEvent>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'id' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<ReactionEvent>();

                                                   var result = new ReactionEvent(
                                                       null,
                                                       referenceObject.ReactionRegistrationId,
                                                       referenceObject.ReactionContext,
                                                       referenceObject.StreamRepresentationToInternalRecordIdsMap,
                                                       referenceObject.TimestampUtc,
                                                       referenceObject.Tags);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "id",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<ReactionEvent>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'id' is white space scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<ReactionEvent>();

                                                   var result = new ReactionEvent(
                                                       Invariant($"  {Environment.NewLine}  "),
                                                       referenceObject.ReactionRegistrationId,
                                                       referenceObject.ReactionContext,
                                                       referenceObject.StreamRepresentationToInternalRecordIdsMap,
                                                       referenceObject.TimestampUtc,
                                                       referenceObject.Tags);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "id",
                                                                   "white space",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<ReactionEvent>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'reactionRegistrationId' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<ReactionEvent>();

                                                   var result = new ReactionEvent(
                                                       referenceObject.Id,
                                                       null,
                                                       referenceObject.ReactionContext,
                                                       referenceObject.StreamRepresentationToInternalRecordIdsMap,
                                                       referenceObject.TimestampUtc,
                                                       referenceObject.Tags);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "reactionRegistrationId",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<ReactionEvent>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'reactionRegistrationId' is white space scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<ReactionEvent>();

                                                   var result = new ReactionEvent(
                                                       referenceObject.Id,
                                                       Invariant($"  {Environment.NewLine}  "),
                                                       referenceObject.ReactionContext,
                                                       referenceObject.StreamRepresentationToInternalRecordIdsMap,
                                                       referenceObject.TimestampUtc,
                                                       referenceObject.Tags);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "reactionRegistrationId",
                                                                   "white space",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<ReactionEvent>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'reactionContext' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<ReactionEvent>();

                                                   var result = new ReactionEvent(
                                                       referenceObject.Id,
                                                       referenceObject.ReactionRegistrationId,
                                                       null,
                                                       referenceObject.StreamRepresentationToInternalRecordIdsMap,
                                                       referenceObject.TimestampUtc,
                                                       referenceObject.Tags);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "reactionContext",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<ReactionEvent>
                        {
                            Name =
                                "constructor should throw ArgumentNullException when parameter 'streamRepresentationToInternalRecordIdsMap' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<ReactionEvent>();

                                                   var result = new ReactionEvent(
                                                       referenceObject.Id,
                                                       referenceObject.ReactionRegistrationId,
                                                       referenceObject.ReactionContext,
                                                       null,
                                                       referenceObject.TimestampUtc,
                                                       referenceObject.Tags);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "streamRepresentationToInternalRecordIdsMap",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<ReactionEvent>
                        {
                            Name =
                                "constructor should throw ArgumentException when parameter 'streamRepresentationToInternalRecordIdsMap' contains a key-value pair with a null value scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<ReactionEvent>();

                                                   var dictionaryWithNullValue =
                                                       referenceObject.StreamRepresentationToInternalRecordIdsMap.ToDictionary(
                                                           _ => _.Key,
                                                           _ => _.Value);

                                                   var randomKey =
                                                       dictionaryWithNullValue.Keys.ElementAt(
                                                           ThreadSafeRandom.Next(0, dictionaryWithNullValue.Count));

                                                   dictionaryWithNullValue[randomKey] = null;

                                                   var result = new ReactionEvent(
                                                       referenceObject.Id,
                                                       referenceObject.ReactionRegistrationId,
                                                       referenceObject.ReactionContext,
                                                       dictionaryWithNullValue,
                                                       referenceObject.TimestampUtc,
                                                       referenceObject.Tags);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "streamRepresentationToInternalRecordIdsMap",
                                                                   "contains at least one key-value pair with a null value",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<ReactionEvent>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'tags' contains a null element scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<ReactionEvent>();

                                                   var result = new ReactionEvent(
                                                       referenceObject.Id,
                                                       referenceObject.ReactionRegistrationId,
                                                       referenceObject.ReactionContext,
                                                       referenceObject.StreamRepresentationToInternalRecordIdsMap,
                                                       referenceObject.TimestampUtc,
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