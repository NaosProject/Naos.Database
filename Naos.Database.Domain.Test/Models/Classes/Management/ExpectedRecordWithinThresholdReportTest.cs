// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpectedRecordWithinThresholdReportTest.cs" company="Naos Project">
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
    using Naos.Diagnostics.Domain;
    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using OBeautifulCode.DateTime.Recipes;
    using OBeautifulCode.Math.Recipes;

    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class ExpectedRecordWithinThresholdReportTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static ExpectedRecordWithinThresholdReportTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                    .AddScenario(() =>
                        new ConstructorArgumentValidationTestScenario<ExpectedRecordWithinThresholdReport>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'expectedRecordWithinThreshold' is null scenario",
                            ConstructionFunc = () =>
                            {
                                var referenceObject = A.Dummy<ExpectedRecordWithinThresholdReport>();

                                var result = new ExpectedRecordWithinThresholdReport(
                                    referenceObject.Status,
                                    null,
                                    referenceObject.LatestMatchingRecordTimestampUtc);

                                return result;
                            },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[] { "expectedRecordWithinThreshold", },
                        })
                   .AddScenario(
                        () =>
                            new ConstructorArgumentValidationTestScenario<ExpectedRecordWithinThresholdReport>
                            {
                                Name = "constructor should throw ArgumentOutOfRangeException when parameter 'status' is 'Invalid' scenario",
                                ConstructionFunc = () =>
                                                   {
                                                       var referenceObject = A.Dummy<ExpectedRecordWithinThresholdReport>();

                                                       var result = new ExpectedRecordWithinThresholdReport(
                                                           CheckStatus.Invalid,
                                                           referenceObject.ExpectedRecordWithinThreshold,
                                                           referenceObject.LatestMatchingRecordTimestampUtc);

                                                       return result;
                                                   },
                                ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                                ExpectedExceptionMessageContains = new[]
                                                                   {
                                                                       "status",
                                                                   },
                            })
                   .AddScenario(
                        () =>
                            new ConstructorArgumentValidationTestScenario<ExpectedRecordWithinThresholdReport>
                            {
                                Name = "constructor should throw ArgumentException when parameter 'latestMatchingRecordTimestampUtc' is not UTC scenario",
                                ConstructionFunc = () =>
                                                   {
                                                       var referenceObject = A.Dummy<ExpectedRecordWithinThresholdReport>();

                                                       var result = new ExpectedRecordWithinThresholdReport(
                                                           referenceObject.Status,
                                                           referenceObject.ExpectedRecordWithinThreshold,
                                                           referenceObject.LatestMatchingRecordTimestampUtc.ToUnspecified());

                                                       return result;
                                                   },
                                ExpectedExceptionType = typeof(ArgumentException),
                                ExpectedExceptionMessageContains = new[]
                                                                   {
                                                                       "latestMatchingRecordTimestampUtc",
                                                                   },
                            });
        }
    }
}