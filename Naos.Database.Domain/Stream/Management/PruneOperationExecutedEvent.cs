// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PruneOperationExecutedEvent.cs" company="Naos Project">
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
    public partial class PruneOperationExecutedEvent : EventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PruneOperationExecutedEvent"/> class.
        /// </summary>
        /// <param name="pruneOperation">The prune operation.</param>
        /// <param name="pruneSummary">The prune summary.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        public PruneOperationExecutedEvent(
            IPruneOp pruneOperation,
            PruneSummary pruneSummary,
            DateTime timestampUtc)
            : base(timestampUtc)
        {
            pruneOperation.MustForArg(nameof(pruneOperation)).NotBeNull();
            pruneSummary.MustForArg(nameof(pruneSummary)).NotBeNull();

            this.PruneOperation = pruneOperation;
            this.PruneSummary = pruneSummary;
        }

        /// <summary>
        /// Gets the prune operation.
        /// </summary>
        /// <value>The prune operation.</value>
        public IPruneOp PruneOperation { get; private set; }

        /// <summary>
        /// Gets the prune summary.
        /// </summary>
        /// <value>The prune summary.</value>
        public PruneSummary PruneSummary { get; private set; }
    }
}
