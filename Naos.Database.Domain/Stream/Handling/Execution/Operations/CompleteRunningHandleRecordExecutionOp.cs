// --------------------------------------------------------------------------------------------------------------------
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
    public partial class CompleteRunningHandleRecordExecutionOp : VoidOperationBase, IIdentifiableBy<long>, IHaveTags, IHaveDetails, IHaveHandleRecordConcern, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteRunningHandleRecordExecutionOp"/> class.
        /// </summary>
        /// <param name="id">The internal record identifier concerned with this handling sequence (the effective aggregate identifier of a record handling scenario).</param>
        /// <param name="concern">Record handling concern.</param>
        /// <param name="details">The optional details about the completion.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        /// <param name="tags">The optional tags for produced events.</param>
        public CompleteRunningHandleRecordExecutionOp(
            long id,
            string concern,
            string details = null,
            IResourceLocator specifiedResourceLocator = null,
            IReadOnlyDictionary<string, string> tags = null)
        {
            concern.ThrowIfInvalidOrReservedConcern();

            this.Id = id;
            this.Concern = concern;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
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
        public IResourceLocator SpecifiedResourceLocator { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Tags { get; private set; }
    }
}
