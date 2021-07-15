// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PruneSummary.cs" company="Naos Project">
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
    /// Event indicating a prune should be done on the stream (standard reads will not go prior to the requested checkpoint).
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
        /// <value>The internal record identifiers that were removed.</value>
        public IReadOnlyList<long> InternalRecordIdsRemoved { get; private set; }
    }
}
