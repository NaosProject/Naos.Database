// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandlingForRecordDisabledEvent.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Handling of the record was disabled for all concerns.
    /// </summary>
    public partial class HandlingForRecordDisabledEvent : RecordHandlingEventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlingForRecordDisabledEvent"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal record identifier of the record being handled.</param>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="details">Details about the event.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        public HandlingForRecordDisabledEvent(
            long internalRecordId,
            string concern,
            DateTime timestampUtc,
            string details)
            : base(internalRecordId, concern, timestampUtc, details)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
        }
    }
}
