// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CanceledPruneRequestedEvent.cs" company="Naos Project">
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
    public partial class CanceledPruneRequestedEvent : EventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CanceledPruneRequestedEvent"/> class.
        /// </summary>
        /// <param name="details">The details.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        public CanceledPruneRequestedEvent(
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
        /// <value>The details.</value>
        public string Details { get; private set; }
    }
}
