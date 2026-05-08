// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumberOfAttemptsWaitOneRetryStrategyTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test
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
    public static partial class NumberOfAttemptsWaitOneRetryStrategyTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static NumberOfAttemptsWaitOneRetryStrategyTest()
        {
            ConstructorArgumentValidationTestScenarios
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<NumberOfAttemptsWaitOneRetryStrategy>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'attempts' is negative",
                        ConstructionFunc = () =>
                        {
                            var result = new NumberOfAttemptsWaitOneRetryStrategy(
                                A.Dummy<NegativeInteger>());

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "attempts", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<NumberOfAttemptsWaitOneRetryStrategy>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'attempts' is 0",
                        ConstructionFunc = () =>
                        {
                            var result = new NumberOfAttemptsWaitOneRetryStrategy(
                                0);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "attempts", },
                    });
        }
    }
}