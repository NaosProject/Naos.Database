// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CancelRunningHandleRecordExecutionOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Cancels a running <see cref="HandleRecordOp"/>, make available for other bots to run? Externally called.
    /// </summary>
    public partial class CancelRunningHandleRecordExecutionOp : VoidOperationBase, IHaveId<long>, IHaveTags, IHaveHandleRecordConcern, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelRunningHandleRecordExecutionOp"/> class.
        /// </summary>
        /// <param name="id">The internal record identifier of the record that is being handled.</param>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="details">Details to write to the resulting event(s).</param>
        /// <param name="tags">OPTIONAL tags to write to the resulting event(s).  DEFAULT is no tags.</param>
        public CancelRunningHandleRecordExecutionOp(
            long id,
            string concern,
            string details,
            IReadOnlyCollection<NamedValue<string>> tags = null)
        {
            concern.ThrowIfInvalidOrReservedConcern();
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();

            this.Id = id;
            this.Concern = concern;
            this.Details = details;
            this.Tags = tags;
        }

        /// <inheritdoc />
        public long Id { get; private set; }

        /// <inheritdoc />
        public string Concern { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }
    }
}
