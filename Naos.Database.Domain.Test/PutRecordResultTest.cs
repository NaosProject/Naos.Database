// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutRecordResultTest.cs" company="Naos Project">
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
    public static partial class PutRecordResultTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static PutRecordResultTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<PutRecordResult>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'existingRecordIds' is null AND the 'internalRecordId' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var result = new PutRecordResult(
                                                       null,
                                                       null);

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "the expectation is that the record was written OR there was an existing record",
                                                               },
                        })
               .AddScenario(
                    () =>
                        new ConstructorArgumentValidationTestScenario<PutRecordResult>
                        {
                            Name = "constructor should throw ArgumentNullException when parameter 'existingRecordIds' is empty collection AND the 'internalRecordId' is null scenario",
                            ConstructionFunc = () =>
                                               {
                                                   var result = new PutRecordResult(
                                                       null,
                                                       new List<long>());

                                                   return result;
                                               },
                            ExpectedExceptionType = typeof(ArgumentNullException),
                            ExpectedExceptionMessageContains = new[]
                                                               {
                                                                   "the expectation is that the record was written OR there was an existing record",
                                                               },
                        });
        }
    }
}