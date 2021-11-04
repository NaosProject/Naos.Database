// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PruneBeforeInternalRecordIdOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Deletes all records in a stream whose internal record identifier is less than the specified threshold.
    /// </summary>
    public partial class PruneBeforeInternalRecordIdOp : VoidOperationBase, IPruneOp, IHaveInternalRecordId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PruneBeforeInternalRecordIdOp"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal record identifier to use, all records older will be pruned.</param>
        /// <param name="details">The pruning context.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        public PruneBeforeInternalRecordIdOp(
            long internalRecordId,
            string details,
            IResourceLocator specifiedResourceLocator = null)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();

            this.InternalRecordId = internalRecordId;
            this.Details = details;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <inheritdoc />
        public long InternalRecordId { get; private set; }

        /// <summary>
        /// Gets the details.
        /// </summary>
        /// <value>The details.</value>
        public string Details { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
