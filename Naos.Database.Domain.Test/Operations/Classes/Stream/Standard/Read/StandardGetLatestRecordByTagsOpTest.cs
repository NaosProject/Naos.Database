// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardGetLatestRecordByTagsOpTest.cs" company="Naos Project">
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
    public static partial class StandardGetLatestRecordByTagsOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static StandardGetLatestRecordByTagsOpTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardGetLatestRecordByTagsOp>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'tagsToMatch' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardGetLatestRecordByTagsOp>();

                            var result = new StandardGetLatestRecordByTagsOp(
                                                 null,
                                                 referenceObject.TagMatchStrategy,
                                                 referenceObject.ObjectType,
                                                 referenceObject.VersionMatchStrategy,
                                                 referenceObject.RecordNotFoundStrategy,
                                                 referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "tagsToMatch", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardGetLatestRecordByTagsOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'tagsToMatch' is an empty enumerable scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardGetLatestRecordByTagsOp>();

                            var result = new StandardGetLatestRecordByTagsOp(
                                                 new List<NamedValue<string>>(),
                                                 referenceObject.TagMatchStrategy,
                                                 referenceObject.ObjectType,
                                                 referenceObject.VersionMatchStrategy,
                                                 referenceObject.RecordNotFoundStrategy,
                                                 referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "tagsToMatch", "is an empty enumerable", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardGetLatestRecordByTagsOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'tagsToMatch' contains a null element scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardGetLatestRecordByTagsOp>();

                            var result = new StandardGetLatestRecordByTagsOp(
                                                 new NamedValue<string>[0].Concat(referenceObject.TagsToMatch).Concat(new NamedValue<string>[] { null }).Concat(referenceObject.TagsToMatch).ToList(),
                                                 referenceObject.TagMatchStrategy,
                                                 referenceObject.ObjectType,
                                                 referenceObject.VersionMatchStrategy,
                                                 referenceObject.RecordNotFoundStrategy,
                                                 referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "tagsToMatch", "contains at least one null element", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardGetLatestRecordByTagsOp>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'tagMatchStrategy' is TagMatchStrategy.Unknown scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardGetLatestRecordByTagsOp>();

                            var result = new StandardGetLatestRecordByTagsOp(
                                referenceObject.TagsToMatch,
                                TagMatchStrategy.Unknown,
                                referenceObject.ObjectType,
                                referenceObject.VersionMatchStrategy,
                                referenceObject.RecordNotFoundStrategy,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "tagMatchStrategy", "Unknown", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardGetLatestRecordByTagsOp>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'recordNotFoundStrategy' is RecordNotFoundStrategy.Unknown scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardGetLatestRecordByTagsOp>();

                            var result = new StandardGetLatestRecordByTagsOp(
                                referenceObject.TagsToMatch,
                                referenceObject.TagMatchStrategy,
                                referenceObject.ObjectType,
                                referenceObject.VersionMatchStrategy,
                                RecordNotFoundStrategy.Unknown,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "recordNotFoundStrategy", "Unknown", },
                    });
        }
    }
}