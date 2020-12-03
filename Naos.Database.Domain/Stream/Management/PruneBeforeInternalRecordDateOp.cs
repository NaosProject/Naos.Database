// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PruneBeforeInternalRecordDateOp.cs" company="Naos Project">
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
    using static System.FormattableString;

    /// <summary>
    /// Deletes all records in a stream whose internal record identifier is less than the specified threshold.
    /// </summary>
    public partial class PruneBeforeInternalRecordDateOp : VoidOperationBase, IHaveTags, IPruneOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PruneBeforeInternalRecordDateOp"/> class.
        /// </summary>
        /// <param name="maxInternalRecordDate">The maximum internal record date.</param>
        /// <param name="details">The pruning context.</param>
        /// <param name="tags">The optional tags.</param>
        public PruneBeforeInternalRecordDateOp(
            DateTime maxInternalRecordDate,
            string details,
            IReadOnlyDictionary<string, string> tags = null)
        {
            maxInternalRecordDate.Kind.MustForArg(Invariant($"{nameof(maxInternalRecordDate)}.{nameof(maxInternalRecordDate.Kind)}")).BeEqualTo(DateTimeKind.Utc);
            details.MustForArg(nameof(details)).NotBeNull();

            this.MaxInternalRecordDate = maxInternalRecordDate;
            this.Details = details;
            this.Tags = tags;
        }

        /// <summary>
        /// Gets the maximum internal record date.
        /// </summary>
        /// <value>The maximum internal record date.</value>
        public DateTime MaxInternalRecordDate { get; private set; }

        /// <summary>
        /// Gets the details.
        /// </summary>
        /// <value>The details.</value>
        public string Details { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Tags { get; private set; }
    }
}
