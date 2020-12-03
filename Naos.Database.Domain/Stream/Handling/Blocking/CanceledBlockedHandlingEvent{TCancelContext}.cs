// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CanceledBlockedHandlingEvent{TCancelContext}.cs" company="Naos Project">
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
    /// Event to indicate that no records should be handled by the stream.
    /// </summary>
    /// <typeparam name="TCancelContext">The type of the cancel context.</typeparam>
    public partial class CanceledBlockedHandlingEvent<TCancelContext> : EventBaseBase, IHaveTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CanceledBlockedHandlingEvent{TId}"/> class.
        /// </summary>
        /// <param name="cancelContext">The cancel context.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        /// <param name="details">The details.</param>
        /// <param name="tags">The optional tags.</param>
        public CanceledBlockedHandlingEvent(
            TCancelContext cancelContext,
            DateTime timestampUtc,
            string details,
            IReadOnlyDictionary<string, string> tags = null)
            : base(timestampUtc)
        {
            cancelContext.MustForArg(nameof(cancelContext)).NotBeNull();
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
            this.CancelContext = cancelContext;
            this.Details = details;
            this.Tags = tags;
        }

        /// <summary>
        /// Gets the cancel context.
        /// </summary>
        /// <value>The cancel context.</value>
        public TCancelContext CancelContext { get; private set; }

        /// <summary>
        /// Gets the details.
        /// </summary>
        /// <value>The details.</value>
        public string Details { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Tags { get; private set; }
    }
}
