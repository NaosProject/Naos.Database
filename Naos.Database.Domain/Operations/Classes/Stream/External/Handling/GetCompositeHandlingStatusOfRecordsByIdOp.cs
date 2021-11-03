// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetCompositeHandlingStatusOfRecordsByIdOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the composite status of the set of records matched by specified tag matching on locators found by identifiers.
    /// </summary>
    public partial class GetCompositeHandlingStatusOfRecordsByIdOp : ReturningOperationBase<CompositeHandlingStatus>, IHaveHandleRecordConcern
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCompositeHandlingStatusOfRecordsByIdOp"/> class.
        /// </summary>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="idsToMatch">The object identifiers to treat as a composite status.</param>
        /// <param name="handlingStatusCompositionStrategy">The optional strategy for composing statuses.</param>
        /// <param name="versionMatchStrategy">The optional type version match strategy; DEFAULT is Any.</param>
        public GetCompositeHandlingStatusOfRecordsByIdOp(
            string concern,
            IReadOnlyCollection<StringSerializedIdentifier> idsToMatch,
            HandlingStatusCompositionStrategy handlingStatusCompositionStrategy = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
        {
            concern.MustForArg(nameof(concern)).NotBeNullNorWhiteSpace();
            idsToMatch.MustForArg(nameof(idsToMatch)).NotBeNullNorEmptyEnumerableNorContainAnyNulls();
            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();

            this.Concern = concern;
            this.IdsToMatch = idsToMatch;
            this.HandlingStatusCompositionStrategy = handlingStatusCompositionStrategy;
            this.VersionMatchStrategy = versionMatchStrategy;
        }

        /// <inheritdoc />
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the object identifiers to match.
        /// </summary>
        public IReadOnlyCollection<StringSerializedIdentifier> IdsToMatch { get; private set; }

        /// <summary>
        /// Gets the handling status composition strategy.
        /// </summary>
        public HandlingStatusCompositionStrategy HandlingStatusCompositionStrategy { get; private set; }

        /// <summary>
        /// Gets the type version match strategy.
        /// </summary>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }
    }
}
