// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockedHandlingEvent.cs" company="Naos Project">
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
    /// Event container to signal a block to a <see cref="IReadWriteStream"/> indicating the stream should not be processed beyond this event without an associated <see cref="CanceledBlockedHandlingEvent"/>.
    /// </summary>
    public partial class BlockedHandlingEvent : EventBaseBase, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockedHandlingEvent"/> class.
        /// </summary>
        /// <param name="details">The details about the block.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        public BlockedHandlingEvent(
            string details,
            DateTime timestampUtc)
            : base(timestampUtc)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
            this.Details = details;
        }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
