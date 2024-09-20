// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PruneAfterInternalRecordDateOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Deletes all records in a <see cref="IManagementOnlyStream"/> whose internal record date is less than the specified threshold.
    /// </summary>
    public partial class PruneAfterInternalRecordDateOp : VoidOperationBase, IPruneOp
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PruneAfterInternalRecordDateOp"/> class.
        /// </summary>
        /// <param name="recordTimestampUtc">The internal record date; all records older will be pruned.</param>
        /// <param name="details">Details related to the operation.</param>
        public PruneAfterInternalRecordDateOp(
            DateTime recordTimestampUtc,
            string details)
        {
            recordTimestampUtc.MustForArg(nameof(recordTimestampUtc)).BeUtcDateTime();
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();

            this.RecordTimestampUtc = recordTimestampUtc;
            this.Details = details;
        }

        /// <summary>
        /// Gets the internal record date to use, all records older will be pruned.
        /// </summary>
        public DateTime RecordTimestampUtc { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
