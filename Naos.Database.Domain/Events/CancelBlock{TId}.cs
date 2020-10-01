// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CancelBlock{TId}.cs" company="Naos Project">
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
    /// Event to indicate a <see cref="Block{TId}" /> was cancelled (i.e. ignore a previous <see cref="Block{TId}" />).
    /// </summary>
    /// <typeparam name="TId">Type of the identifier.</typeparam>
    public partial class CancelBlock<TId> : EventBase<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelBlock{TId}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        /// <param name="details">The details.</param>
        /// <param name="tags">The optional tags.</param>
        public CancelBlock(
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
