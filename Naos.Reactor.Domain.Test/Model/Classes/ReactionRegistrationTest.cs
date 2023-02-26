// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReactionRegistrationTest.cs" company="Naos Project">
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
    public static partial class ReactionRegistrationTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static ReactionRegistrationTest()
        {
            ConstructorArgumentValidationTestScenarios.RemoveAllScenarios()
            .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<ReactionRegistration>
                {
                    Name = "constructor should throw ArgumentNullException when parameter 'id' is null scenario",
                    ConstructionFunc = () =>
                    {
                        var referenceObject = A.Dummy<ReactionRegistration>();

                        var result = new ReactionRegistration(
                                             null,
                                             referenceObject.ReactionContext,
                                             referenceObject.Dependencies,
                                             referenceObject.IdealWaitTimeBetweenEvaluations,
                                             referenceObject.Tags);

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentNullException),
                    ExpectedExceptionMessageContains = new[] { "id", },
                })
            .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<ReactionRegistration>
                {
                    Name = "constructor should throw ArgumentException when parameter 'id' is white space scenario",
                    ConstructionFunc = () =>
                    {
                        var referenceObject = A.Dummy<ReactionRegistration>();

                        var result = new ReactionRegistration(
                                             Invariant($"  {Environment.NewLine}  "),
                                             referenceObject.ReactionContext,
                                             referenceObject.Dependencies,
                                             referenceObject.IdealWaitTimeBetweenEvaluations,
                                             referenceObject.Tags);

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentException),
                    ExpectedExceptionMessageContains = new[] { "id", "white space", },
                })
            .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<ReactionRegistration>
                {
                    Name = "constructor should throw ArgumentNullException when parameter 'reactionContext' is null scenario",
                    ConstructionFunc = () =>
                    {
                        var referenceObject = A.Dummy<ReactionRegistration>();

                        var result = new ReactionRegistration(
                                             referenceObject.Id,
                                             null,
                                             referenceObject.Dependencies,
                                             referenceObject.IdealWaitTimeBetweenEvaluations,
                                             referenceObject.Tags);

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentNullException),
                    ExpectedExceptionMessageContains = new[] { "reactionContext", },
                })
            .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<ReactionRegistration>
                {
                    Name = "constructor should throw ArgumentNullException when parameter 'dependencies' is null scenario",
                    ConstructionFunc = () =>
                    {
                        var referenceObject = A.Dummy<ReactionRegistration>();

                        var result = new ReactionRegistration(
                                             referenceObject.Id,
                                             referenceObject.ReactionContext,
                                             null,
                                             referenceObject.IdealWaitTimeBetweenEvaluations,
                                             referenceObject.Tags);

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentNullException),
                    ExpectedExceptionMessageContains = new[] { "dependencies", },
                })
            .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<ReactionRegistration>
                {
                    Name = "constructor should throw ArgumentException when parameter 'dependencies' is an empty enumerable scenario",
                    ConstructionFunc = () =>
                    {
                        var referenceObject = A.Dummy<ReactionRegistration>();

                        var result = new ReactionRegistration(
                                             referenceObject.Id,
                                             referenceObject.ReactionContext,
                                             new List<IReactorDependency>(),
                                             referenceObject.IdealWaitTimeBetweenEvaluations,
                                             referenceObject.Tags);

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentException),
                    ExpectedExceptionMessageContains = new[] { "dependencies", "is an empty enumerable", },
                })
            .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<ReactionRegistration>
                {
                    Name = "constructor should throw ArgumentException when parameter 'dependencies' contains a null element scenario",
                    ConstructionFunc = () =>
                    {
                        var referenceObject = A.Dummy<ReactionRegistration>();

                        var result = new ReactionRegistration(
                                             referenceObject.Id,
                                             referenceObject.ReactionContext,
                                             new IReactorDependency[0].Concat(referenceObject.Dependencies).Concat(new IReactorDependency[] { null }).Concat(referenceObject.Dependencies).ToList(),
                                             referenceObject.IdealWaitTimeBetweenEvaluations,
                                             referenceObject.Tags);

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentException),
                    ExpectedExceptionMessageContains = new[] { "dependencies", "contains at least one null element", },
                });
        }
    }
}