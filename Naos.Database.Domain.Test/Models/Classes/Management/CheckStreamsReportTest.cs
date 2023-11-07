// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckStreamsReportTest.cs" company="Naos Project">
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
    public static partial class CheckStreamsReportTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static CheckStreamsReportTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<CheckStreamsReport>
                        {
                            Name = "constructor should throw ArgumentOutOfRangeException when parameter 'status' is 'Invalid' scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<CheckStreamsReport>();

                                                   var result = new CheckStreamsReport(
                                                       CheckStatus.Invalid,
                                                       referenceObject.StreamNameToReportMap,
                                                       referenceObject.SampleTimeUtc);

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
                        new ConstructorArgumentValidationTestScenario<CheckStreamsReport>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'streamNameToReportMap' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<CheckStreamsReport>();

                                                   var result = new CheckStreamsReport(
                                                       referenceObject.Status,
                                                       null,
                                                       referenceObject.SampleTimeUtc);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "streamNameToReportMap",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<CheckStreamsReport>
                        {
                            Name =
                                "constructor should throw ArgumentException when parameter 'streamNameToReportMap' contains a key-value pair with a null value scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<CheckStreamsReport>();

                                                   var dictionaryWithNullValue =
                                                       referenceObject.StreamNameToReportMap.ToDictionary(_ => _.Key, _ => _.Value);

                                                   var randomKey =
                                                       dictionaryWithNullValue.Keys.ElementAt(
                                                           ThreadSafeRandom.Next(0, dictionaryWithNullValue.Count));

                                                   dictionaryWithNullValue[randomKey] = null;

                                                   var result = new CheckStreamsReport(
                                                       referenceObject.Status,
                                                       dictionaryWithNullValue,
                                                       referenceObject.SampleTimeUtc);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "streamNameToReportMap",
                                                                   "contains at least one key-value pair with a null value",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<CheckStreamsReport>
                        {
                            Name = "constructor should throw ArgumentException when parameter 'sampleTimeUtc' is not UTC scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<CheckStreamsReport>();

                                                   var result = new CheckStreamsReport(
                                                       referenceObject.Status,
                                                       referenceObject.StreamNameToReportMap,
                                                       referenceObject.SampleTimeUtc.ToUnspecified());

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "sampleTimeUtc",
                                                                   "not DateTimeKind.Utc",
                                                               },
                        });
        }
    }
}