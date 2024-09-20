// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetCompositeHandlingStatusByIdsOp{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the <see cref="CompositeHandlingStatus"/> of a set of records matched by ids.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public partial class GetCompositeHandlingStatusByIdsOp<TId> : ReturningOperationBase<CompositeHandlingStatus>, IHaveHandleRecordConcern
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCompositeHandlingStatusByIdsOp{TId}"/> class.
        /// </summary>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="idsToMatch">The object identifiers to match.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        public GetCompositeHandlingStatusByIdsOp(
            string concern,
            IReadOnlyCollection<TId> idsToMatch,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType)
        {
            concern.ThrowIfInvalidOrReservedConcern();
            idsToMatch.MustForArg(nameof(idsToMatch)).NotBeNullNorEmptyEnumerableNorContainAnyNulls();
            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();
            typeSelectionStrategy.MustForArg(nameof(typeSelectionStrategy)).NotBeEqualTo(TypeSelectionStrategy.Unknown);

            this.Concern = concern;
            this.IdsToMatch = idsToMatch;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.TypeSelectionStrategy = typeSelectionStrategy;
        }

        /// <inheritdoc />
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the object identifiers to match.
        /// </summary>
        public IReadOnlyCollection<TId> IdsToMatch { get; private set; }

        /// <summary>
        /// Gets the strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).
        /// </summary>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).
        /// </summary>
        public TypeSelectionStrategy TypeSelectionStrategy { get; private set; }
    }
}
