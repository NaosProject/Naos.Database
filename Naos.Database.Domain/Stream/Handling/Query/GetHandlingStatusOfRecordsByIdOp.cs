// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetHandlingStatusOfRecordsByIdOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the composite status of the set of records matched by specified tag matching on locators found by identifiers.
    /// </summary>
    public partial class GetHandlingStatusOfRecordsByIdOp : ReturningOperationBase<HandlingStatus>, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetHandlingStatusOfRecordsByIdOp"/> class.
        /// </summary>
        /// <param name="concern">The handling concern.</param>
        /// <param name="idsToMatch">The object identifiers to treat as a composite status.</param>
        /// <param name="handlingStatusCompositionStrategy">The optional strategy for composing statuses.</param>
        /// <param name="versionMatchStrategy">The optional type version match strategy; DEFAULT is Any.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        public GetHandlingStatusOfRecordsByIdOp(
            string concern,
            IReadOnlyCollection<StringSerializedIdentifier> idsToMatch,
            HandlingStatusCompositionStrategy handlingStatusCompositionStrategy = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IResourceLocator specifiedResourceLocator = null)
        {
            concern.MustForArg(nameof(concern)).NotBeNullNorWhiteSpace();
            idsToMatch.MustForArg(nameof(idsToMatch)).NotBeNullNorEmptyEnumerableNorContainAnyNulls();

            this.Concern = concern;
            this.IdsToMatch = idsToMatch;
            this.HandlingStatusCompositionStrategy = handlingStatusCompositionStrategy;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <summary>
        /// Gets the handling concern.
        /// </summary>
        /// <value>The handling concern.</value>
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the object identifiers to match.
        /// </summary>
        /// <value>The object identifiers to match.</value>
        public IReadOnlyCollection<StringSerializedIdentifier> IdsToMatch { get; private set; }

        /// <summary>
        /// Gets the handling status composition strategy.
        /// </summary>
        /// <value>The handling status composition strategy.</value>
        public HandlingStatusCompositionStrategy HandlingStatusCompositionStrategy { get; private set; }

        /// <summary>
        /// Gets the type version match strategy.
        /// </summary>
        /// <value>The type version match strategy.</value>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
