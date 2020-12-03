// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockedHandlingEvent{TBlockingContext}.cs" company="Naos Project">
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
    /// Event container to signal a block to a <see cref="IReadWriteStream"/> indicating the stream should not be processed beyond this event without an associated <see cref="CanceledBlockedHandlingEvent{TId}"/>.
    /// </summary>
    /// <typeparam name="TBlockingContext">Type of the blocking context.</typeparam>
    public partial class BlockedHandlingEvent<TBlockingContext> : EventBaseBase, IHaveTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockedHandlingEvent{TBlockingContext}"/> class.
        /// </summary>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        /// <param name="blockingContext">The details.</param>
        /// <param name="tags">The optional tags.</param>
        public BlockedHandlingEvent(
            TBlockingContext blockingContext,
            DateTime timestampUtc,
            IReadOnlyDictionary<string, string> tags = null)
            : base(timestampUtc)
        {
            blockingContext.MustForArg(nameof(blockingContext)).NotBeNullNorWhiteSpace();
            this.BlockingContext = blockingContext;
            this.Tags = tags;
        }

        /// <summary>
        /// Gets the blocking context.
        /// </summary>
        /// <value>The blocking context.</value>
        public TBlockingContext BlockingContext { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Tags { get; private set; }
    }
}
