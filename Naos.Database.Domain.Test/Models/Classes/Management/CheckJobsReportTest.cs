// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckJobsReportTest.cs" company="Naos Project">
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
    using OBeautifulCode.DateTime.Recipes;
    using OBeautifulCode.Math.Recipes;

    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class CheckJobsReportTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static CheckJobsReportTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<CheckJobsReport>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'sampleTimeUtc' is not UTC scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<CheckJobsReport>();

                                                   var result = new CheckJobsReport(
                                                       referenceObject.ShouldAlert,
                                                       referenceObject.JobNameToInformationMap,
                                                       referenceObject.SampleTimeUtc.ToUnspecified());

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "sampleTimeUtc",
                                                                   "not DateTimeKind.Utc",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<CheckJobsReport>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'jobNameToInformationMap' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<CheckJobsReport>();

                                                   var result = new CheckJobsReport(
                                                       referenceObject.ShouldAlert,
                                                       null,
                                                       referenceObject.SampleTimeUtc);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "jobNameToInformationMap",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<CheckJobsReport>
                        {
                            Name =
                                "constructor should throw ArgumentException when parameter 'jobNameToInformationMap' contains a key-value pair with a null value scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<CheckJobsReport>();

                                                   var dictionaryWithNullValue =
                                                       referenceObject.JobNameToInformationMap.ToDictionary(_ => _.Key, _ => _.Value);

                                                   var randomKey =
                                                       dictionaryWithNullValue.Keys.ElementAt(
                                                           ThreadSafeRandom.Next(0, dictionaryWithNullValue.Count));

                                                   dictionaryWithNullValue[randomKey] = null;

                                                   var result = new CheckJobsReport(
                                                       referenceObject.ShouldAlert,
                                                       dictionaryWithNullValue,
                                                       referenceObject.SampleTimeUtc);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "jobNameToInformationMap",
                                                                   "contains at least one key-value pair with a null value",
                                                               },
                        });
        }
    }
}