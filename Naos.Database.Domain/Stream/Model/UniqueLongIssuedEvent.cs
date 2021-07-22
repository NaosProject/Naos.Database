// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UniqueLongIssuedEvent.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Event to record the execution of <see cref="GetNextUniqueLongOp"/>.
    /// </summary>
    public partial class UniqueLongIssuedEvent : EventBase<long>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueLongIssuedEvent"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        /// <param name="details">The details.</param>
        public UniqueLongIssuedEvent(
            long id,
            DateTime timestampUtc,
            string details = null)
            : base(id, timestampUtc)
        {
            this.Details = details;
        }

        /// <summary>
        /// Gets the details about why it's blocked.
        /// </summary>
        /// <value>The details about why it's blocked.</value>
        public string Details { get; private set; }
    }
}
