// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckSingleStreamReportTest.cs" company="Naos Project">
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
    using OBeautifulCode.Math.Recipes;

    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class CheckSingleStreamReportTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static CheckSingleStreamReportTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<CheckSingleStreamReport>
                        {
                            Name =
                                "constructor should throw ArgumentOutOfRangeException when parameter 'status' is 'Invalid' scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<CheckSingleStreamReport>();

                                                   var result = new CheckSingleStreamReport(
                                                       CheckStatus.Invalid,
                                                       referenceObject.ExpectedRecordWithinThresholdIdToReportMap,
                                                       referenceObject.RecordExpectedToBeHandledIdToReportMap);

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
                        new ConstructorArgumentValidationTestScenario<CheckSingleStreamReport>
                        {
                            Name =
                                "constructor should throw ArgumentNullException when parameter 'expectedRecordWithinThresholdIdToReportMap' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<CheckSingleStreamReport>();

                                                   var result = new CheckSingleStreamReport(
                                                       referenceObject.Status,
                                                       null,
                                                       referenceObject.RecordExpectedToBeHandledIdToReportMap);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "expectedRecordWithinThresholdIdToReportMap",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<CheckSingleStreamReport>
                        {
                            Name =
                                "constructor should throw ArgumentNullException when parameter 'recordExpectedToBeHandledIdToReportMap' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<CheckSingleStreamReport>();

                                                   var result = new CheckSingleStreamReport(
                                                       referenceObject.Status,
                                                       referenceObject.ExpectedRecordWithinThresholdIdToReportMap,
                                                       null);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "recordExpectedToBeHandledIdToReportMap",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<CheckSingleStreamReport>
                        {
                            Name =
                                "constructor should throw ArgumentException when parameter 'recordExpectedToBeHandledIdToReportMap' contains a key-value pair with a null value scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<CheckSingleStreamReport>();

                                                   var dictionaryWithNullValue =
                                                       referenceObject.RecordExpectedToBeHandledIdToReportMap.ToDictionary(
                                                           _ => _.Key,
                                                           _ => _.Value);

                                                   var randomKey =
                                                       dictionaryWithNullValue.Keys.ElementAt(
                                                           ThreadSafeRandom.Next(0, dictionaryWithNullValue.Count));

                                                   dictionaryWithNullValue[randomKey] = null;

                                                   var result = new CheckSingleStreamReport(
                                                       referenceObject.Status,
                                                       referenceObject.ExpectedRecordWithinThresholdIdToReportMap,
                                                       dictionaryWithNullValue);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "recordExpectedToBeHandledIdToReportMap",
                                                                   "contains at least one key-value pair with a null value",
                                                               },
                        });
        }
    }
}