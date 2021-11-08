// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardGetLatestRecordByTagsOp.cs" company="Naos Project">
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
    /// Gets the latest record with the specified tag.
    /// </summary>
    /// <remarks>
    /// This is an internal operation; it is designed to honor the contract of an <see cref="IStandardStream"/>.
    /// While technically there are no limitations on who may execute this operation on such a stream,
    /// these are "bare metal" operations and can be misused without a deeper understanding of what will happen.
    /// Most typically, you will use the operations that are exposed via these extension methods
    /// <see cref="ReadOnlyStreamExtensions"/> and <see cref="WriteOnlyStreamExtensions"/>.
    /// </remarks>
    public partial class StandardGetLatestRecordByTagsOp : ReturningOperationBase<StreamRecord>, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardGetLatestRecordByTagsOp"/> class.
        /// </summary>
        /// <param name="tagsToMatch">The tags to match.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        public StandardGetLatestRecordByTagsOp(
            IReadOnlyCollection<NamedValue<string>> tagsToMatch,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault,
            IResourceLocator specifiedResourceLocator = null)
        {
            tagsToMatch.MustForArg(nameof(tagsToMatch)).NotBeNullNorEmptyEnumerableNorContainAnyNulls();
            tagMatchStrategy.MustForArg(nameof(tagMatchStrategy)).NotBeEqualTo(TagMatchStrategy.Unknown);
            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();
            recordNotFoundStrategy.MustForArg(nameof(recordNotFoundStrategy)).NotBeEqualTo(RecordNotFoundStrategy.Unknown);

            this.TagsToMatch = tagsToMatch;
            this.TagMatchStrategy = tagMatchStrategy;
            this.ObjectType = objectType;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.RecordNotFoundStrategy = recordNotFoundStrategy;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <summary>
        /// Gets the tags to match.
        /// </summary>
        public IReadOnlyCollection<NamedValue<string>> TagsToMatch { get; private set; }

        /// <summary>
        /// Gets the strategy to use for comparing tags when <see cref="TagsToMatch"/> is specified.
        /// </summary>
        public TagMatchStrategy TagMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the type object to filter on or null for no filter.
        /// </summary>
        public TypeRepresentation ObjectType { get; private set; }

        /// <summary>
        /// Gets the strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).
        /// </summary>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the strategy to use when no record(s) are found.
        /// </summary>
        public RecordNotFoundStrategy RecordNotFoundStrategy { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
