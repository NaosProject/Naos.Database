// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnableRecordHandlingForStreamOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Cancels a block placed on an <see cref="IRecordHandlingOnlyStream"/> when
    /// <see cref="DisableRecordHandlingForStreamOp"/> is executed, so that records can continue to be handled.
    /// should not be processed until a <see cref="EnableRecordHandlingForStreamOp"/> is executed.
    /// </summary>
    public partial class EnableRecordHandlingForStreamOp : VoidOperationBase, IHaveDetails, IHaveTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnableRecordHandlingForStreamOp"/> class.
        /// </summary>
        /// <param name="details">The details about the cancellation.</param>
        /// <param name="tags">OPTIONAL tags to write to the resulting event(s).  DEFAULT is no tags.</param>
        public EnableRecordHandlingForStreamOp(
            string details,
            IReadOnlyCollection<NamedValue<string>> tags = null)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
            this.Details = details;
            this.Tags = tags;
        }

        /// <inheritdoc />
        public string Details { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }
    }
}