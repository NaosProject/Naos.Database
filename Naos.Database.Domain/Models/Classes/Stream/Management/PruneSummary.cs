// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PruneSummary.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// A summary of an executed <see cref="IPruneOp"/>.
    /// </summary>
    public partial class PruneSummary : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PruneSummary"/> class.
        /// </summary>
        /// <param name="internalRecordIdsRemoved">The internal record identifiers that were removed.</param>
        public PruneSummary(
            IReadOnlyList<long> internalRecordIdsRemoved)
        {
            internalRecordIdsRemoved.MustForArg(nameof(internalRecordIdsRemoved)).NotBeNull();

            this.InternalRecordIdsRemoved = internalRecordIdsRemoved;
        }

        /// <summary>
        /// Gets the internal record identifiers that were removed.
        /// </summary>
        public IReadOnlyList<long> InternalRecordIdsRemoved { get; private set; }
    }
}
