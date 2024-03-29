﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryHandleRecordWithIdOp{TId,TObject}Test.cs" company="Naos Project">
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
    public static partial class TryHandleRecordWithIdOpTIdTObjectTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static TryHandleRecordWithIdOpTIdTObjectTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<TryHandleRecordWithIdOp<Version, Version>>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'concern' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<TryHandleRecordWithIdOp<Version, Version>>();

                            var result = new TryHandleRecordWithIdOp<Version, Version>(
                                                 null,
                                                 referenceObject.VersionMatchStrategy,
                                                 referenceObject.TagsToMatch,
                                                 referenceObject.TagMatchStrategy,
                                                 referenceObject.OrderRecordsBy,
                                                 referenceObject.Tags,
                                                 referenceObject.Details,
                                                 referenceObject.MinimumInternalRecordId,
                                                 referenceObject.InheritRecordTags);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "concern", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<TryHandleRecordWithIdOp<Version, Version>>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'concern' is white space scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<TryHandleRecordWithIdOp<Version, Version>>();

                            var result = new TryHandleRecordWithIdOp<Version, Version>(
                                                 Invariant($"  {Environment.NewLine}  "),
                                                 referenceObject.VersionMatchStrategy,
                                                 referenceObject.TagsToMatch,
                                                 referenceObject.TagMatchStrategy,
                                                 referenceObject.OrderRecordsBy,
                                                 referenceObject.Tags,
                                                 referenceObject.Details,
                                                 referenceObject.MinimumInternalRecordId,
                                                 referenceObject.InheritRecordTags);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "concern", "white space", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<TryHandleRecordWithIdOp<Version, Version>>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'concern' is reserved scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<TryHandleRecordWithIdOp<Version, Version>>();

                            var result = new TryHandleRecordWithIdOp<Version, Version>(
                                Concerns.StreamHandlingDisabledConcern,
                                referenceObject.VersionMatchStrategy,
                                referenceObject.TagsToMatch,
                                referenceObject.TagMatchStrategy,
                                referenceObject.OrderRecordsBy,
                                referenceObject.Tags,
                                referenceObject.Details,
                                referenceObject.MinimumInternalRecordId,
                                referenceObject.InheritRecordTags);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "concern", "is reserved for internal use and may not be used", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<TryHandleRecordWithIdOp<Version, Version>>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'tagsToMatch' contains a null element",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<TryHandleRecordWithIdOp<Version, Version>>();

                            var result = new TryHandleRecordWithIdOp<Version, Version>(
                                referenceObject.Concern,
                                referenceObject.VersionMatchStrategy,
                                new[] { A.Dummy<NamedValue<string>>(), null, A.Dummy<NamedValue<string>>() },
                                referenceObject.TagMatchStrategy,
                                referenceObject.OrderRecordsBy,
                                referenceObject.Tags,
                                referenceObject.Details,
                                referenceObject.MinimumInternalRecordId,
                                referenceObject.InheritRecordTags);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "tagsToMatch", "contains at least one null element", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<TryHandleRecordWithIdOp<Version, Version>>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'tagMatchStrategy' is TagMatchStrategy.Unknown scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<TryHandleRecordWithIdOp<Version, Version>>();

                            var result = new TryHandleRecordWithIdOp<Version, Version>(
                                referenceObject.Concern,
                                referenceObject.VersionMatchStrategy,
                                referenceObject.TagsToMatch,
                                TagMatchStrategy.Unknown,
                                referenceObject.OrderRecordsBy,
                                referenceObject.Tags,
                                referenceObject.Details,
                                referenceObject.MinimumInternalRecordId,
                                referenceObject.InheritRecordTags);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "tagMatchStrategy", "Unknown", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<TryHandleRecordWithIdOp<Version, Version>>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'orderRecordsBy' is OrderRecordsBy.Unknown scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<TryHandleRecordWithIdOp<Version, Version>>();

                            var result = new TryHandleRecordWithIdOp<Version, Version>(
                                referenceObject.Concern,
                                referenceObject.VersionMatchStrategy,
                                referenceObject.TagsToMatch,
                                referenceObject.TagMatchStrategy,
                                OrderRecordsBy.Unknown,
                                referenceObject.Tags,
                                referenceObject.Details,
                                referenceObject.MinimumInternalRecordId,
                                referenceObject.InheritRecordTags);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "orderRecordsBy", "Unknown", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<TryHandleRecordWithIdOp<Version, Version>>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'tags' contains a null element",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<TryHandleRecordWithIdOp<Version, Version>>();

                            var result = new TryHandleRecordWithIdOp<Version, Version>(
                                referenceObject.Concern,
                                referenceObject.VersionMatchStrategy,
                                referenceObject.TagsToMatch,
                                referenceObject.TagMatchStrategy,
                                referenceObject.OrderRecordsBy,
                                new[] { A.Dummy<NamedValue<string>>(), null, A.Dummy<NamedValue<string>>() },
                                referenceObject.Details,
                                referenceObject.MinimumInternalRecordId,
                                referenceObject.InheritRecordTags);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "tags", "contains at least one null element", },
                    });
        }
    }
}