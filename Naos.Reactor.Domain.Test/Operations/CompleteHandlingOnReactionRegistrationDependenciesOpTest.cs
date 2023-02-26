// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompleteHandlingOnReactionRegistrationDependenciesOpTest.cs" company="Naos Project">
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

    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class CompleteHandlingOnReactionRegistrationDependenciesOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static CompleteHandlingOnReactionRegistrationDependenciesOpTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<CompleteHandlingOnReactionRegistrationDependenciesOp>
                {
                    Name = "constructor should throw ArgumentNullException when parameter 'reactionRegistration' is null scenario",
                    ConstructionFunc = () =>
                    {
                        var referenceObject = A.Dummy<CompleteHandlingOnReactionRegistrationDependenciesOp>();

                        var result = new CompleteHandlingOnReactionRegistrationDependenciesOp(
                                             null,
                                             referenceObject.Details,
                                             referenceObject.AcceptableHandlingStatuses);

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentNullException),
                    ExpectedExceptionMessageContains = new[] { "reactionRegistration", },
                })
            .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<CompleteHandlingOnReactionRegistrationDependenciesOp>
                {
                    Name = "constructor should throw ArgumentNullException when parameter 'details' is null scenario",
                    ConstructionFunc = () =>
                    {
                        var referenceObject = A.Dummy<CompleteHandlingOnReactionRegistrationDependenciesOp>();

                        var result = new CompleteHandlingOnReactionRegistrationDependenciesOp(
                                             referenceObject.ReactionRegistration,
                                             null,
                                             referenceObject.AcceptableHandlingStatuses);

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentNullException),
                    ExpectedExceptionMessageContains = new[] { "details", },
                })
            .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<CompleteHandlingOnReactionRegistrationDependenciesOp>
                {
                    Name = "constructor should throw ArgumentException when parameter 'details' is white space scenario",
                    ConstructionFunc = () =>
                    {
                        var referenceObject = A.Dummy<CompleteHandlingOnReactionRegistrationDependenciesOp>();

                        var result = new CompleteHandlingOnReactionRegistrationDependenciesOp(
                                             referenceObject.ReactionRegistration,
                                             Invariant($"  {Environment.NewLine}  "),
                                             referenceObject.AcceptableHandlingStatuses);

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentException),
                    ExpectedExceptionMessageContains = new[] { "details", "white space", },
                });
        }
    }
}