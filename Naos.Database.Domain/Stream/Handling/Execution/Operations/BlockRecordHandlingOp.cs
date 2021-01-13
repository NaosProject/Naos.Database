// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlockRecordHandlingOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Event container to signal a block to a <see cref="IReadWriteStream"/> indicating the stream should not be processed beyond this event without an associated <see cref="CanceledBlockedRecordHandlingEvent"/>.
    /// </summary>
    public partial class BlockRecordHandlingOp : VoidOperationBase, IHaveDetails, IHaveTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockRecordHandlingOp"/> class.
        /// </summary>
        /// <param name="details">The details about the block.</param>
        /// <param name="tags">The optional tags to write to produced events.</param>
        public BlockRecordHandlingOp(
            string details,
            IReadOnlyDictionary<string, string> tags = null)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
            this.Details = details;
            this.Tags = tags;
        }

        /// <inheritdoc />
        public string Details { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Tags { get; private set; }
    }
}
