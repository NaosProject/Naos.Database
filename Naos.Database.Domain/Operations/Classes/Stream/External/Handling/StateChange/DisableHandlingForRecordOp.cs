// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisableHandlingForRecordOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Disables a record from being handled for all concerns.
    /// </summary>
    public partial class DisableHandlingForRecordOp : VoidOperationBase, IHaveTags, IHaveDetails, IHaveInternalRecordId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisableHandlingForRecordOp"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal record identifier.</param>
        /// <param name="details">Details to write to the resulting <see cref="IHandlingEvent"/>.</param>
        /// <param name="tags">OPTIONAL tags to write to the resulting <see cref="IHandlingEvent"/>.  DEFAULT is no tags.</param>
        /// <param name="inheritRecordTags">OPTIONAL value indicating whether the resulting <see cref="IHandlingEvent"/> should inherit tags from the record being handled.  DEFAULT is to not inherit tags.</param>
        public DisableHandlingForRecordOp(
            long internalRecordId,
            string details,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            bool inheritRecordTags = false)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
            tags.MustForArg(nameof(tags)).NotContainAnyNullElementsWhenNotNull();

            this.InternalRecordId = internalRecordId;
            this.Details = details;
            this.Tags = tags;
            this.InheritRecordTags = inheritRecordTags;
        }

        /// <inheritdoc />
        public long InternalRecordId { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the resulting <see cref="IHandlingEvent"/> should inherit tags from the record being handled.
        /// </summary>
        public bool InheritRecordTags { get; private set; }
    }
}
