// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockEvent{TId}.cs" company="Naos Project">
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
    /// Event container to signal a block to a <see cref="IReadWriteStream"/> indicating the stream should not be processed beyond this event without an associated <see cref="CancelBlockEvent{TId}"/>.
    /// </summary>
    /// <typeparam name="TId">Type of the identifier.</typeparam>
    public partial class BlockEvent<TId> : EventBase<TId>, IHaveTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockEvent{TId}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        /// <param name="details">The details.</param>
        /// <param name="tags">The optional tags.</param>
        public BlockEvent(
            TId id,
            DateTime timestampUtc,
            string details,
            IReadOnlyDictionary<string, string> tags = null)
            : base(id, timestampUtc)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
            this.Details = details;
            this.Tags = tags;
        }

        /// <summary>
        /// Gets the details about why it's blocked.
        /// </summary>
        /// <value>The details about why it's blocked.</value>
        public string Details { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Tags { get; private set; }
    }
}
