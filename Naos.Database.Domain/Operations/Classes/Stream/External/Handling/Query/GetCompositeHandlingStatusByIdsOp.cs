// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetCompositeHandlingStatusByIdsOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the <see cref="CompositeHandlingStatus"/> of a set of records matched by a specified set of string serialized ids.
    /// </summary>
    public partial class GetCompositeHandlingStatusByIdsOp : ReturningOperationBase<CompositeHandlingStatus>, IHaveHandleRecordConcern
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCompositeHandlingStatusByIdsOp"/> class.
        /// </summary>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="idsToMatch">The string serialized object identifiers to match.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        public GetCompositeHandlingStatusByIdsOp(
            string concern,
            IReadOnlyCollection<StringSerializedIdentifier> idsToMatch,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
        {
            concern.ThrowIfInvalidOrReservedConcern();
            idsToMatch.MustForArg(nameof(idsToMatch)).NotBeNullNorEmptyEnumerableNorContainAnyNulls();
            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();

            this.Concern = concern;
            this.IdsToMatch = idsToMatch;
            this.VersionMatchStrategy = versionMatchStrategy;
        }

        /// <inheritdoc />
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the string serialized object identifiers to match.
        /// </summary>
        public IReadOnlyCollection<StringSerializedIdentifier> IdsToMatch { get; private set; }

        /// <summary>
        /// Gets the strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).
        /// </summary>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }
    }
}
