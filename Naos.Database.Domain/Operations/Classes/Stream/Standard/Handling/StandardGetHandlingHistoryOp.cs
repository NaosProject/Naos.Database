// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardGetHandlingHistoryOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the handling history of a single record.
    /// </summary>
    public partial class StandardGetHandlingHistoryOp : ReturningOperationBase<IReadOnlyList<StreamRecordHandlingEntry>>, ISpecifyResourceLocator, IHaveInternalRecordId, IHaveHandleRecordConcern
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardGetHandlingHistoryOp"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal records identifier.</param>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        public StandardGetHandlingHistoryOp(
            long internalRecordId,
            string concern,
            IResourceLocator specifiedResourceLocator = null)
        {
            concern.MustForArg(nameof(concern)).NotBeNullNorWhiteSpace();
            this.InternalRecordId = internalRecordId;
            this.Concern = concern;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <inheritdoc />
        public long InternalRecordId { get; private set; }

        /// <inheritdoc />
        public string Concern { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
