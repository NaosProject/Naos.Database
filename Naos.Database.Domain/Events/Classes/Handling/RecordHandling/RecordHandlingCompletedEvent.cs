// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordHandlingCompletedEvent.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;

    /// <summary>
    /// The record was successfully handled for a specified concern.
    /// </summary>
    public partial class RecordHandlingCompletedEvent : RecordHandlingEventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordHandlingCompletedEvent"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal record identifier of the record being handled.</param>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="details">Details about the event.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        public RecordHandlingCompletedEvent(
            long internalRecordId,
            string concern,
            DateTime timestampUtc,
            string details = null)
            : base(internalRecordId, concern, timestampUtc, details)
        {
        }
    }
}
