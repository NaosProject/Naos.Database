// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WaitOneOpTest.cs" company="Naos Project">
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
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using OBeautifulCode.Math.Recipes;

    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class WaitOneOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static WaitOneOpTest()
        {
            ConstructorArgumentValidationTestScenarios
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<WaitOneOp>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'pollingWaitTime' is 0",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<WaitOneOp>();

                            var result = new WaitOneOp(
                                referenceObject.Id,
                                referenceObject.Details,
                                referenceObject.Concern,
                                TimeSpan.FromMilliseconds(A.Dummy<NegativeInteger>()));

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "pollingWaitTime", "is not greater than or equal to", "00:00:00" },
                    });
        }

        [Fact]
        public static void Constructor___Should_set_PollingWaitTime_to_200_milliseconds___When_parameter_pollingWaitTime_is_the_default_TimeSpan()
        {
            // Arrange
            var systemUnderTest = A.Dummy<WaitOneOp>().DeepCloneWithPollingWaitTime(default);

            // Act
            var actual = systemUnderTest.PollingWaitTime;

            // Assert
            actual.AsTest().Must().BeEqualTo(TimeSpan.FromMilliseconds(200));
        }
    }
}