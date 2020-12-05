// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockRecordHandlingOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Event container to signal a block to a <see cref="IReadWriteStream"/> indicating the stream should not be processed beyond this event without an associated <see cref="CanceledBlockedRecordHandlingEvent"/>.
    /// </summary>
    public partial class BlockRecordHandlingOp : VoidOperationBase, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockRecordHandlingOp"/> class.
        /// </summary>
        /// <param name="details">The details about the block.</param>
        public BlockRecordHandlingOp(
            string details)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
            this.Details = details;
        }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
