// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAllRecordsByIdOp{TId}Test.cs" company="Naos Project">
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
    using OBeautifulCode.Representation.System;
    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class GetAllRecordsByIdOpTIdTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static GetAllRecordsByIdOpTIdTest()
        {
            ConstructorArgumentValidationTestScenarios
               .RemoveAllScenarios()
               .AddScenario(() =>
                   new ConstructorArgumentValidationTestScenario<GetAllRecordsByIdOp<Version>>
                   {
                       Name = "constructor should throw ArgumentOutOfRangeException when parameter 'recordNotFoundStrategy' is RecordNotFoundStrategy.Unknown scenario",
                       ConstructionFunc = () =>
                       {
                           var referenceObject = A.Dummy<GetAllRecordsByIdOp<Version>>();

                           var result = new GetAllRecordsByIdOp<Version>(
                               referenceObject.Id,
                               referenceObject.ObjectType,
                               referenceObject.VersionMatchStrategy,
                               RecordNotFoundStrategy.Unknown,
                               referenceObject.OrderRecordsBy,
                               referenceObject.DeprecatedIdTypes);

                           return result;
                       },
                       ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                       ExpectedExceptionMessageContains = new[] { "recordNotFoundStrategy", "Unknown" },
                   })
               .AddScenario(() =>
                   new ConstructorArgumentValidationTestScenario<GetAllRecordsByIdOp<Version>>
                   {
                       Name = "constructor should throw ArgumentOutOfRangeException when parameter 'orderRecordsBy' is OrderRecordsBy.Unknown scenario",
                       ConstructionFunc = () =>
                       {
                           var referenceObject = A.Dummy<GetAllRecordsByIdOp<Version>>();

                           var result = new GetAllRecordsByIdOp<Version>(
                               referenceObject.Id,
                               referenceObject.ObjectType,
                               referenceObject.VersionMatchStrategy,
                               referenceObject.RecordNotFoundStrategy,
                               OrderRecordsBy.Unknown,
                               referenceObject.DeprecatedIdTypes);

                           return result;
                       },
                       ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                       ExpectedExceptionMessageContains = new[] { "orderRecordsBy", "Unknown" },
                   })
               .AddScenario(() =>
                   new ConstructorArgumentValidationTestScenario<GetAllRecordsByIdOp<Version>>
                   {
                       Name = "constructor should throw ArgumentException when parameter 'deprecatedIdTypes' contains a null element.",
                       ConstructionFunc = () =>
                       {
                           var referenceObject = A.Dummy<GetAllRecordsByIdOp<Version>>();

                           var result = new GetAllRecordsByIdOp<Version>(
                               referenceObject.Id,
                               referenceObject.ObjectType,
                               referenceObject.VersionMatchStrategy,
                               referenceObject.RecordNotFoundStrategy,
                               referenceObject.OrderRecordsBy,
                               new[] { A.Dummy<TypeRepresentation>(), null });

                           return result;
                       },
                       ExpectedExceptionType = typeof(ArgumentException),
                       ExpectedExceptionMessageContains = new[] { "deprecatedIdTypes", "contains at least one null element" },
                   });
        }
    }
}