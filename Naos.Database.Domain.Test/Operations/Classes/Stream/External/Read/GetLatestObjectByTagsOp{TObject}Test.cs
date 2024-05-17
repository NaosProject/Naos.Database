// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestObjectByTagsOp{TObject}Test.cs" company="Naos Project">
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
    using OBeautifulCode.Type;
    using Xunit;

    using static System.FormattableString;

    [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
    public static partial class GetLatestObjectByTagsOpTObjectTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static GetLatestObjectByTagsOpTObjectTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<GetLatestObjectByTagsOp<Version>>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'tagsToMatch' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<GetLatestObjectByTagsOp<Version>>();

                            var result = new GetLatestObjectByTagsOp<Version>(
                                                 null,
                                                 referenceObject.TagMatchStrategy,
                                                 referenceObject.VersionMatchStrategy,
                                                 referenceObject.RecordNotFoundStrategy,
                                                 referenceObject.DeprecatedIdTypes);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "tagsToMatch", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<GetLatestObjectByTagsOp<Version>>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'tagsToMatch' is an empty enumerable scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<GetLatestObjectByTagsOp<Version>>();

                            var result = new GetLatestObjectByTagsOp<Version>(
                                                 new List<NamedValue<string>>(),
                                                 referenceObject.TagMatchStrategy,
                                                 referenceObject.VersionMatchStrategy,
                                                 referenceObject.RecordNotFoundStrategy,
                                                 referenceObject.DeprecatedIdTypes);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "tagsToMatch", "is an empty enumerable", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<GetLatestObjectByTagsOp<Version>>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'tagsToMatch' contains a null element scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<GetLatestObjectByTagsOp<Version>>();

                            var result = new GetLatestObjectByTagsOp<Version>(
                                                 new NamedValue<string>[0].Concat(referenceObject.TagsToMatch).Concat(new NamedValue<string>[] { null }).Concat(referenceObject.TagsToMatch).ToList(),
                                                 referenceObject.TagMatchStrategy,
                                                 referenceObject.VersionMatchStrategy,
                                                 referenceObject.RecordNotFoundStrategy,
                                                 referenceObject.DeprecatedIdTypes);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "tagsToMatch", "contains at least one null element", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<GetLatestObjectByTagsOp<Version>>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'deprecatedIdTypes' contains a null element scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<GetLatestObjectByTagsOp<Version>>();

                            var result = new GetLatestObjectByTagsOp<Version>(
                                                 referenceObject.TagsToMatch,
                                                 referenceObject.TagMatchStrategy,
                                                 referenceObject.VersionMatchStrategy,
                                                 referenceObject.RecordNotFoundStrategy,
                                                 new TypeRepresentation[0].Concat(referenceObject.DeprecatedIdTypes).Concat(new TypeRepresentation[] { null }).Concat(referenceObject.DeprecatedIdTypes).ToList());

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "deprecatedIdTypes", "contains at least one null element", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<GetLatestObjectByTagsOp<Version>>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'tagMatchStrategy' is TagMatchStrategy.Unknown scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<GetLatestObjectByTagsOp<Version>>();

                            var result = new GetLatestObjectByTagsOp<Version>(
                                referenceObject.TagsToMatch,
                                TagMatchStrategy.Unknown,
                                referenceObject.VersionMatchStrategy,
                                referenceObject.RecordNotFoundStrategy,
                                referenceObject.DeprecatedIdTypes);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "tagMatchStrategy", "Unknown", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<GetLatestObjectByTagsOp<Version>>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'recordNotFoundStrategy' is RecordNotFoundStrategy.Unknown scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<GetLatestObjectByTagsOp<Version>>();

                            var result = new GetLatestObjectByTagsOp<Version>(
                                referenceObject.TagsToMatch,
                                referenceObject.TagMatchStrategy,
                                referenceObject.VersionMatchStrategy,
                                RecordNotFoundStrategy.Unknown,
                                referenceObject.DeprecatedIdTypes);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "recordNotFoundStrategy", "Unknown", },
                    });
        }
    }
}