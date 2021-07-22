// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagMatchExtensionsTests.cs" company="Naos Project">
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
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Cloning.Recipes;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.CodeGen.ModelObject.Recipes;
    using OBeautifulCode.Math.Recipes;
    using OBeautifulCode.Type;
    using Xunit;

    using static System.FormattableString;

    public static partial class TagMatchExtensionsTests
    {
        [Fact]
        public static void FuzzyMatchAccordingToStrategy___AllAny()
        {
            var targetSet = new List<NamedValue<string>>
                            {
                                new NamedValue<string>("key1", "value1"),
                                new NamedValue<string>("key2", "value2"),
                                new NamedValue<string>("key3", "value3"),
                            };

            var findSetMatch = new List<NamedValue<string>>
                               {
                                   new NamedValue<string>("key1", "value1"),
                               };

            var findSetNoMatch = new List<NamedValue<string>>
                                 {
                                     new NamedValue<string>("key", "value"),
                                 };

            var resultMatch = findSetMatch.FuzzyMatchAccordingToStrategy(
                targetSet,
                new TagMatchStrategy(TagMatchScope.All, TagMatchScope.Any));

            resultMatch.MustForTest().BeTrue();

            var resultNoMatch = findSetNoMatch.FuzzyMatchAccordingToStrategy(
                targetSet,
                new TagMatchStrategy(TagMatchScope.All, TagMatchScope.Any));

            resultNoMatch.MustForTest().BeFalse();
        }

        [Fact]
        public static void FuzzyMatchAccordingToStrategy___AnyAny()
        {
            var targetSet = new List<NamedValue<string>>
                            {
                                new NamedValue<string>("key1", "value1"),
                                new NamedValue<string>("key2", "value2"),
                                new NamedValue<string>("key3", "value3"),
                            };

            var findSetMatch = new List<NamedValue<string>>
                               {
                                   new NamedValue<string>("key1", "value1"),
                                   new NamedValue<string>("key2", "value2"),
                               };

            var findSetNoMatch = new List<NamedValue<string>>
                                 {
                                     new NamedValue<string>("key", "value"),
                                 };

            var resultMatch = findSetMatch.FuzzyMatchAccordingToStrategy(
                targetSet,
                new TagMatchStrategy(TagMatchScope.Any, TagMatchScope.Any));

            resultMatch.MustForTest().BeTrue();

            var resultNoMatch = findSetNoMatch.FuzzyMatchAccordingToStrategy(
                targetSet,
                new TagMatchStrategy(TagMatchScope.Any, TagMatchScope.Any));

            resultNoMatch.MustForTest().BeFalse();
        }

        [Fact]
        public static void FuzzyMatchAccordingToStrategy___AllAll()
        {
            var targetSet = new List<NamedValue<string>>
                            {
                                new NamedValue<string>("key1", "value1"),
                                new NamedValue<string>("key2", "value2"),
                                new NamedValue<string>("key3", "value3"),
                            };

            var findSetMatch = targetSet.DeepClone();

            var findSetNoMatchOne = new List<NamedValue<string>>
                                 {
                                     new NamedValue<string>("key", "value"),
                                 };

            var findSetNoMatchTwo = new List<NamedValue<string>>
                               {
                                   new NamedValue<string>("key1", "value1"),
                                   new NamedValue<string>("key2", "value2"),
                               };

            var resultMatch = findSetMatch.FuzzyMatchAccordingToStrategy(
                targetSet,
                new TagMatchStrategy(TagMatchScope.All, TagMatchScope.All));

            resultMatch.MustForTest().BeTrue();

            var resultNoMatchOne = findSetNoMatchOne.FuzzyMatchAccordingToStrategy(
                targetSet,
                new TagMatchStrategy(TagMatchScope.All, TagMatchScope.All));

            resultNoMatchOne.MustForTest().BeFalse();

            var resultNoMatchTwo = findSetNoMatchTwo.FuzzyMatchAccordingToStrategy(
                targetSet,
                new TagMatchStrategy(TagMatchScope.All, TagMatchScope.All));

            resultNoMatchTwo.MustForTest().BeFalse();
        }
    }
}