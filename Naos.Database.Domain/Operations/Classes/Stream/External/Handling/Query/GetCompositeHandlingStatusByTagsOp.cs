// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetCompositeHandlingStatusByTagsOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the <see cref="CompositeHandlingStatus"/> of a set of records matched by a specified set of tags.
    /// </summary>
    public partial class GetCompositeHandlingStatusByTagsOp : ReturningOperationBase<CompositeHandlingStatus>, IHaveHandleRecordConcern
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCompositeHandlingStatusByTagsOp"/> class.
        /// </summary>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="tagsToMatch">The tags to match.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        public GetCompositeHandlingStatusByTagsOp(
            string concern,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags)
        {
            concern.ThrowIfInvalidOrReservedConcern();
            tagsToMatch.MustForArg(nameof(tagsToMatch)).NotBeNullNorEmptyEnumerableNorContainAnyNulls();
            tagMatchStrategy.MustForArg(nameof(tagMatchStrategy)).NotBeEqualTo(TagMatchStrategy.Unknown);

            this.Concern = concern;
            this.TagsToMatch = tagsToMatch;
            this.TagMatchStrategy = tagMatchStrategy;
        }

        /// <inheritdoc />
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the tags to match.
        /// </summary>
        public IReadOnlyCollection<NamedValue<string>> TagsToMatch { get; private set; }

        /// <summary>
        /// Gets the strategy to use for comparing tags when <see cref="TagsToMatch"/> is specified.
        /// </summary>
        public TagMatchStrategy TagMatchStrategy { get; private set; }
    }
}
