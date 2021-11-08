﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResetFailedHandleRecordOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Resets a record that was handled for a specified concern, but whose execution resulted in an error.
    /// This makes the record available for handling for that concern.
    /// </summary>
    public partial class ResetFailedHandleRecordOp : VoidOperationBase, IHaveInternalRecordId, IHaveTags, IHaveHandleRecordConcern, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResetFailedHandleRecordOp"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal record identifier.</param>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="details">Details to write to the resulting <see cref="IHandlingEvent"/>.</param>
        /// <param name="tags">OPTIONAL tags to write to the resulting <see cref="IHandlingEvent"/>.  DEFAULT is no tags.</param>
        /// <param name="inheritRecordTags">OPTIONAL value indicating whether the resulting <see cref="IHandlingEvent"/> should inherit tags from the record being handled.  DEFAULT is to not inherit tags.</param>
        public ResetFailedHandleRecordOp(
            long internalRecordId,
            string concern,
            string details,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            bool inheritRecordTags = false)
        {
            concern.ThrowIfInvalidOrReservedConcern();
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
            tags.MustForArg(nameof(tags)).NotContainAnyNullElementsWhenNotNull();

            this.InternalRecordId = internalRecordId;
            this.Concern = concern;
            this.Details = details;
            this.Tags = tags;
            this.InheritRecordTags = inheritRecordTags;
        }

        /// <inheritdoc />
        public long InternalRecordId { get; private set; }

        /// <inheritdoc />
        public string Concern { get; private set; }

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