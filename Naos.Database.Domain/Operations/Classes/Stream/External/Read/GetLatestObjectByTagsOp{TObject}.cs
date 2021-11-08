// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestObjectByTagsOp{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the most recent object with the specified tags.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public partial class GetLatestObjectByTagsOp<TObject> : ReturningOperationBase<TObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestObjectByTagsOp{TObject}"/> class.
        /// </summary>
        /// <param name="tagsToMatch">The tags to match.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public GetLatestObjectByTagsOp(
            IReadOnlyCollection<NamedValue<string>> tagsToMatch,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault)
        {
            tagsToMatch.MustForArg(nameof(tagsToMatch)).NotBeNullNorEmptyEnumerableNorContainAnyNulls();
            tagMatchStrategy.MustForArg(nameof(tagMatchStrategy)).NotBeEqualTo(TagMatchStrategy.Unknown);
            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();
            recordNotFoundStrategy.MustForArg(nameof(recordNotFoundStrategy)).NotBeEqualTo(RecordNotFoundStrategy.Unknown);

            this.TagsToMatch = tagsToMatch;
            this.TagMatchStrategy = tagMatchStrategy;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.RecordNotFoundStrategy = recordNotFoundStrategy;
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
        /// Gets the strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).
        /// </summary>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the strategy to use when no record(s) are found.
        /// </summary>
        public RecordNotFoundStrategy RecordNotFoundStrategy { get; private set; }
    }
}
