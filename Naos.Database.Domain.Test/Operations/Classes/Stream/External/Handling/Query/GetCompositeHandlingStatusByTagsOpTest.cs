// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetCompositeHandlingStatusByTagsOpTest.cs" company="Naos Project">
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
    public static partial class GetCompositeHandlingStatusByTagsOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static GetCompositeHandlingStatusByTagsOpTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<GetCompositeHandlingStatusByTagsOp>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'concern' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<GetCompositeHandlingStatusByTagsOp>();

                            var result = new GetCompositeHandlingStatusByTagsOp(
                                                 null,
                                                 referenceObject.TagsToMatch,
                                                 referenceObject.TagMatchStrategy);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "concern", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<GetCompositeHandlingStatusByTagsOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'concern' is white space scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<GetCompositeHandlingStatusByTagsOp>();

                            var result = new GetCompositeHandlingStatusByTagsOp(
                                                 Invariant($"  {Environment.NewLine}  "),
                                                 referenceObject.TagsToMatch,
                                                 referenceObject.TagMatchStrategy);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "concern", "white space", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<GetCompositeHandlingStatusByTagsOp>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'concern' is reserved",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<GetCompositeHandlingStatusByTagsOp>();

                            var result = new GetCompositeHandlingStatusByTagsOp(
                                Concerns.RecordHandlingConcern,
                                referenceObject.TagsToMatch,
                                referenceObject.TagMatchStrategy);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "concern", "is reserved for internal use and may not be used" },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<GetCompositeHandlingStatusByTagsOp>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'tagsToMatch' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<GetCompositeHandlingStatusByTagsOp>();

                            var result = new GetCompositeHandlingStatusByTagsOp(
                                                 referenceObject.Concern,
                                                 null,
                                                 referenceObject.TagMatchStrategy);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "tagsToMatch", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<GetCompositeHandlingStatusByTagsOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'tagsToMatch' is an empty enumerable scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<GetCompositeHandlingStatusByTagsOp>();

                            var result = new GetCompositeHandlingStatusByTagsOp(
                                                 referenceObject.Concern,
                                                 new List<NamedValue<string>>(),
                                                 referenceObject.TagMatchStrategy);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "tagsToMatch", "is an empty enumerable", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<GetCompositeHandlingStatusByTagsOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'tagsToMatch' contains a null element scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<GetCompositeHandlingStatusByTagsOp>();

                            var result = new GetCompositeHandlingStatusByTagsOp(
                                                 referenceObject.Concern,
                                                 new NamedValue<string>[0].Concat(referenceObject.TagsToMatch).Concat(new NamedValue<string>[] { null }).Concat(referenceObject.TagsToMatch).ToList(),
                                                 referenceObject.TagMatchStrategy);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "tagsToMatch", "contains at least one null element", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<GetCompositeHandlingStatusByTagsOp>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'tagMatchStrategy' is TagMatchStrategy.Unknown scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<GetCompositeHandlingStatusByTagsOp>();

                            var result = new GetCompositeHandlingStatusByTagsOp(
                                referenceObject.Concern,
                                referenceObject.TagsToMatch,
                                TagMatchStrategy.Unknown);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "tagMatchStrategy", "Unknown", },
                    });
        }
    }
}