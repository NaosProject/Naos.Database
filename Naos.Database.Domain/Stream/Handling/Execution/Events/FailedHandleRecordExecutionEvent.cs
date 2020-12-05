// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FailedHandleRecordExecutionEvent.cs" company="Naos Project">
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
    /// Event indicating that an operation failed to execute.
    /// </summary>
    public partial class FailedHandleRecordExecutionEvent : EventBase<long>, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FailedHandleRecordExecutionEvent"/> class.
        /// </summary>
        /// <param name="id">The internal record identifier concerned with this handling sequence (the effective aggregate identifier of a record handling scenario).</param>
        /// <param name="details">The details of the failure.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        public FailedHandleRecordExecutionEvent(
            long id,
            string details,
            DateTime timestampUtc)
        : base(id, timestampUtc)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
            this.Details = details;
        }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
