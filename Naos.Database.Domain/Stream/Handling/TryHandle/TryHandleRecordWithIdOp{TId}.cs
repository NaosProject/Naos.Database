// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryHandleRecordWithIdOp{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;

    /// <summary>
    /// Handles a record.
    /// </summary>
    /// <typeparam name="TId">Type of the identifier of the record.</typeparam>
    public partial class TryHandleRecordWithIdOp<TId> : ReturningOperationBase<StreamRecordWithId<TId>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TryHandleRecordWithIdOp{TId}"/> class.
        /// </summary>
        /// <param name="recordToHandle">The record to handle.</param>
        public TryHandleRecordWithIdOp(
            StreamRecordWithId<TId> recordToHandle)
        {
            this.RecordToHandle = recordToHandle;
        }

        /// <summary>
        /// Gets the record to handle.
        /// </summary>
        /// <value>The record to handle.</value>
        public StreamRecordWithId<TId> RecordToHandle { get; private set; }
    }
}
