// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Block{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using Naos.Protocol.Domain;

    /// <summary>
    /// Event container to signal a block to a <see cref="IStream{TId}"/> indicating the stream should not be processed beyond this event without an associated <see cref="CancelBlock{TId}"/>.
    /// </summary>
    /// <typeparam name="TId">Type of the identifier.</typeparam>
    public partial class Block<TId> : EventBase<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Block{TId}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        /// <param name="details">The details.</param>
        /// <param name="tags">The optional tags.</param>
        public Block(
            TId id,
            DateTime timestampUtc,
            string details,
            IReadOnlyDictionary<string, string> tags = null)
            : base(id, timestampUtc, tags)
        {
            this.Details = details ?? throw new ArgumentNullException(nameof(details));
        }

        /// <summary>
        /// Gets the details about why it's blocked.
        /// </summary>
        /// <value>The details about why it's blocked.</value>
        public string Details { get; private set; }
    }
}
