// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardGetDistinctStringSerializedIdsOpTest.cs" company="Naos Project">
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
    using OBeautifulCode.Type;
    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class StandardGetDistinctStringSerializedIdsOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static StandardGetDistinctStringSerializedIdsOpTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardGetDistinctStringSerializedIdsOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'tagsToMatch' contains a null element",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardGetDistinctStringSerializedIdsOp>();

                            var result = new StandardGetDistinctStringSerializedIdsOp(
                                referenceObject.IdentifierType,
                                referenceObject.ObjectType,
                                referenceObject.VersionMatchStrategy,
                                new[] { A.Dummy<NamedValue<string>>(), null, A.Dummy<NamedValue<string>>() },
                                referenceObject.TagMatchStrategy,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "tagsToMatch", "contains at least one null element" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardGetDistinctStringSerializedIdsOp>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'tagMatchStrategy' is TagMatchStrategy.Unknown scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardGetDistinctStringSerializedIdsOp>();

                            var result = new StandardGetDistinctStringSerializedIdsOp(
                                referenceObject.IdentifierType,
                                referenceObject.ObjectType,
                                referenceObject.VersionMatchStrategy,
                                referenceObject.TagsToMatch,
                                TagMatchStrategy.Unknown,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "tagMatchStrategy", "Unknown" },
                    });
        }
    }
}