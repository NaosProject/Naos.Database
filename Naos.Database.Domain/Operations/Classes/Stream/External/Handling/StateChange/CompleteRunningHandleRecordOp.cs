// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompleteRunningHandleRecordOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Marks a record that was handled for a specified concern as having executed without an error; handling has completed.
    /// </summary>
    public partial class CompleteRunningHandleRecordOp : VoidOperationBase, IHaveInternalRecordId, IHaveTags, IHaveDetails, IHaveHandleRecordConcern
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteRunningHandleRecordOp"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal record identifier.</param>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="details">OPTIONAL details to write to the resulting <see cref="IHandlingEvent"/>.  DEFAULT is no details.</param>
        /// <param name="tags">OPTIONAL tags to write to the resulting <see cref="IHandlingEvent"/>.  DEFAULT is no tags.</param>
        /// <param name="inheritRecordTags">OPTIONAL value indicating whether the resulting <see cref="IHandlingEvent"/> should inherit tags from the record being handled.  DEFAULT is to not inherit tags.</param>
        public CompleteRunningHandleRecordOp(
            long internalRecordId,
            string concern,
            string details = null,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            bool inheritRecordTags = false)
        {
            concern.ThrowIfInvalidOrReservedConcern();
            tags.MustForArg(nameof(tags)).NotContainAnyNullElementsWhenNotNull();

            this.InternalRecordId = internalRecordId;
            this.Concern = concern;
            this.Tags = tags;
            this.Details = details;
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
