// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardGetHandlingStatusOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the <see cref="HandlingStatus"/> of one or more records.
    /// </summary>
    public partial class StandardGetHandlingStatusOp : ReturningOperationBase<IReadOnlyCollection<HandlingStatus>>, ISpecifyResourceLocator, IHaveHandleRecordConcern
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardGetHandlingStatusOp"/> class.
        /// </summary>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="internalRecordId">OPTIONAL internal record identifier to match on.  DEFAULT is no matching on internal record id.</param>
        /// <param name="idsToMatch">OPTIONAL string serialized object identifiers to match on.  DEFAULT is no matching on object id.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the object type.  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="tagsToMatch">OPTIONAL tags to match.  DEFAULT is no matching on tags.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to use the defaults of <see cref="Domain.TagMatchStrategy"/> when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        public StandardGetHandlingStatusOp(
            string concern,
            long? internalRecordId = null,
            IReadOnlyCollection<StringSerializedIdentifier> idsToMatch = null,
            VersionMatchStrategy? versionMatchStrategy = null,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = null,
            IResourceLocator specifiedResourceLocator = null)
        {
            concern.ThrowIfInvalidOrReservedConcern();
            var allMatchingParametersAreNull = internalRecordId == null && idsToMatch == null && tagsToMatch == null;
            allMatchingParametersAreNull.MustForArg(nameof(allMatchingParametersAreNull)).BeFalse();

            this.Concern = concern;
            this.InternalRecordId = internalRecordId;
            this.IdsToMatch = idsToMatch;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.TagsToMatch = tagsToMatch;
            this.TagMatchStrategy = tagMatchStrategy;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <inheritdoc />
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the internal record identifier to match or null when not matching on internal record id.
        /// </summary>
        public long? InternalRecordId { get; private set; }

        /// <summary>
        /// Gets the string serialized object identifiers to match on or null when not matching on object id.
        /// </summary>
        public IReadOnlyCollection<StringSerializedIdentifier> IdsToMatch { get; private set; }

        /// <summary>
        /// Gets the strategy to use to filter on the version of the object type.
        /// </summary>
        public VersionMatchStrategy? VersionMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the tags to match or null when not matching on tags.
        /// </summary>
        public IReadOnlyCollection<NamedValue<string>> TagsToMatch { get; private set; }

        /// <summary>
        /// Gets the strategy to use for comparing tags or null to use the defaults of <see cref="Domain.TagMatchStrategy"/> when <see cref="TagsToMatch"/> are specified.
        /// </summary>
        public TagMatchStrategy TagMatchStrategy { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
