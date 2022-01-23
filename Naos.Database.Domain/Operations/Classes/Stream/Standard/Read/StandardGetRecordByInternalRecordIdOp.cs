// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardGetRecordByInternalRecordIdOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the record by its internal record identifier.
    /// </summary>
    /// <remarks>
    /// This is an internal operation; it is designed to honor the contract of an <see cref="IStandardStream"/>.
    /// While technically there are no limitations on who may execute this operation on such a stream,
    /// these are "bare metal" operations and can be misused without a deeper understanding of what will happen.
    /// Most typically, you will use the operations that are exposed via these extension methods
    /// <see cref="ReadOnlyStreamExtensions"/> and <see cref="WriteOnlyStreamExtensions"/>.
    /// </remarks>
    public partial class StandardGetRecordByInternalRecordIdOp : ReturningOperationBase<StreamRecord>, ISpecifyResourceLocator, IHaveInternalRecordId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardGetRecordByInternalRecordIdOp"/> class.
        /// </summary>
        /// <param name="internalRecordId">The internal record identifier.</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <param name="streamRecordItemsToInclude">OPTIONAL value that determines which aspects of a <see cref="StreamRecord"/> to include with the result.  DEFAULT is to include both the metadata and the payload.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        public StandardGetRecordByInternalRecordIdOp(
            long internalRecordId,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault,
            StreamRecordItemsToInclude streamRecordItemsToInclude = StreamRecordItemsToInclude.MetadataAndPayload,
            IResourceLocator specifiedResourceLocator = null)
        {
            recordNotFoundStrategy.MustForArg(nameof(recordNotFoundStrategy)).NotBeEqualTo(RecordNotFoundStrategy.Unknown);
            streamRecordItemsToInclude.MustForArg(nameof(streamRecordItemsToInclude)).NotBeEqualTo(StreamRecordItemsToInclude.Unknown);

            this.InternalRecordId = internalRecordId;
            this.RecordNotFoundStrategy = recordNotFoundStrategy;
            this.StreamRecordItemsToInclude = streamRecordItemsToInclude;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <inheritdoc />
        public long InternalRecordId { get; private set; }

        /// <summary>
        /// Gets the strategy to use when no record(s) are found.
        /// </summary>
        public RecordNotFoundStrategy RecordNotFoundStrategy { get; private set; }

        /// <summary>
        /// Gets a value that determines which aspects of a <see cref="StreamRecord"/> to include with the result.
        /// </summary>
        public StreamRecordItemsToInclude StreamRecordItemsToInclude { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
