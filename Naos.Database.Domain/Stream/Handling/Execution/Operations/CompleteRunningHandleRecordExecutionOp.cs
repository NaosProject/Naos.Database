﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompleteRunningHandleRecordExecutionOp.cs" company="Naos Project">
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
    /// Operation to mark a running operation as completed.
    /// </summary>
    public partial class CompleteRunningHandleRecordExecutionOp : VoidOperationBase, IIdentifiableBy<long>, IHaveTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteRunningHandleRecordExecutionOp"/> class.
        /// </summary>
        /// <param name="id">The internal record identifier concerned with this handling sequence (the effective aggregate identifier of a record handling scenario).</param>
        /// <param name="tags">The optional tags for produced events.</param>
        public CompleteRunningHandleRecordExecutionOp(
            long id,
            IReadOnlyDictionary<string, string> tags = null)
        {
            this.Id = id;
            this.Tags = tags;
        }

        /// <inheritdoc />
        public long Id { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Tags { get; private set; }
    }
}
