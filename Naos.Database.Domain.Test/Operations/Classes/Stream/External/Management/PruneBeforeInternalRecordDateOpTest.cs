// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PruneBeforeInternalRecordDateOpTest.cs" company="Naos Project">
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
    public static partial class PruneBeforeInternalRecordDateOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static PruneBeforeInternalRecordDateOpTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(() =>
                   new ConstructorArgumentValidationTestScenario<PruneBeforeInternalRecordDateOp>
                   {
                       Name = "constructor should throw ArgumentOutOfRangeException when parameter 'internalRecordDate' is not in UTC",
                       ConstructionFunc = () =>
                       {
                           var referenceObject = A.Dummy<PruneBeforeInternalRecordDateOp>();

                           var result = new PruneBeforeInternalRecordDateOp(
                               A.Dummy<DateTime>().Whose(_ => _.Kind != DateTimeKind.Utc),
                               referenceObject.Details);

                           return result;
                       },
                       ExpectedExceptionType = typeof(ArgumentException),
                       ExpectedExceptionMessageContains = new[] { "Timestamp must be UTC", },
                   })
               .AddScenario(() =>
                   new ConstructorArgumentValidationTestScenario<PruneBeforeInternalRecordDateOp>
                   {
                       Name = "constructor should throw ArgumentNullException when parameter 'details' is null scenario",
                       ConstructionFunc = () =>
                       {
                           var referenceObject = A.Dummy<PruneBeforeInternalRecordDateOp>();

                           var result = new PruneBeforeInternalRecordDateOp(
                               referenceObject.InternalRecordDate,
                               null);

                           return result;
                       },
                       ExpectedExceptionType = typeof(ArgumentNullException),
                       ExpectedExceptionMessageContains = new[] { "details", },
                   })
               .AddScenario(() =>
                   new ConstructorArgumentValidationTestScenario<PruneBeforeInternalRecordDateOp>
                   {
                       Name = "constructor should throw ArgumentException when parameter 'details' is white space scenario",
                       ConstructionFunc = () =>
                       {
                           var referenceObject = A.Dummy<PruneBeforeInternalRecordDateOp>();

                           var result = new PruneBeforeInternalRecordDateOp(
                               referenceObject.InternalRecordDate,
                               Invariant($"  {Environment.NewLine}  "));

                           return result;
                       },
                       ExpectedExceptionType = typeof(ArgumentException),
                       ExpectedExceptionMessageContains = new[] { "details", "white space", },
                   });
        }
    }
}