// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CancelRunningHandleRecordExecutionOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Operation to mark a running <see cref="HandleRecordOp"/> as canceled externally.
    /// </summary>
    public partial class CancelRunningHandleRecordExecutionOp : VoidOperationBase, IIdentifiableBy<long>, IHaveTags, IHaveHandleRecordConcern, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelRunningHandleRecordExecutionOp"/> class.
        /// </summary>
        /// <param name="id">The internal record identifier concerned with this handling sequence (the effective aggregate identifier of a record handling scenario).</param>
        /// <param name="concern">Record handling concern.</param>
        /// <param name="details">The details for produced events.</param>
        /// <param name="tags">The optional tags for produced events.</param>
        public CancelRunningHandleRecordExecutionOp(
            long id,
            string concern,
            string details,
            IReadOnlyDictionary<string, string> tags = null)
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
        public IReadOnlyDictionary<string, string> Tags { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
