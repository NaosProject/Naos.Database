// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdDeprecatedEvent.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// Event to indicate in a stream that an identifier is no longer expected to be used.  This can be useful in marking the event and also to filter out identifiers which have been removed.
    /// </summary>
    public partial class IdDeprecatedEvent : EventBase, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdDeprecatedEvent"/> class.
        /// </summary>
        /// <param name="timestampUtc">The timestamp UTC.</param>
        /// <param name="details">The details.</param>
        public IdDeprecatedEvent(
            DateTime timestampUtc,
            string details = null)
            : base(timestampUtc)
        {
            this.Details = details;
        }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
