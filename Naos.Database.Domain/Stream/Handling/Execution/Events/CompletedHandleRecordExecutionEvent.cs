﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompletedHandleRecordExecutionEvent.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Type;


    /// <summary>
    /// Event indicating that the <see cref="HandleRecordOp"/> was executed without exception.
    /// </summary>
    public partial class CompletedHandleRecordExecutionEvent : EventBase<long>, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompletedHandleRecordExecutionEvent"/> class.
        /// </summary>
        /// <param name="id">The internal record identifier concerned with this handling sequence (the effective aggregate identifier of a record handling scenario).</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        /// <param name="details">The optional details about the completion.</param>
        public CompletedHandleRecordExecutionEvent(
            long id,
            DateTime timestampUtc,
            string details = null)
        : base(id, timestampUtc)
        {
            this.Details = details;
        }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
