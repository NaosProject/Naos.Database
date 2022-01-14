// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetDistinctIdsOp{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the distinct identifiers for the supplied filters.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the objects to query.</typeparam>
    public partial class GetDistinctIdsOp<TId> : ReturningOperationBase<IReadOnlyCollection<TId>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDistinctIdsOp{TId}"/> class.
        /// </summary>
        /// <param name="objectTypes">The object types to match on or null when not matching on object type.</param>
        /// <param name="versionMatchStrategy">The strategy to use to filter on the version of the identifier and/or object type.</param>
        /// <param name="tagsToMatch">The tags to match or null when not matching on tags.</param>
        /// <param name="tagMatchStrategy">The strategy to use for comparing tags when <see cref="TagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">The object types used in a record that indicates an identifier deprecation.</param>
        public GetDistinctIdsOp(
            IReadOnlyCollection<TypeRepresentation> objectTypes = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null)
        {
            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();
            tagsToMatch.MustForArg(nameof(tagsToMatch)).NotContainAnyNullElementsWhenNotNull();
            tagMatchStrategy.MustForArg(nameof(tagMatchStrategy)).NotBeEqualTo(TagMatchStrategy.Unknown);

            this.ObjectTypes = objectTypes;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.TagsToMatch = tagsToMatch;
            this.TagMatchStrategy = tagMatchStrategy;
            this.DeprecatedIdTypes = deprecatedIdTypes;
        }

        /// <summary>
        /// Gets the object types to match on or null when not matching on object type.
        /// </summary>
        public IReadOnlyCollection<TypeRepresentation> ObjectTypes { get; private set; }

        /// <summary>
        /// Gets the strategy to use to filter on the version of the identifier and/or object type.
        /// </summary>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the tags to match or null when not matching on tags.
        /// </summary>
        public IReadOnlyCollection<NamedValue<string>> TagsToMatch { get; private set; }

        /// <summary>
        /// Gets the strategy to use for comparing tags when <see cref="TagsToMatch"/> is specified.
        /// </summary>
        public TagMatchStrategy TagMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the object types used in a record that indicates an identifier deprecation.
        /// </summary>
        public IReadOnlyCollection<TypeRepresentation> DeprecatedIdTypes { get; private set; }
    }
}
