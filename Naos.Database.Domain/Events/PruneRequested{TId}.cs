// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PruneRequested{TId}.cs" company="Naos Project">
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
    /// Event indicating a prune should be done on the stream (standard reads will not go prior to the requested checkpoint).
    /// </summary>
    /// <typeparam name="TId">Type of the identifier.</typeparam>
    public partial class PruneRequested<TId> : EventBase<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PruneRequested{TId}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        /// <param name="details">The details.</param>
        /// <param name="tags">The optional tags.</param>
        public PruneRequested(
            TId id,
            DateTime timestampUtc,
            string details,
            IReadOnlyDictionary<string, string> tags = null)
            : base(id, timestampUtc, tags)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
            this.Details = details;
        }

        /// <summary>
        /// Gets the details.
        /// </summary>
        /// <value>The details.</value>
        public string Details { get; private set; }
    }
}
