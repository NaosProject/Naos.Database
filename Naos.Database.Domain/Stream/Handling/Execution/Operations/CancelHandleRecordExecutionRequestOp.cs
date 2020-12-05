﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CancelHandleRecordExecutionRequestOp.cs" company="Naos Project">
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
    /// Operation to mark a request operation execution as canceled.
    /// </summary>
    public partial class CancelHandleRecordExecutionRequestOp : VoidOperationBase, IIdentifiableBy<long>, IHaveTags, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelHandleRecordExecutionRequestOp"/> class.
        /// </summary>
        /// <param name="id">The internal record identifier concerned with this handling sequence (the effective aggregate identifier of a record handling scenario).</param>
        /// <param name="details">The details for produced events.</param>
        /// <param name="tags">The optional tags for produced events.</param>
        public CancelHandleRecordExecutionRequestOp(
            long id,
            string details,
            IReadOnlyDictionary<string, string> tags = null)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
            this.Id = id;
            this.Details = details;
            this.Tags = tags;
        }

        /// <inheritdoc />
        public long Id { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Tags { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
