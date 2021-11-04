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
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to use the defaults of <see cref="Domain.TagMatchStrategy"/> when <paramref name="tagsToMatch"/> is specified.</param>
        public GetCompositeHandlingStatusByTagsOp(
            string concern,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch,
            TagMatchStrategy tagMatchStrategy = null)
        {
            concern.ThrowIfInvalidOrReservedConcern();
            tagsToMatch.MustForArg(nameof(tagsToMatch)).NotBeNullNorEmptyEnumerableNorContainAnyNulls();

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
        /// Gets the strategy to use for comparing tags or null to use the defaults of <see cref="Domain.TagMatchStrategy"/>.
        /// </summary>
        public TagMatchStrategy TagMatchStrategy { get; private set; }
    }
}
