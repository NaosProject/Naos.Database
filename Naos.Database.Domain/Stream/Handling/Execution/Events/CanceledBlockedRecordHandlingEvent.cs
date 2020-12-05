// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CanceledBlockedRecordHandlingEvent.cs" company="Naos Project">
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
    /// Event container to signal a block to a <see cref="IReadWriteStream"/> as a <see cref="BlockedRecordHandlingEvent"/> should be ignored.
    /// </summary>
    public partial class CanceledBlockedRecordHandlingEvent : EventBaseBase, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CanceledBlockedRecordHandlingEvent"/> class.
        /// </summary>
        /// <param name="details">The details about the cancellation.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        public CanceledBlockedRecordHandlingEvent(
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