// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventToPutWithIdOnHandlingStatusMatch{TId}Test.cs" company="Naos Project">
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

    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class EventToPutWithIdOnHandlingStatusMatchTIdTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static EventToPutWithIdOnHandlingStatusMatchTIdTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<EventToPutWithIdOnHandlingStatusMatch<Version>>
                {
                    Name = "constructor should throw ArgumentNullException when parameter 'eventToPut' is null scenario",
                    ConstructionFunc = () =>
                    {
                        var referenceObject = A.Dummy<EventToPutWithIdOnHandlingStatusMatch<Version>>();

                        var result = new EventToPutWithIdOnHandlingStatusMatch<Version>(
                                             referenceObject.StatusToMatch,
                                             referenceObject.CompositeHandlingStatusMatchStrategy,
                                             null,
                                             referenceObject.ChainOfResponsibilityLinkMatchStrategy,
                                             referenceObject.Details);

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentNullException),
                    ExpectedExceptionMessageContains = new[] { "eventToPut", },
                })
            .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<EventToPutWithIdOnHandlingStatusMatch<Version>>
                {
                    Name = "constructor should throw ArgumentOutOfRangeException when parameter 'statusToMatch' is 'Unknown' scenario",
                    ConstructionFunc = () =>
                    {
                        var referenceObject = A.Dummy<EventToPutWithIdOnHandlingStatusMatch<Version>>();

                        var result = new EventToPutWithIdOnHandlingStatusMatch<Version>(
                                             CompositeHandlingStatus.Unknown,
                                             referenceObject.CompositeHandlingStatusMatchStrategy,
                                             referenceObject.EventToPut,
                                             referenceObject.ChainOfResponsibilityLinkMatchStrategy,
                                             referenceObject.Details);

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                    ExpectedExceptionMessageContains = new[] { "statusToMatch", },
                })
            .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<EventToPutWithIdOnHandlingStatusMatch<Version>>
                {
                    Name = "constructor should throw ArgumentOutOfRangeException when parameter 'compositeHandlingStatusMatchStrategy' is 'Unknown' scenario",
                    ConstructionFunc = () =>
                    {
                        var referenceObject = A.Dummy<EventToPutWithIdOnHandlingStatusMatch<Version>>();

                        var result = new EventToPutWithIdOnHandlingStatusMatch<Version>(
                                             referenceObject.StatusToMatch,
                                             CompositeHandlingStatusMatchStrategy.Unknown,
                                             referenceObject.EventToPut,
                                             referenceObject.ChainOfResponsibilityLinkMatchStrategy,
                                             referenceObject.Details);

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                    ExpectedExceptionMessageContains = new[] { "compositeHandlingStatusMatchStrategy", },
                })
            .AddScenario(() =>
                new ConstructorArgumentValidationTestScenario<EventToPutWithIdOnHandlingStatusMatch<Version>>
                {
                    Name = "constructor should throw ArgumentOutOfRangeException when parameter 'chainOfResponsibilityLinkMatchStrategy' is 'Unknown' scenario",
                    ConstructionFunc = () =>
                    {
                        var referenceObject = A.Dummy<EventToPutWithIdOnHandlingStatusMatch<Version>>();

                        var result = new EventToPutWithIdOnHandlingStatusMatch<Version>(
                                             referenceObject.StatusToMatch,
                                             referenceObject.CompositeHandlingStatusMatchStrategy,
                                             referenceObject.EventToPut,
                                             ChainOfResponsibilityLinkMatchStrategy.Unknown,
                                             referenceObject.Details);

                        return result;
                    },
                    ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                    ExpectedExceptionMessageContains = new[] { "chainOfResponsibilityLinkMatchStrategy", },
                });
        }
    }
}