// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckStreamInstructionTest.cs" company="Naos Project">
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
    public static partial class CheckStreamInstructionTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static CheckStreamInstructionTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<CheckStreamInstruction>
                        {
                            Name =
                                "constructor should throw ArgumentNullException when parameter 'expectedRecordsWithinThreshold' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<CheckStreamInstruction>();

                                                   var result = new CheckStreamInstruction(
                                                       null,
                                                       referenceObject.RecordsExpectedToBeHandled);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "expectedRecordsWithinThreshold",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<CheckStreamInstruction>
                        {
                            Name =
                                "constructor should throw ArgumentException when parameter 'expectedRecordsWithinThreshold' contains a null element scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<CheckStreamInstruction>();

                                                   var result = new CheckStreamInstruction(
                                                       new ExpectedRecordWithinThreshold[0]
                                                          .Concat(referenceObject.ExpectedRecordsWithinThreshold)
                                                          .Concat(
                                                               new ExpectedRecordWithinThreshold[]
                                                               {
                                                                   null,
                                                               })
                                                          .Concat(referenceObject.ExpectedRecordsWithinThreshold)
                                                          .ToList(),
                                                       referenceObject.RecordsExpectedToBeHandled);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "expectedRecordsWithinThreshold",
                                                                   "contains at least one null element",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<CheckStreamInstruction>
                        {
                            Name =
                                "constructor should throw ArgumentNullException when parameter 'recordsExpectedToBeHandled' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<CheckStreamInstruction>();

                                                   var result = new CheckStreamInstruction(
                                                       referenceObject.ExpectedRecordsWithinThreshold,
                                                       null);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "recordsExpectedToBeHandled",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<CheckStreamInstruction>
                        {
                            Name =
                                "constructor should throw ArgumentException when parameter 'recordsExpectedToBeHandled' contains a null element scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var referenceObject = A.Dummy<CheckStreamInstruction>();

                                                   var result = new CheckStreamInstruction(
                                                       referenceObject.ExpectedRecordsWithinThreshold,
                                                       new RecordExpectedToBeHandled[0].Concat(referenceObject.RecordsExpectedToBeHandled)
                                                                                       .Concat(
                                                                                            new RecordExpectedToBeHandled[]
                                                                                            {
                                                                                                null,
                                                                                            })
                                                                                       .Concat(referenceObject.RecordsExpectedToBeHandled)
                                                                                       .ToList());

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "recordsExpectedToBeHandled",
                                                                   "contains at least one null element",
                                                               },
                        });
        }
    }
}