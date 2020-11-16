// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UniqueLongIssuedEvent.cs" company="Naos Project">
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
    /// Event to record the execution of <see cref="GetNextUniqueLongOp"/>.
    /// </summary>
    public partial class UniqueLongIssuedEvent : EventBase<long>, IHaveTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueLongIssuedEvent"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        /// <param name="details">The details.</param>
        /// <param name="tags">The optional tags.</param>
        public UniqueLongIssuedEvent(
            long id,
            DateTime timestampUtc,
            string details = null,
            IReadOnlyDictionary<string, string> tags = null)
            : base(id, timestampUtc)
        {
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
