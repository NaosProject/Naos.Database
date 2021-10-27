// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardPruneStreamOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    using static System.FormattableString;

    /// <summary>
    /// Prune a stream.
    /// </summary>
    /// <remarks>
    /// This is an internal operation; it is designed to honor the contract of an <see cref="IStandardReadWriteStream"/>.
    /// While technically there are no limitations on who may execute this operation on such a stream,
    /// these are "bare metal" operations and can be misused without a deeper understanding of what will happen.
    /// Most typically, you will use the operations that are exposed via these extension methods
    /// <see cref="ReadOnlyStreamExtensions"/> and <see cref="WriteOnlyStreamExtensions"/>.
    /// </remarks>
    public partial class StandardPruneStreamOp : VoidOperationBase, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardPruneStreamOp"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal record identifier to use, all records older will be pruned.</param>
        /// <param name="internalRecordDate">The internal record date, all records older will be pruned.</param>
        /// <param name="details">The pruning context.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        public StandardPruneStreamOp(
            long? internalRecordId,
            DateTime? internalRecordDate,
            string details,
            IResourceLocator specifiedResourceLocator)
        {
            if (internalRecordId == null && internalRecordDate == null)
            {
                throw new ArgumentException(Invariant($"Either '{nameof(internalRecordId)}' or '{nameof(internalRecordDate)}' must be specified."));
            }

            if (internalRecordDate != null)
            {
                internalRecordDate.Value.Kind.MustForArg(Invariant($"{nameof(internalRecordDate)}.{nameof(internalRecordDate.Value.Kind)}"))
                                  .BeEqualTo(DateTimeKind.Utc, "Timestamp must be UTC.");
            }

            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();

            this.InternalRecordId = internalRecordId;
            this.InternalRecordDate = internalRecordDate;
            this.Details = details;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <summary>
        /// Gets the internal record identifier to use, all records older will be pruned.
        /// </summary>
        /// <value>The internal record identifier to use, all records older will be pruned..</value>
        public long? InternalRecordId { get; private set; }

        /// <summary>
        /// Gets the internal record date to use, all records older will be pruned.
        /// </summary>
        /// <value>The internal record date.</value>
        public DateTime? InternalRecordDate { get; private set; }

        /// <summary>
        /// Gets the details.
        /// </summary>
        /// <value>The details.</value>
        public string Details { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
