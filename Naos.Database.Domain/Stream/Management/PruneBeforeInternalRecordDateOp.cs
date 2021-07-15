// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PruneBeforeInternalRecordDateOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Deletes all records in a stream whose internal record date is less than the specified threshold.
    /// </summary>
    public partial class PruneBeforeInternalRecordDateOp : VoidOperationBase, IPruneOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PruneBeforeInternalRecordDateOp"/> class.
        /// </summary>
        /// <param name="internalRecordDate">The internal record date, all records older will be pruned.</param>
        /// <param name="details">The pruning context.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        public PruneBeforeInternalRecordDateOp(
            DateTime internalRecordDate,
            string details,
            IResourceLocator specifiedResourceLocator = null)
        {
            internalRecordDate.Kind.MustForArg(Invariant($"{nameof(internalRecordDate)}.{nameof(internalRecordDate.Kind)}")).BeEqualTo(DateTimeKind.Utc, "Timestamp must be UTC.");
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();

            this.InternalRecordDate = internalRecordDate;
            this.Details = details;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <summary>
        /// Gets the internal record date to use, all records older will be pruned.
        /// </summary>
        /// <value>The internal record date.</value>
        public DateTime InternalRecordDate { get; private set; }

        /// <summary>
        /// Gets the details.
        /// </summary>
        /// <value>The details.</value>
        public string Details { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
