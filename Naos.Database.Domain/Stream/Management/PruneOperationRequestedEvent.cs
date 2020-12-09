// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PruneOperationRequestedEvent.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Event indicating a prune should be done on the stream (standard reads will not go prior to the requested checkpoint).
    /// </summary>
    public partial class PruneOperationRequestedEvent : EventBaseBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PruneOperationRequestedEvent"/> class.
        /// </summary>
        /// <param name="pruneOperation">The prune operation.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        public PruneOperationRequestedEvent(
            IPruneOperation pruneOperation,
            DateTime timestampUtc)
            : base(timestampUtc)
        {
            pruneOperation.MustForArg(nameof(pruneOperation)).NotBeNull();

            this.PruneOperation = pruneOperation;
        }

        /// <summary>
        /// Gets the prune operation.
        /// </summary>
        /// <value>The prune operation.</value>
        public IPruneOperation PruneOperation { get; private set; }
    }
}
