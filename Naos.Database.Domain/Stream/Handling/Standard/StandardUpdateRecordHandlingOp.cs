// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardUpdateRecordHandlingOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;

    using OBeautifulCode.Type;

    /// <summary>
    /// Operation to update the <see cref="HandlingStatus"/> of a given record for a given concern.
    /// </summary>
    public partial class StandardUpdateRecordHandlingOp : VoidOperationBase, IHaveDetails, IHaveTags, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardUpdateRecordHandlingOp"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal record identifier of the record to update the <see cref="HandlingStatus"/> on.</param>
        /// <param name="concern">The concern.</param>
        /// <param name="newStatus">The new <see cref="HandlingStatus"/> the record should have.</param>
        /// <param name="acceptableCurrentStatuses">The acceptable <see cref="HandlingStatus"/>'s that the record should have.</param>
        /// <param name="details">The optional details to write with produced events.</param>
        /// <param name="tags">The optional tags to write with produced events.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        public StandardUpdateRecordHandlingOp(
            long internalRecordId,
            string concern,
            HandlingStatus newStatus,
            IReadOnlyCollection<HandlingStatus> acceptableCurrentStatuses,
            string details = null,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            IResourceLocator specifiedResourceLocator = null)
        {
            concern.ThrowIfInvalidOrReservedConcern();

            this.InternalRecordId = internalRecordId;
            this.Concern = concern;
            this.NewStatus = newStatus;
            this.AcceptableCurrentStatuses = acceptableCurrentStatuses;
            this.Details = details;
            this.Tags = tags;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <summary>
        /// Gets the internal record identifier of the record to update the <see cref="HandlingStatus"/> on.
        /// </summary>
        /// <value>The internal record identifier of the record to update the <see cref="HandlingStatus"/> on.</value>
        public long InternalRecordId { get; private set; }

        /// <summary>
        /// Gets the concern if applicable.
        /// </summary>
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the new status of the record.
        /// </summary>
        public HandlingStatus NewStatus { get; private set; }

        /// <summary>
        /// Gets the acceptable current statuses.
        /// </summary>
        public IReadOnlyCollection<HandlingStatus> AcceptableCurrentStatuses { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
