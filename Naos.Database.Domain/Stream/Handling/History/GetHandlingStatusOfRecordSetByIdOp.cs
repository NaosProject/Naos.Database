// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetHandlingStatusOfRecordSetByIdOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Handles a record.
    /// </summary>
    public partial class GetHandlingStatusOfRecordSetByIdOp : ReturningOperationBase<HandlingStatus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetHandlingStatusOfRecordSetByIdOp"/> class.
        /// </summary>
        /// <param name="internalRecordIdsToMatch">The internal record ids to treat as a composite status.</param>
        public GetHandlingStatusOfRecordSetByIdOp(
            IReadOnlyCollection<long> internalRecordIdsToMatch)
        {
            internalRecordIdsToMatch.MustForArg(nameof(internalRecordIdsToMatch)).NotBeNull();

            this.InternalRecordIdsToMatch = internalRecordIdsToMatch;
        }

        /// <summary>
        /// Gets the internal record identifiers to match.
        /// </summary>
        /// <value>The internal record identifiers to match.</value>
        public IReadOnlyCollection<long> InternalRecordIdsToMatch { get; private set; }
    }
}
