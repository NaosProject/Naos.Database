// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PruneRequestCancelledEvent.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Event indicating a prune should be done on the stream (standard reads will not go prior to the requested checkpoint).
    /// </summary>
    public partial class PruneRequestCancelledEvent : EventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PruneRequestCancelledEvent"/> class.
        /// </summary>
        /// <param name="details">The details.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        public PruneRequestCancelledEvent(
            string details,
            DateTime timestampUtc)
            : base(timestampUtc)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
            this.Details = details;
        }

        /// <summary>
        /// Gets the details.
        /// </summary>
        public string Details { get; private set; }
    }
}
