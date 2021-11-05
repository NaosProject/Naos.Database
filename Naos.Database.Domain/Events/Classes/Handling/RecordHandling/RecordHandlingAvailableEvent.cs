// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordHandlingAvailableEvent.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    using static System.FormattableString;

    /// <summary>
    /// The record was available to be handled for a specified concern.
    /// </summary>
    public partial class RecordHandlingAvailableEvent : RecordHandlingEventBase, IForsakeDeepCloneWithVariantsViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordHandlingAvailableEvent"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal record identifier of the record being handled.</param>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="recordToHandle">The record to handle.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        /// <param name="details">Details about the event.</param>
        public RecordHandlingAvailableEvent(
            long internalRecordId,
            string concern,
            StreamRecord recordToHandle,
            DateTime timestampUtc,
            string details = null)
            : base(internalRecordId, concern, timestampUtc, details)
        {
            this.RecordToHandle = recordToHandle ?? throw new ArgumentNullException(nameof(recordToHandle));

            internalRecordId.MustForArg(nameof(internalRecordId)).BeEqualTo(recordToHandle.InternalRecordId, Invariant($"{nameof(internalRecordId)} and {nameof(recordToHandle)}.{nameof(StreamRecord.InternalRecordId)} must match."));
        }

        /// <summary>
        /// Gets the record to handle.
        /// </summary>
        public StreamRecord RecordToHandle { get; private set; }
    }
}
