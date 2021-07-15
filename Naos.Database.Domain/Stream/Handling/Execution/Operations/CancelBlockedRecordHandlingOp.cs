// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CancelBlockedRecordHandlingOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Event container to signal a block to a <see cref="IReadWriteStream"/> as a <see cref="BlockedRecordHandlingEvent"/> should be ignored.
    /// </summary>
    public partial class CancelBlockedRecordHandlingOp : VoidOperationBase, IHaveDetails, IHaveTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelBlockedRecordHandlingOp"/> class.
        /// </summary>
        /// <param name="details">The details about the cancellation.</param>
        /// <param name="tags">The optional tags to write to produced events.</param>
        public CancelBlockedRecordHandlingOp(
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