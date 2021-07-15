// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetHandlingHistoryOfRecordOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Handles a record.
    /// </summary>
    public partial class GetHandlingHistoryOfRecordOp : ReturningOperationBase<IReadOnlyList<StreamRecordHandlingEntry>>, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetHandlingHistoryOfRecordOp"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal records identifier.</param>
        /// <param name="concern">The handling concern.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        public GetHandlingHistoryOfRecordOp(
            long internalRecordId,
            string concern,
            IResourceLocator specifiedResourceLocator = null)
        {
            concern.MustForArg(nameof(concern)).NotBeNullNorWhiteSpace();
            this.InternalRecordId = internalRecordId;
            this.Concern = concern;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <summary>
        /// Gets the concern.
        /// </summary>
        /// <value>The concern.</value>
        public long InternalRecordId { get; private set; }

        /// <summary>
        /// Gets the handling concern.
        /// </summary>
        /// <value>The handling concern.</value>
        public string Concern { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
