// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FailRunningHandleRecordExecutionOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Operation to mark a running <see cref="HandleRecordOp"/> as failed.
    /// </summary>
    public partial class FailRunningHandleRecordExecutionOp : VoidOperationBase, IHaveId<long>, IHaveTags, IHaveHandleRecordConcern, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FailRunningHandleRecordExecutionOp"/> class.
        /// </summary>
        /// <param name="id">The internal record identifier concerned with this handling sequence (the effective aggregate identifier of a record handling scenario).</param>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="details">Details to write to the resulting event(s).</param>
        /// <param name="tags">OPTIONAL tags to write to the resulting event(s).  DEFAULT is no tags.</param>
        public FailRunningHandleRecordExecutionOp(
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
