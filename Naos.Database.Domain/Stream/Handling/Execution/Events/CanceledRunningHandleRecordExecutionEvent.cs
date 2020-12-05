// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CanceledRunningHandleRecordExecutionEvent.cs" company="Naos Project">
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
    /// Event indicating that the <see cref="HandleRecordOp"/> running execution was canceled externally.
    /// </summary>
    public partial class CanceledRunningHandleRecordExecutionEvent : EventBase<long>, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CanceledRunningHandleRecordExecutionEvent"/> class.
        /// </summary>
        /// <param name="id">The internal record identifier concerned with this handling sequence (the effective aggregate identifier of a record handling scenario).</param>
        /// <param name="details">The details about the cancellation.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        public CanceledRunningHandleRecordExecutionEvent(
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
