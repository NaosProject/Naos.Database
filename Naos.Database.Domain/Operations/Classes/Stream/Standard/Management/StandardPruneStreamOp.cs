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
    /// Prunes a stream.
    /// </summary>
    /// <remarks>
    /// This is an internal operation; it is designed to honor the contract of an <see cref="IStandardStream"/>.
    /// While technically there are no limitations on who may execute this operation on such a stream,
    /// these are "bare metal" operations and can be misused without a deeper understanding of what will happen.
    /// Most typically, you will use the operations that are exposed via these extension methods
    /// <see cref="ReadOnlyStreamExtensions"/> and <see cref="WriteOnlyStreamExtensions"/>.
    /// </remarks>
    public partial class StandardPruneStreamOp : VoidOperationBase, ISpecifyResourceLocator, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardPruneStreamOp"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal record identifier to use; all records older will be pruned.</param>
        /// <param name="recordTimestampUtc">The internal record date to use; all records older will be pruned.</param>
        /// <param name="details">Details about the operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        public StandardPruneStreamOp(
            long? internalRecordId,
            DateTime? recordTimestampUtc,
            string details,
            IResourceLocator specifiedResourceLocator = null)
        {
            if ((internalRecordId == null) && (recordTimestampUtc == null))
            {
                throw new ArgumentException(Invariant($"Either '{nameof(internalRecordId)}' or '{nameof(recordTimestampUtc)}' must be specified."));
            }

            recordTimestampUtc.MustForArg(nameof(recordTimestampUtc)).BeUtcDateTimeWhenNotNull();
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();

            this.InternalRecordId = internalRecordId;
            this.RecordTimestampUtc = recordTimestampUtc;
            this.Details = details;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <summary>
        /// Gets the internal record identifier to use, all records older will be pruned.
        /// </summary>
        public long? InternalRecordId { get; private set; }

        /// <summary>
        /// Gets the internal record date to use, all records older will be pruned.
        /// </summary>
        public DateTime? RecordTimestampUtc { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
