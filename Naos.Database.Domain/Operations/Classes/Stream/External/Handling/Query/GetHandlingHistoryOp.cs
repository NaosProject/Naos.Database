// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetHandlingHistoryOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the handling history of a record.
    /// </summary>
    /// <remarks>
    /// If there has been any handling history at all (i.e. the record has been try-handled at least one time),
    /// then it is returned (including an entry for the <see cref="HandlingStatus.AvailableByDefault"/> status).
    /// Otherwise, an empty set of entries is returned (including when an invalid record id is specified).
    /// </remarks>
    public partial class GetHandlingHistoryOp : ReturningOperationBase<IReadOnlyList<StreamRecordHandlingEntry>>, IHaveHandleRecordConcern, IHaveInternalRecordId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetHandlingHistoryOp"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal record identifier.</param>
        /// <param name="concern">The record handling concern.</param>
        public GetHandlingHistoryOp(
            long internalRecordId,
            string concern)
        {
            concern.MustForArg(nameof(concern)).NotBeNullNorWhiteSpace();
            this.InternalRecordId = internalRecordId;
            this.Concern = concern;
        }

        /// <inheritdoc />
        public long InternalRecordId { get; private set; }

        /// <inheritdoc />
        public string Concern { get; private set; }
    }
}
