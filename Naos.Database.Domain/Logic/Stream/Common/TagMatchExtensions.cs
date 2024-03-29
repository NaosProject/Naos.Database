﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagMatchExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Extensions used for <see cref="TagMatchStrategy"/>.
    /// </summary>
    public static class TagMatchExtensions
    {
        /// <summary>
        /// Fuzzy matches according to strategy.
        /// </summary>
        /// <param name="recordTags">The record tags.</param>
        /// <param name="queryTags">The query tags.</param>
        /// <param name="tagMatchStrategy">The strategy to use for comparing tags.</param>
        /// <returns>
        /// <c>true</c> if the tags match, otherwise <c>false</c>.
        /// </returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = NaosSuppressBecause.CA1502_AvoidExcessiveComplexity_DisagreeWithAssessment)]
        public static bool FuzzyMatchTags(
            this IReadOnlyCollection<NamedValue<string>> recordTags,
            IReadOnlyCollection<NamedValue<string>> queryTags,
            TagMatchStrategy tagMatchStrategy)
        {
            recordTags.MustForArg(nameof(queryTags)).NotContainAnyNullElementsWhenNotNull();
            queryTags.MustForArg(nameof(queryTags)).NotContainAnyNullElementsWhenNotNull();
            tagMatchStrategy.MustForArg(nameof(tagMatchStrategy)).NotBeEqualTo(TagMatchStrategy.Unknown);

            // We won't be able to construct an operation that causes this method to execute with empty query tags.
            ////if (!queryTags.Any())
            ////{
            ////    return false;
            ////}

            var localRecordTags = recordTags ?? new List<NamedValue<string>>();

            if (!localRecordTags.Any())
            {
                // Short-circuited; would have returned false on all branches below.
                return false;
            }

            bool result;

            if (tagMatchStrategy == TagMatchStrategy.RecordContainsAnyQueryTag)
            {
                result = queryTags.Intersect(localRecordTags).Any();
            }
            else if (tagMatchStrategy == TagMatchStrategy.RecordContainsAllQueryTags)
            {
                result = !queryTags.Except(localRecordTags).Any();
            }
            else if (tagMatchStrategy == TagMatchStrategy.RecordContainsAllQueryTagsAndNoneOther)
            {
                result = queryTags.Distinct().IsEqualTo(localRecordTags.Distinct());
            }
            else
            {
                throw new NotSupportedException(Invariant($"This {nameof(TagMatchStrategy)} is not supported {tagMatchStrategy}."));
            }

            return result;
        }
    }
}