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
    public partial class GetDistinctIdsOp<TId> : ReturningOperationBase<IReadOnlyCollection<TId>>, ISpecifyRecordsToFilterSelectionStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDistinctIdsOp{TId}"/> class.
        /// </summary>
        /// <param name="objectTypes">OPTIONAL object types to match on or null when not matching on object type.  DEFAULT is not to match on object types.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="tagsToMatch">OPTIONAL tags to match or null when not matching on tags.  DEFAULT is not to match on tags.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="recordsToFilterSelectionStrategy">OPTIONAL strategy for selecting records before filtering.  DEFAULT is to select all records.</param>
        public GetDistinctIdsOp(
            IReadOnlyCollection<TypeRepresentation> objectTypes = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            RecordsToFilterSelectionStrategy recordsToFilterSelectionStrategy = RecordsToFilterSelectionStrategy.All)
        {
            objectTypes.MustForArg(nameof(objectTypes)).NotContainAnyNullElementsWhenNotNull();
            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();
            tagsToMatch.MustForArg(nameof(tagsToMatch)).NotContainAnyNullElementsWhenNotNull();
            tagMatchStrategy.MustForArg(nameof(tagMatchStrategy)).NotBeEqualTo(TagMatchStrategy.Unknown);
            deprecatedIdTypes.MustForArg(nameof(deprecatedIdTypes)).NotContainAnyNullElementsWhenNotNull();
            recordsToFilterSelectionStrategy.MustForArg(nameof(recordsToFilterSelectionStrategy)).NotBeEqualTo(RecordsToFilterSelectionStrategy.Unknown);

            this.ObjectTypes = objectTypes;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.TagsToMatch = tagsToMatch;
            this.TagMatchStrategy = tagMatchStrategy;
            this.DeprecatedIdTypes = deprecatedIdTypes;
            this.RecordsToFilterSelectionStrategy = recordsToFilterSelectionStrategy;
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

        /// <inheritdoc />
        public RecordsToFilterSelectionStrategy RecordsToFilterSelectionStrategy { get; private set; }
    }
}
