// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PruneOperationRequestedEvent.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// A request was made to prune an <see cref="IManagementOnlyStream"/>.
    /// </summary>
    /// <remarks>
    /// Standard reads will ignore records prior to the requested pruning checkpoint.
    /// </remarks>
    public partial class PruneOperationRequestedEvent : EventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PruneOperationRequestedEvent"/> class.
        /// </summary>
        /// <param name="pruneOperation">The prune operation.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        public PruneOperationRequestedEvent(
            IPruneOp pruneOperation,
            DateTime timestampUtc)
            : base(timestampUtc)
        {
            pruneOperation.MustForArg(nameof(pruneOperation)).NotBeNull();

            this.PruneOperation = pruneOperation;
        }

        /// <summary>
        /// Gets the prune operation.
        /// </summary>
        public IPruneOp PruneOperation { get; private set; }
    }
}
