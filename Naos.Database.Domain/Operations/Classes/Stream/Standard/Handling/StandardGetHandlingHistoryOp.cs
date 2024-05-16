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
    /// Gets the handling history of a single record if any.
    /// If there has been any handling history at all (i.e. the record has been try-handled at least one time),
    /// then it is returned (including an entry for the <see cref="HandlingStatus.AvailableByDefault"/> status).
    /// Otherwise, an empty set of entries is returned (including when an invalid record id is specified).
    /// </summary>
    /// <remarks>
    /// This is an internal operation; it is designed to honor the contract of an <see cref="IStandardStream"/>.
    /// While technically there are no limitations on who may execute this operation on such a stream,
    /// these are "bare metal" operations and can be misused without a deeper understanding of what will happen.
    /// Most typically, you will use the operations that are exposed via these extension methods
    /// <see cref="ReadOnlyStreamExtensions"/> and <see cref="WriteOnlyStreamExtensions"/>.
    /// </remarks>
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
