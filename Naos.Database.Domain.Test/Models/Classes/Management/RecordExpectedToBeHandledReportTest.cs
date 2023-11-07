// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordExpectedToBeHandledReportTest.cs" company="Naos Project">
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
    public static partial class RecordExpectedToBeHandledReportTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static RecordExpectedToBeHandledReportTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<RecordExpectedToBeHandledReport>
                        {
                            Name = "constructor should throw ArgumentOutOfRangeException when parameter 'status' is 'Invalid' scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<RecordExpectedToBeHandledReport>();

                                                   var result = new RecordExpectedToBeHandledReport(
                                                       CheckStatus.Invalid,
                                                       referenceObject.RecordExpectedToBeHandled,
                                                       referenceObject.InternalRecordIdToHandlingStatusMap);

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
                        new ConstructorArgumentValidationTestScenario<RecordExpectedToBeHandledReport>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'recordExpectedToBeHandled' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<RecordExpectedToBeHandledReport>();

                                                   var result = new RecordExpectedToBeHandledReport(
                                                       referenceObject.Status,
                                                       null,
                                                       referenceObject.InternalRecordIdToHandlingStatusMap);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "recordExpectedToBeHandled",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<RecordExpectedToBeHandledReport>
                        {
                            Name =
                                "constructor should throw ArgumentNullException when parameter 'internalRecordIdToHandlingStatusMap' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<RecordExpectedToBeHandledReport>();

                                                   var result = new RecordExpectedToBeHandledReport(
                                                       referenceObject.Status,
                                                       referenceObject.RecordExpectedToBeHandled,
                                                       null);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "internalRecordIdToHandlingStatusMap",
                                                               },
                        });
        }
    }
}