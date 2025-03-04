// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestObjectsByIdOp{TId,TObject}.cs" company="Naos Project">
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
    /// Gets the most recent objects for a specified set of identifiers or for all uniquely identified objects in the stream.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public partial class GetLatestObjectsByIdOp<TId, TObject> : ReturningOperationBase<IReadOnlyList<TObject>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestObjectsByIdOp{TId, TObject}"/> class.
        /// </summary>
        /// <param name="ids">The identifiers of the objects.  If null or empty then the operation returns the most recently Put object for each unique identifier.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="tagsToMatch">OPTIONAL tags to match.  DEFAULT is no matching on tags.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return an empty collection.</param>
        /// <param name="orderRecordsBy">OPTIONAL value that specifies how to order the resulting records.  DEFAULT is random.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        public GetLatestObjectsByIdOp(
            IReadOnlyCollection<TId> ids,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault,
            OrderRecordsBy orderRecordsBy = OrderRecordsBy.Random,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType)
        {
            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();
            tagsToMatch.MustForArg(nameof(tagsToMatch)).NotContainAnyNullElementsWhenNotNull();
            tagMatchStrategy.MustForArg(nameof(tagMatchStrategy)).NotBeEqualTo(TagMatchStrategy.Unknown);
            recordNotFoundStrategy.MustForArg(nameof(recordNotFoundStrategy)).NotBeEqualTo(RecordNotFoundStrategy.Unknown);
            orderRecordsBy.MustForArg(nameof(orderRecordsBy)).NotBeEqualTo(OrderRecordsBy.Unknown);
            deprecatedIdTypes.MustForArg(nameof(deprecatedIdTypes)).NotContainAnyNullElementsWhenNotNull();
            typeSelectionStrategy.MustForArg(nameof(typeSelectionStrategy)).NotBeEqualTo(TypeSelectionStrategy.Unknown);

            this.Ids = ids;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.TagsToMatch = tagsToMatch;
            this.TagMatchStrategy = tagMatchStrategy;
            this.RecordNotFoundStrategy = recordNotFoundStrategy;
            this.OrderRecordsBy = orderRecordsBy;
            this.DeprecatedIdTypes = deprecatedIdTypes;
            this.TypeSelectionStrategy = typeSelectionStrategy;
        }

        /// <summary>
        /// Gets the identifiers of the objects.  If null or empty then the operation returns the most recently Put object for each unique identifier.
        /// </summary>
        public IReadOnlyCollection<TId> Ids { get; private set; }

        /// <summary>
        /// Gets the strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).
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
        /// Gets the strategy to use when no record(s) are found.
        /// </summary>
        public RecordNotFoundStrategy RecordNotFoundStrategy { get; private set; }

        /// <summary>
        /// Gets a value that specifies how to order the resulting records.
        /// </summary>
        public OrderRecordsBy OrderRecordsBy { get; private set; }

        /// <summary>
        /// Gets the object types used in a record that indicates an identifier deprecation.
        /// </summary>
        public IReadOnlyCollection<TypeRepresentation> DeprecatedIdTypes { get; private set; }

        /// <summary>
        /// Gets the strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).
        /// </summary>
        public TypeSelectionStrategy TypeSelectionStrategy { get; private set; }
    }
}
