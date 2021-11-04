// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetHandlingStatusOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the <see cref="HandlingStatus"/> of a record.
    /// </summary>
    public partial class GetHandlingStatusOp : ReturningOperationBase<HandlingStatus>, IHaveHandleRecordConcern, IHaveInternalRecordId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetHandlingStatusOp"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal record identifier.</param>
        /// <param name="concern">The record handling concern.</param>
        public GetHandlingStatusOp(
            long internalRecordId,
            string concern)
        {
            concern.ThrowIfInvalidOrReservedConcern();
            this.InternalRecordId = internalRecordId;
            this.Concern = concern;
        }

        /// <inheritdoc />
        public long InternalRecordId { get; private set; }

        /// <inheritdoc />
        public string Concern { get; private set; }
    }
}
