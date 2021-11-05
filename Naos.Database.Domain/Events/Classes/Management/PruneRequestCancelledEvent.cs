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
    /// The request to prune an <see cref="IManagementOnlyStream"/> was cancelled.
    /// </summary>
    public partial class PruneRequestCancelledEvent : EventBase, IHaveDetails
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

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
