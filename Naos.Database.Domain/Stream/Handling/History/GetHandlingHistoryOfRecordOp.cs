// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetHandlingHistoryOfRecordOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using Naos.Protocol.Domain;

    /// <summary>
    /// Handles a record.
    /// </summary>
    public partial class GetHandlingHistoryOfRecordOp : ReturningOperationBase<IReadOnlyList<StreamRecordHandlingEntry>>, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetHandlingHistoryOfRecordOp"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal records identifier.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        public GetHandlingHistoryOfRecordOp(
            long internalRecordId,
            IResourceLocator specifiedResourceLocator = null)
        {
            this.InternalRecordId = internalRecordId;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <summary>
        /// Gets the concern.
        /// </summary>
        /// <value>The concern.</value>
        public long InternalRecordId { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
