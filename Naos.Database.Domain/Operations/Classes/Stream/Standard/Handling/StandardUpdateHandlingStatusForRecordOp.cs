// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardUpdateHandlingStatusForRecordOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Update the <see cref="HandlingStatus"/> of a record.
    /// </summary>
    /// <remarks>
    /// This is an internal operation; it is designed to honor the contract of an <see cref="IStandardStream"/>.
    /// While technically there are no limitations on who may execute this operation on such a stream,
    /// these are "bare metal" operations and can be misused without a deeper understanding of what will happen.
    /// Most typically, you will use the operations that are exposed via these extension methods
    /// <see cref="ReadOnlyStreamExtensions"/> and <see cref="WriteOnlyStreamExtensions"/>.
    /// </remarks>
    public partial class StandardUpdateHandlingStatusForRecordOp : VoidOperationBase, IHaveDetails, IHaveTags, ISpecifyResourceLocator, IHaveInternalRecordId, IHaveHandleRecordConcern
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardUpdateHandlingStatusForRecordOp"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal record identifier.</param>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="newStatus">The new <see cref="HandlingStatus"/> to apply to the record.</param>
        /// <param name="acceptableCurrentStatuses">The set of acceptable <see cref="HandlingStatus"/> for the record.  The record's status must be in this set for the update to be applied, otherwise the protocol will throw.</param>
        /// <param name="details">OPTIONAL details to write to the resulting <see cref="IHandlingEvent"/>.  DEFAULT is no details.</param>
        /// <param name="tags">OPTIONAL tags to write to the resulting <see cref="IHandlingEvent"/>.  DEFAULT is no tags.</param>
        /// <param name="inheritRecordTags">OPTIONAL value indicating whether the resulting <see cref="IHandlingEvent"/> should inherit tags from the record being handled.  DEFAULT is to not inherit tags.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        public StandardUpdateHandlingStatusForRecordOp(
            long internalRecordId,
            string concern,
            HandlingStatus newStatus,
            IReadOnlyCollection<HandlingStatus> acceptableCurrentStatuses,
            string details = null,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            bool inheritRecordTags = false,
            IResourceLocator specifiedResourceLocator = null)
        {
            concern.ThrowIfInvalidOrReservedConcern();
            newStatus.MustForArg(nameof(newStatus)).NotBeElementIn(new[] { HandlingStatus.Unknown, HandlingStatus.DisabledForStream });
            acceptableCurrentStatuses.MustForArg(nameof(acceptableCurrentStatuses)).NotBeNullNorEmptyEnumerable().And().Each().NotBeEqualTo(HandlingStatus.Unknown);
            tags.MustForArg(nameof(tags)).NotContainAnyNullElementsWhenNotNull();

            this.InternalRecordId = internalRecordId;
            this.Concern = concern;
            this.NewStatus = newStatus;
            this.AcceptableCurrentStatuses = acceptableCurrentStatuses;
            this.Details = details;
            this.Tags = tags;
            this.InheritRecordTags = inheritRecordTags;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <inheritdoc />
        public long InternalRecordId { get; private set; }

        /// <inheritdoc />
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the new <see cref="HandlingStatus"/> to apply to the record.
        /// </summary>
        public HandlingStatus NewStatus { get; private set; }

        /// <summary>
        /// Gets the set of acceptable <see cref="HandlingStatus"/>.
        /// The record's status must be in this set for the update to be applied, otherwise the protocol will throw.
        /// </summary>
        public IReadOnlyCollection<HandlingStatus> AcceptableCurrentStatuses { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the resulting <see cref="IHandlingEvent"/> should inherit tags from the record being handled.
        /// </summary>
        public bool InheritRecordTags { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
