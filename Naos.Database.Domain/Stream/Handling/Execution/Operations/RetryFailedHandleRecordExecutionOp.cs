// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RetryFailedHandleRecordExecutionOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Operation to mark a <see cref="HandleRecordOp"/> execution that is failed as available for handing.
    /// </summary>
    public partial class RetryFailedHandleRecordExecutionOp : VoidOperationBase, IHaveId<long>, IHaveTags, IHaveHandleRecordConcern, IHaveDetails, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RetryFailedHandleRecordExecutionOp"/> class.
        /// </summary>
        /// <param name="id">The internal record identifier concerned with this handling sequence (the effective aggregate identifier of a record handling scenario).</param>
        /// <param name="concern">Record handling concern.</param>
        /// <param name="details">The details for produced events.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        /// <param name="tags">The optional tags for produced events.</param>
        public RetryFailedHandleRecordExecutionOp(
            long id,
            string concern,
            string details,
            IResourceLocator specifiedResourceLocator = null,
            IReadOnlyCollection<NamedValue<string>> tags = null)
        {
            concern.ThrowIfInvalidOrReservedConcern();
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();

            this.Id = id;
            this.Concern = concern;
            this.Details = details;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
            this.Tags = tags;
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
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }
    }
}
