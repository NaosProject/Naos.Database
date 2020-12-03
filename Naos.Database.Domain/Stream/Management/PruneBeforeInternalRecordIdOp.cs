// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PruneBeforeInternalRecordIdOp.cs" company="Naos Project">
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
    /// Deletes all records in a stream whose internal record identifier is less than the specified threshold.
    /// </summary>
    public partial class PruneBeforeInternalRecordIdOp : VoidOperationBase, IHaveTags, IPruneOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PruneBeforeInternalRecordIdOp"/> class.
        /// </summary>
        /// <param name="maxInternalRecordId">The maximum internal record identifier.</param>
        /// <param name="details">The pruning context.</param>
        /// <param name="tags">The optional tags.</param>
        public PruneBeforeInternalRecordIdOp(
            long maxInternalRecordId,
            string details,
            IReadOnlyDictionary<string, string> tags = null)
        {
            maxInternalRecordId.MustForArg(nameof(maxInternalRecordId)).NotBeLessThan(0L);
            details.MustForArg(nameof(details)).NotBeNull();

            this.MaxInternalRecordId = maxInternalRecordId;
            this.Details = details;
            this.Tags = tags;
        }

        /// <summary>
        /// Gets the maximum internal record identifier.
        /// </summary>
        /// <value>The maximum internal record identifier.</value>
        public long MaxInternalRecordId { get; private set; }

        /// <summary>
        /// Gets the details.
        /// </summary>
        /// <value>The details.</value>
        public string Details { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Tags { get; private set; }
    }
}
