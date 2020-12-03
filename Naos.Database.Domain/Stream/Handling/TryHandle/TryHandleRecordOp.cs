// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryHandleRecordOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;

    /// <summary>
    /// Handles a record.
    /// </summary>
    public partial class TryHandleRecordOp : ReturningOperationBase<StreamRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TryHandleRecordOp"/> class.
        /// </summary>
        /// <param name="recordToHandle">The record to handle.</param>
        public TryHandleRecordOp(
            StreamRecord recordToHandle)
        {
            this.RecordToHandle = recordToHandle;
        }

        /// <summary>
        /// Gets the record to handle.
        /// </summary>
        /// <value>The record to handle.</value>
        public StreamRecord RecordToHandle { get; private set; }
    }
}
