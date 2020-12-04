// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetHandlingStatusOfRecordsByIdOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Gets the composite status of the set of records matched by specified tag matching on locators found by identifiers.
    /// </summary>
    public partial class GetHandlingStatusOfRecordsByIdOp : ReturningOperationBase<HandlingStatus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetHandlingStatusOfRecordsByIdOp"/> class.
        /// </summary>
        /// <param name="locatedIdsToMatch">The object identifiers to treat as a composite status.</param>
        /// <param name="typeVersionMatchStrategy">The optional type version match strategy; DEFAULT is Any.</param>
        public GetHandlingStatusOfRecordsByIdOp(
            IReadOnlyCollection<LocatedStringSerializedIdentifier> locatedIdsToMatch,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any)
        {
            locatedIdsToMatch.MustForArg(nameof(locatedIdsToMatch)).NotBeNull();

            this.LocatedIdsToMatch = locatedIdsToMatch;
            this.TypeVersionMatchStrategy = typeVersionMatchStrategy;
        }

        /// <summary>
        /// Gets the object identifiers to match.
        /// </summary>
        /// <value>The object identifiers to match.</value>
        public IReadOnlyCollection<LocatedStringSerializedIdentifier> LocatedIdsToMatch { get; private set; }

        /// <summary>
        /// Gets the type version match strategy.
        /// </summary>
        /// <value>The type version match strategy.</value>
        public TypeVersionMatchStrategy TypeVersionMatchStrategy { get; private set; }
    }
}
