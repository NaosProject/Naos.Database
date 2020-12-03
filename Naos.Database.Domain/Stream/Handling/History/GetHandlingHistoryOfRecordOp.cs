// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetHandlingHistoryOfRecordOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using Naos.Protocol.Domain;

    /// <summary>
    /// Handles a record.
    /// </summary>
    public partial class GetHandlingHistoryOfRecordOp : ReturningOperationBase<IReadOnlyList<StreamRecordHandlingEntry>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetHandlingHistoryOfRecordOp"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal records identifier.</param>
        public GetHandlingHistoryOfRecordOp(
            long internalRecordId)
        {
            this.InternalRecordId = internalRecordId;
        }

        /// <summary>
        /// Gets the concern.
        /// </summary>
        /// <value>The concern.</value>
        public long InternalRecordId { get; private set; }
    }
}
