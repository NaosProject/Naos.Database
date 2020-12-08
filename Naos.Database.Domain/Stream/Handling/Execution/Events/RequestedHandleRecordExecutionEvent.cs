// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestedHandleRecordExecutionEvent.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Validation.Recipes;

    /// <summary>
    /// Event indicating that a record needs to be executed via <see cref="HandleRecordOp"/>.
    /// </summary>
    public partial class RequestedHandleRecordExecutionEvent : EventBase<long>, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestedHandleRecordExecutionEvent"/> class.
        /// </summary>
        /// <param name="id">The internal record identifier concerned with this handling sequence (the effective aggregate identifier of a record handling scenario).</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        /// <param name="recordToHandle">The record to handle.</param>
        /// <param name="details">Optional details about the request.</param>
        public RequestedHandleRecordExecutionEvent(
            long id,
            DateTime timestampUtc,
            StreamRecord recordToHandle,
            string details = null)
        : base(id, timestampUtc)
        {
            this.RecordToHandle = recordToHandle ?? throw new ArgumentNullException(nameof(recordToHandle));

            id.MustForArg(nameof(id)).BeEqualTo(recordToHandle.InternalRecordId, "The id of the event and the id of the internal record id of the record must match.");

            this.Details = details;
        }

        /// <summary>
        /// Gets the executed operation.
        /// </summary>
        /// <value>The executed operation.</value>
        public StreamRecord RecordToHandle { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
