﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardGetHandlingStatusOpTest.cs" company="Naos Project">
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
    public static partial class StandardGetHandlingStatusOpTest
    {
        [SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification = ObcSuppressBecause.CA1505_AvoidUnmaintainableCode_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = ObcSuppressBecause.CA1810_InitializeReferenceTypeStaticFieldsInline_FieldsDeclaredInCodeGeneratedPartialTestClass)]
        static StandardGetHandlingStatusOpTest()
        {
            ConstructorArgumentValidationTestScenarios
                .RemoveAllScenarios()
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardGetHandlingStatusOp>
                    {
                        Name = "constructor should throw ArgumentNullException when parameter 'concern' is null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardGetHandlingStatusOp>();

                            var result = new StandardGetHandlingStatusOp(
                                                 null,
                                                 referenceObject.InternalRecordId,
                                                 referenceObject.IdsToMatch,
                                                 referenceObject.VersionMatchStrategy,
                                                 referenceObject.TagsToMatch,
                                                 referenceObject.TagMatchStrategy,
                                                 referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentNullException),
                        ExpectedExceptionMessageContains = new[] { "concern", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardGetHandlingStatusOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'concern' is white space scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardGetHandlingStatusOp>();

                            var result = new StandardGetHandlingStatusOp(
                                                 Invariant($"  {Environment.NewLine}  "),
                                                 referenceObject.InternalRecordId,
                                                 referenceObject.IdsToMatch,
                                                 referenceObject.VersionMatchStrategy,
                                                 referenceObject.TagsToMatch,
                                                 referenceObject.TagMatchStrategy,
                                                 referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "concern", "white space", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardGetHandlingStatusOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'concern' is reserved",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardGetHandlingStatusOp>();

                            var result = new StandardGetHandlingStatusOp(
                                Concerns.RecordHandlingConcern,
                                referenceObject.InternalRecordId,
                                referenceObject.IdsToMatch,
                                referenceObject.VersionMatchStrategy,
                                referenceObject.TagsToMatch,
                                referenceObject.TagMatchStrategy,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "concern", "is reserved for internal use and may not be used", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardGetHandlingStatusOp>
                    {
                        Name = "constructor should throw ArgumentException when parameter 'tagsToMatch' contains a null element",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardGetHandlingStatusOp>();

                            var result = new StandardGetHandlingStatusOp(
                                referenceObject.Concern,
                                referenceObject.InternalRecordId,
                                referenceObject.IdsToMatch,
                                referenceObject.VersionMatchStrategy,
                                new[] { A.Dummy<NamedValue<string>>(), null, A.Dummy<NamedValue<string>>() },
                                referenceObject.TagMatchStrategy,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "tagsToMatch", "contains at least one null element", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardGetHandlingStatusOp>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameter 'tagMatchStrategy' is TagMatchStrategy.Unknown scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardGetHandlingStatusOp>();

                            var result = new StandardGetHandlingStatusOp(
                                referenceObject.Concern,
                                referenceObject.InternalRecordId,
                                referenceObject.IdsToMatch,
                                referenceObject.VersionMatchStrategy,
                                referenceObject.TagsToMatch,
                                TagMatchStrategy.Unknown,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentOutOfRangeException),
                        ExpectedExceptionMessageContains = new[] { "tagMatchStrategy", "contains at least one null element", },
                    })
                .AddScenario(() =>
                    new ConstructorArgumentValidationTestScenario<StandardGetHandlingStatusOp>
                    {
                        Name = "constructor should throw ArgumentOutOfRangeException when parameters 'internalRecordId', 'idsToMatch', and 'tagsToMatch' are all null scenario",
                        ConstructionFunc = () =>
                        {
                            var referenceObject = A.Dummy<StandardGetHandlingStatusOp>();

                            var result = new StandardGetHandlingStatusOp(
                                referenceObject.Concern,
                                null,
                                null,
                                referenceObject.VersionMatchStrategy,
                                null,
                                referenceObject.TagMatchStrategy,
                                referenceObject.SpecifiedResourceLocator);

                            return result;
                        },
                        ExpectedExceptionType = typeof(ArgumentException),
                        ExpectedExceptionMessageContains = new[] { "allMatchingParametersAreNull", },
                    });
        }
    }
}