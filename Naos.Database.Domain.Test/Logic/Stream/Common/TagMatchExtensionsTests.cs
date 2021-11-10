// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagMatchExtensionsTests.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Test
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Cloning.Recipes;
    using OBeautifulCode.Type;
    using Xunit;

    public static class TagMatchExtensionsTests
    {
        [Fact]
        public static void FuzzyMatchTags___Should_return_expected_result___When_tagMatchStrategy_is_RecordContainsAnyQueryTag()
        {
            // Arrange
            var recordTags = new List<NamedValue<string>>
            {
                new NamedValue<string>("key1", "value1"),
                new NamedValue<string>("key2", "value2"),
                new NamedValue<string>("key3", "value3"),
            };

            var queryTagsThatMatch = new List<NamedValue<string>>
            {
                new NamedValue<string>("key", "value"),
                new NamedValue<string>("key2", "value2"),
            };

            var queryTagsThatDoNotMatch = new List<NamedValue<string>>
            {
                new NamedValue<string>("key", "value"),
            };

            // Act
            var actualMatch = recordTags.FuzzyMatchTags(
                queryTagsThatMatch,
                TagMatchStrategy.RecordContainsAnyQueryTag);

            var actualDoNotMatch = recordTags.FuzzyMatchTags(
                queryTagsThatDoNotMatch,
                TagMatchStrategy.RecordContainsAnyQueryTag);

            // Assert
            actualMatch.MustForTest().BeTrue();
            actualDoNotMatch.MustForTest().BeFalse();
        }

        [Fact]
        public static void FuzzyMatchTags___Should_return_expected_result___When_tagMatchStrategy_is_RecordContainsAllQueryTags()
        {
            // Arrange
            var recordTags = new List<NamedValue<string>>
            {
                new NamedValue<string>("key1", "value1"),
                new NamedValue<string>("key2", "value2"),
                new NamedValue<string>("key3", "value3"),
            };

            var queryTagsThatMatch = new List<NamedValue<string>>
            {
                new NamedValue<string>("key1", "value1"),
                new NamedValue<string>("key3", "value3"),
            };

            var queryTagsThatDoNotMatch = new List<NamedValue<string>>
            {
                new NamedValue<string>("key1", "value1"),
                new NamedValue<string>("key", "value2"),
            };

            // Act
            var actualMatch = recordTags.FuzzyMatchTags(
                queryTagsThatMatch,
                TagMatchStrategy.RecordContainsAllQueryTags);

            var actualThatDoNotMatch = recordTags.FuzzyMatchTags(
                queryTagsThatDoNotMatch,
                TagMatchStrategy.RecordContainsAllQueryTags);

            // Assert
            actualMatch.MustForTest().BeTrue();

            actualThatDoNotMatch.MustForTest().BeFalse();
        }

        [Fact]
        public static void FuzzyMatchTags___Should_return_expected_result___When_tagMatchStrategy_is_RecordContainsAllQueryTagsAndNoneOther()
        {
            // Arrange
            var recordTags = new List<NamedValue<string>>
            {
                new NamedValue<string>("key1", "value1"),
                new NamedValue<string>("key2", "value2"),
                new NamedValue<string>("key3", "value3"),
            };

            var queryTagsThatMatch = recordTags.DeepClone();

            var queryTagsThatDoNotMatch1 = new List<NamedValue<string>>
            {
                new NamedValue<string>("key", "value"),
            };

            var queryTagsThatDoNotMatch2 = new List<NamedValue<string>>
            {
                new NamedValue<string>("key1", "value1"),
                new NamedValue<string>("key2", "value2"),
            };

            // Act
            var actualMatch = recordTags.FuzzyMatchTags(
                queryTagsThatMatch,
                TagMatchStrategy.RecordContainsAllQueryTagsAndNoneOther);

            var actualDoNotMatch1 = recordTags.FuzzyMatchTags(
                queryTagsThatDoNotMatch1,
                TagMatchStrategy.RecordContainsAllQueryTagsAndNoneOther);

            var actualDoNotMatch2 = recordTags.FuzzyMatchTags(
                queryTagsThatDoNotMatch2,
                TagMatchStrategy.RecordContainsAllQueryTagsAndNoneOther);

            // Assert
            actualMatch.MustForTest().BeTrue();
            actualDoNotMatch1.MustForTest().BeFalse();
            actualDoNotMatch2.MustForTest().BeFalse();
        }
    }
}