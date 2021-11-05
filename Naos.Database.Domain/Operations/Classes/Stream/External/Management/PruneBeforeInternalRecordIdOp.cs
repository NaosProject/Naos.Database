// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PruneBeforeInternalRecordIdOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Deletes all records in a <see cref="IManagementOnlyStream"/> whose internal record identifier is less than the specified threshold.
    /// </summary>
    public partial class PruneBeforeInternalRecordIdOp : VoidOperationBase, IHaveInternalRecordId, IPruneOp
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PruneBeforeInternalRecordIdOp"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal record identifier; all records older will be pruned.</param>
        /// <param name="details">Details related to the operation.</param>
        public PruneBeforeInternalRecordIdOp(
            long internalRecordId,
            string details)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();

            this.InternalRecordId = internalRecordId;
            this.Details = details;
        }

        /// <inheritdoc />
        public long InternalRecordId { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
