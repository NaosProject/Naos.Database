// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompleteRunningHandleRecordExecutionOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Type;

    /// <summary>
    /// Operation to mark a running operation as completed. (update the state)
    /// </summary>
    public partial class CompleteRunningHandleRecordExecutionOp : VoidOperationBase, IHaveId<long>, IHaveTags, IHaveDetails, IHaveHandleRecordConcern
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteRunningHandleRecordExecutionOp"/> class.
        /// </summary>
        /// <param name="id">The internal record identifier concerned with this handling sequence (the effective aggregate identifier of a record handling scenario).</param>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="details">OPTIONAL details to write to the resulting event(s).  DEFAULT is no details.</param>
        /// <param name="tags">OPTIONAL tags to write to the resulting event(s).  DEFAULT is no tags.</param>
        public CompleteRunningHandleRecordExecutionOp(
            long id,
            string concern,
            string details = null,
            IReadOnlyCollection<NamedValue<string>> tags = null)
        {
            concern.ThrowIfInvalidOrReservedConcern();

            this.Id = id;
            this.Concern = concern;
            this.Tags = tags;
            this.Details = details;
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
