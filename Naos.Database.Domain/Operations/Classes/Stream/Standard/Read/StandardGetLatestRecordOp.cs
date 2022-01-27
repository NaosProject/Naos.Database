// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardGetLatestRecordOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the most recent record.
    /// </summary>
    /// <remarks>
    /// This is an internal operation; it is designed to honor the contract of an <see cref="IStandardStream"/>.
    /// While technically there are no limitations on who may execute this operation on such a stream,
    /// these are "bare metal" operations and can be misused without a deeper understanding of what will happen.
    /// Most typically, you will use the operations that are exposed via these extension methods
    /// <see cref="ReadOnlyStreamExtensions"/> and <see cref="WriteOnlyStreamExtensions"/>.
    /// </remarks>
    public partial class StandardGetLatestRecordOp : ReturningOperationBase<StreamRecord>, ISpecifyRecordFilter, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardGetLatestRecordOp"/> class.
        /// </summary>
        /// <param name="recordFilter">Filter to evaluate on records.</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <param name="streamRecordItemsToInclude">OPTIONAL items to retrieve.  DEFAULT is <see cref="StreamRecordItemsToInclude.MetadataAndPayload"/>.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        public StandardGetLatestRecordOp(
            RecordFilter recordFilter,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault,
            StreamRecordItemsToInclude streamRecordItemsToInclude = StreamRecordItemsToInclude.MetadataAndPayload,
            IResourceLocator specifiedResourceLocator = null)
        {
            recordFilter.MustForArg(nameof(recordFilter)).NotBeNull();
            recordNotFoundStrategy.MustForArg(nameof(recordNotFoundStrategy)).NotBeEqualTo(RecordNotFoundStrategy.Unknown);
            streamRecordItemsToInclude.MustForArg(nameof(streamRecordItemsToInclude)).NotBeEqualTo(StreamRecordItemsToInclude.Unknown);

            this.RecordFilter = recordFilter;
            this.RecordNotFoundStrategy = recordNotFoundStrategy;
            this.StreamRecordItemsToInclude = streamRecordItemsToInclude;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <inheritdoc />
        public RecordFilter RecordFilter { get; private set; }

        /// <summary>
        /// Gets the strategy to use when no record(s) are found.
        /// </summary>
        public RecordNotFoundStrategy RecordNotFoundStrategy { get; private set; }

        /// <summary>
        /// Gets the items to retrieve from the record.
        /// </summary>
        public StreamRecordItemsToInclude StreamRecordItemsToInclude { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
