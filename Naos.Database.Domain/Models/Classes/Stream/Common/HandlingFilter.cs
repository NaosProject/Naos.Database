// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandlingFilter.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Filter set to apply to a set of <see cref="StreamRecordHandlingEntry"/>'s.
    /// </summary>
    public partial class HandlingFilter : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlingFilter"/> class.
        /// </summary>
        /// <param name="currentHandlingStatuses">OPTIONAL current statuses to match.  DEFAULT is to not filter on <see cref="HandlingStatus"/>.  Empty collection will be treated as null.</param>
        /// <param name="tags">OPTIONAL tags to match.  DEFAULT is to not matching on tags.  Empty collection will be treated as null.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags when <see cref="Tags"/> is specified.  DEFAULT is to match if the record contains all of the queried tags (with extra tags on the record ignored).</param>
        public HandlingFilter(
            IReadOnlyCollection<HandlingStatus> currentHandlingStatuses = null,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags)
        {
            currentHandlingStatuses.MustForArg(nameof(currentHandlingStatuses)).NotContainElementWhenNotNull(HandlingStatus.Unknown);
            tags.MustForArg(nameof(tags)).NotContainAnyNullElementsWhenNotNull();
            tagMatchStrategy.MustForArg(nameof(tagMatchStrategy)).NotBeEqualTo(TagMatchStrategy.Unknown);

            this.CurrentHandlingStatuses = currentHandlingStatuses;
            this.Tags = tags;
            this.TagMatchStrategy = tagMatchStrategy;
        }

        /// <summary>
        /// Gets the current handling statuses to match.
        /// </summary>
        public IReadOnlyCollection<HandlingStatus> CurrentHandlingStatuses { get; private set; }

        /// <summary>
        /// Gets the tags to match or null when not matching on tags.
        /// </summary>
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }

        /// <summary>
        /// Gets the strategy to use for comparing tags when <see cref="Tags"/> is specified.
        /// </summary>
        public TagMatchStrategy TagMatchStrategy { get; private set; }
    }
}
