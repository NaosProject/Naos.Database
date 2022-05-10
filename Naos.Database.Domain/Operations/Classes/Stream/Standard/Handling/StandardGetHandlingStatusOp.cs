// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardGetHandlingStatusOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the <see cref="HandlingStatus"/> of one or more records.
    /// </summary>
    /// <remarks>
    /// This is an internal operation; it is designed to honor the contract of an <see cref="IStandardStream"/>.
    /// While technically there are no limitations on who may execute this operation on such a stream,
    /// these are "bare metal" operations and can be misused without a deeper understanding of what will happen.
    /// Most typically, you will use the operations that are exposed via these extension methods
    /// <see cref="ReadOnlyStreamExtensions"/> and <see cref="WriteOnlyStreamExtensions"/>.
    /// </remarks>
    public partial class StandardGetHandlingStatusOp : ReturningOperationBase<IReadOnlyDictionary<long, HandlingStatus>>, ISpecifyResourceLocator, ISpecifyRecordFilter, IHaveHandleRecordConcern
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardGetHandlingStatusOp"/> class.
        /// </summary>
        /// <param name="concern">The record handling concern.</param>
        /// <param name="recordFilter">The filter to apply to the set of records to query for handling status.</param>
        /// <param name="handlingFilter">The filter to apply to the set of handling entries to query for handling status.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        public StandardGetHandlingStatusOp(
            string concern,
            RecordFilter recordFilter,
            HandlingFilter handlingFilter,
            IResourceLocator specifiedResourceLocator = null)
        {
            concern.ThrowIfInvalidOrReservedConcern();
            recordFilter.MustForArg(nameof(recordFilter)).NotBeNull();
            handlingFilter.MustForArg(nameof(handlingFilter)).NotBeNull();

            this.Concern = concern;
            this.RecordFilter = recordFilter;
            this.HandlingFilter = handlingFilter;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <inheritdoc />
        public string Concern { get; private set; }

        /// <inheritdoc />
        public RecordFilter RecordFilter { get; private set; }

        /// <summary>
        /// Gets the handling filter.
        /// </summary>
        public HandlingFilter HandlingFilter { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
