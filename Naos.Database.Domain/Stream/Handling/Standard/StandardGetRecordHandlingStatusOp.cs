// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardGetRecordHandlingStatusOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Handles a record.
    /// </summary>
    public partial class StandardGetRecordHandlingStatusOp : ReturningOperationBase<HandlingStatus>, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardGetRecordHandlingStatusOp"/> class.
        /// </summary>
        /// <param name="concern">The handling concern.</param>
        /// <param name="internalRecordId">The internal records identifier.</param>
        /// <param name="idsToMatch">The object identifiers to match.</param>
        /// <param name="versionMatchStrategy">Version match strategy for use with <paramref name="idsToMatch"/> if applicable.</param>
        /// <param name="tagsToMatch">The internal record ids to treat as a composite status.</param>
        /// <param name="tagMatchStrategy">The strategy for comparing tags from <paramref name="tagsToMatch"/> if applicable.</param>
        /// <param name="handlingStatusCompositionStrategy">The <see cref="HandlingStatusCompositionStrategy"/> to use when consolidating the <see cref="HandlingStatus"/> to a single answer.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        public StandardGetRecordHandlingStatusOp(
            string concern,
            long? internalRecordId = null,
            IReadOnlyCollection<StringSerializedIdentifier> idsToMatch = null,
            VersionMatchStrategy? versionMatchStrategy = null,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = null,
            HandlingStatusCompositionStrategy handlingStatusCompositionStrategy = null,
            IResourceLocator specifiedResourceLocator = null)
        {
            concern.MustForArg(nameof(concern)).NotBeNullNorWhiteSpace();

            var allMatchingParametersAreNull = internalRecordId == null && idsToMatch == null && tagsToMatch == null;
            allMatchingParametersAreNull.MustForArg(nameof(allMatchingParametersAreNull)).BeFalse();

            this.Concern = concern;
            this.InternalRecordId = internalRecordId;
            this.IdsToMatch = idsToMatch;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.TagsToMatch = tagsToMatch;
            this.TagMatchStrategy = tagMatchStrategy;
            this.HandlingStatusCompositionStrategy = handlingStatusCompositionStrategy;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <summary>
        /// Gets the handling concern.
        /// </summary>
        /// <value>The handling concern.</value>
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the concern.
        /// </summary>
        /// <value>The concern.</value>
        public long? InternalRecordId { get; private set; }

        /// <summary>
        /// Gets the object identifiers to match.
        /// </summary>
        /// <value>The object identifiers to match.</value>
        public IReadOnlyCollection<StringSerializedIdentifier> IdsToMatch { get; private set; }

        /// <summary>
        /// Gets the version match strategy.
        /// </summary>
        /// <value>The version match strategy.</value>
        public VersionMatchStrategy? VersionMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the tags to match.
        /// </summary>
        /// <value>The tags to match.</value>
        public IReadOnlyCollection<NamedValue<string>> TagsToMatch { get; private set; }

        /// <summary>
        /// Gets the tag match strategy.
        /// </summary>
        /// <value>The tag match strategy.</value>
        public TagMatchStrategy TagMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the handling status composition strategy.
        /// </summary>
        /// <value>The handling status composition strategy.</value>
        public HandlingStatusCompositionStrategy HandlingStatusCompositionStrategy { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
