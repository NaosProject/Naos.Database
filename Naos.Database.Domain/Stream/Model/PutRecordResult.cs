// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutRecordResult.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Result of <see cref="PutRecordOp"/>.
    /// </summary>
    public partial class PutRecordResult : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutRecordResult"/> class.
        /// </summary>
        /// <param name="internalRecordIdOfPutRecord">The internal record identifier of the new record.</param>
        /// <param name="internalRecordIdOfExistingRecord">The internal record identifier of any existing record.</param>
        public PutRecordResult(
            long? internalRecordIdOfPutRecord,
            long? internalRecordIdOfExistingRecord)
        {
            if (internalRecordIdOfPutRecord == null && internalRecordIdOfExistingRecord == null)
            {
                throw new ArgumentNullException(FormattableString.Invariant($"Cannot have both {nameof(internalRecordIdOfPutRecord)} AND {nameof(internalRecordIdOfExistingRecord)} both be null; the expectation is that the record was written or there was an existing record."));
            }

            this.InternalRecordIdOfPutRecord = internalRecordIdOfPutRecord;
            this.InternalRecordIdOfExistingRecord = internalRecordIdOfExistingRecord;
        }

        /// <summary>
        /// Gets the internal record identifier of new record.
        /// </summary>
        /// <value>The internal record identifier of put record.</value>
        public long? InternalRecordIdOfPutRecord { get; private set; }

        /// <summary>
        /// Gets the internal record identifier of existing record.
        /// </summary>
        /// <value>The internal record identifier of existing record.</value>
        public long? InternalRecordIdOfExistingRecord { get; private set; }
    }
}
