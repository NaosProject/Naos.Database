// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardGetLatestStringSerializedObjectOpTest.cs" company="Naos Project">
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
    public static partial class StandardGetLatestStringSerializedObjectOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static StandardGetLatestStringSerializedObjectOpTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardGetLatestStringSerializedObjectOp>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'recordFilter' is null",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardGetLatestStringSerializedObjectOp>();

                            var result = new StandardGetLatestStringSerializedObjectOp(
                                null,
                                referenceObject.RecordNotFoundStrategy,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "recordFilter" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardGetLatestStringSerializedObjectOp>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'recordNotFoundStrategy' is RecordNotFoundStrategy.Unknown scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardGetLatestStringSerializedObjectOp>();

                            var result = new StandardGetLatestStringSerializedObjectOp(
                                referenceObject.RecordFilter,
                                RecordNotFoundStrategy.Unknown,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "recordNotFoundStrategy", "Unknown" },
                    });
        }
    }
}