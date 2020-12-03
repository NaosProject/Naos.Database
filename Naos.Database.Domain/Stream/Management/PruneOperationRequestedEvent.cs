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
    public partial class PruneOperationRequestedEvent : EventBaseBase, IHaveTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PruneOperationRequestedEvent"/> class.
        /// </summary>
        /// <param name="pruneOperation">The prune operation.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        /// <param name="tags">The optional tags.</param>
        public PruneOperationRequestedEvent(
            IPruneOperation pruneOperation,
            DateTime timestampUtc,
            IReadOnlyDictionary<string, string> tags = null)
            : base(timestampUtc)
        {
            this.PruneOperation = pruneOperation;
            this.Tags = tags;
        }

        /// <summary>
        /// Gets the prune operation.
        /// </summary>
        /// <value>The prune operation.</value>
        public IPruneOperation PruneOperation { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Tags { get; private set; }
    }
}
