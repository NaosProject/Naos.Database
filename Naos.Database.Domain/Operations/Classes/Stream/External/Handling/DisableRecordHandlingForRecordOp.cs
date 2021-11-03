// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisableRecordHandlingForRecordOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Cancels a request to handle a record.   , no one should handle this record for the given concern.
    /// this cancels and prevents
    /// </summary>
    public partial class DisableRecordHandlingForRecordOp : VoidOperationBase, IHaveTags, IHaveDetails, IHaveInternalRecordId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisableRecordHandlingForRecordOp"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal record identifier of the record that is the subject of handling.</param>
        /// <param name="details">Details to write to the resulting event(s).</param>
        /// <param name="tags">OPTIONAL tags to write to the resulting event(s).  DEFAULT is no tags.</param>
        public DisableRecordHandlingForRecordOp(
            long internalRecordId,
            string details,
            IReadOnlyCollection<NamedValue<string>> tags = null)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();

            this.InternalRecordId = internalRecordId;
            this.Details = details;
            this.Tags = tags;
        }

        /// <inheritdoc />
        public long InternalRecordId { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }
    }
}
