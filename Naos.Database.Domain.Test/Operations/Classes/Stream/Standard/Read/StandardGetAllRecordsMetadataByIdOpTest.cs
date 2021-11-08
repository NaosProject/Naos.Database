// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardGetAllRecordsMetadataByIdOpTest.cs" company="Naos Project">
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
    public static partial class StandardGetAllRecordsMetadataByIdOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static StandardGetAllRecordsMetadataByIdOpTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardGetAllRecordsMetadataByIdOp>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'recordNotFoundStrategy' is RecordNotFoundStrategy.Unknown scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardGetAllRecordsMetadataByIdOp>();

                            var result = new StandardGetAllRecordsMetadataByIdOp(
                                referenceObject.StringSerializedId,
                                referenceObject.IdentifierType,
                                referenceObject.ObjectType,
                                referenceObject.VersionMatchStrategy,
                                RecordNotFoundStrategy.Unknown,
                                referenceObject.OrderRecordsBy,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "recordNotFoundStrategy", "Unknown" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardGetAllRecordsMetadataByIdOp>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'orderRecordsBy' is OrderRecordsBy.Unknown scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardGetAllRecordsMetadataByIdOp>();

                            var result = new StandardGetAllRecordsMetadataByIdOp(
                                referenceObject.StringSerializedId,
                                referenceObject.IdentifierType,
                                referenceObject.ObjectType,
                                referenceObject.VersionMatchStrategy,
                                referenceObject.RecordNotFoundStrategy,
                                OrderRecordsBy.Unknown,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "orderRecordsBy", "Unknown" },
                    });
        }
    }
}