// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordHandlingEventBase.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// The record had handling activity for a specified concern.
    /// </summary>
    public abstract partial class RecordHandlingEventBase : HandlingEventBase, IHaveInternalRecordId, IHaveHandleRecordConcern
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordHandlingEventBase"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal record identifier of the record being handled.</param>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="details">Details about the event.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        protected RecordHandlingEventBase(
            long internalRecordId,
            string concern,
            DateTime timestampUtc,
            string details)
            : base(timestampUtc, details)
        {
            concern.MustForArg(nameof(concern)).NotBeNullNorWhiteSpace();

            this.Concern = concern;
            this.InternalRecordId = internalRecordId;
        }

        /// <inheritdoc />
        public string Concern { get; private set; }

        /// <inheritdoc />
        public long InternalRecordId { get; private set; }
    }
}
