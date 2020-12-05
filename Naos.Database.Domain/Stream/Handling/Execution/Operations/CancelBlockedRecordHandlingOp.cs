// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CancelBlockedRecordHandlingOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Event container to signal a block to a <see cref="IReadWriteStream"/> as a <see cref="BlockedRecordHandlingEvent"/> should be ignored.
    /// </summary>
    public partial class CancelBlockedRecordHandlingOp : VoidOperationBase, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelBlockedRecordHandlingOp"/> class.
        /// </summary>
        /// <param name="details">The details about the cancellation.</param>
        public CancelBlockedRecordHandlingOp(
            string details)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
            this.Details = details;
        }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}