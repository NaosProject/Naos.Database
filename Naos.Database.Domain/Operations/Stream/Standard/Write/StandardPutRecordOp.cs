// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardPutRecordOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// Event to record the execution of <see cref="GetNextUniqueLongOp"/>.
    /// </summary>
    /// <remarks>
    /// This is an internal operation; it is designed to honor the contract of an <see cref="IStandardReadWriteStream"/>.
    /// While technically there are no limitations on who may execute this operation on such a stream,
    /// these are "bare metal" operations and can be misused without a deeper understanding of what will happen.
    /// Most typically, you will use the operations that are exposed via these extension methods
    /// <see cref="ReadOnlyStreamExtensions"/> and <see cref="WriteOnlyStreamExtensions"/>.
    /// </remarks>
    public partial class StandardPutRecordOp : ReturningOperationBase<PutRecordResult>, ISpecifyResourceLocator, IForsakeDeepCloneWithVariantsViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardPutRecordOp"/> class.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="payload">The payload.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        /// <param name="existingRecordEncounteredStrategy">Optional strategy for an existing record.</param>
        /// <param name="recordRetentionCount">Optional number of existing records to retain if <paramref name="existingRecordEncounteredStrategy"/> is set to prune.</param>
        /// <param name="internalRecordId">Optional internal record identifier to specify.</param>
        /// <param name="versionMatchStrategy">The optional type version match strategy; DEFAULT is any version.</param>
        public StandardPutRecordOp(
            StreamRecordMetadata metadata,
            DescribedSerializationBase payload,
            IResourceLocator specifiedResourceLocator = null,
            ExistingRecordEncounteredStrategy existingRecordEncounteredStrategy = ExistingRecordEncounteredStrategy.None,
            int? recordRetentionCount = null,
            long? internalRecordId = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
        {
            metadata.MustForArg(nameof(metadata)).NotBeNull();
            payload.MustForArg(nameof(payload)).NotBeNull();

            if (existingRecordEncounteredStrategy == ExistingRecordEncounteredStrategy.PruneIfFoundById
             || existingRecordEncounteredStrategy == ExistingRecordEncounteredStrategy.PruneIfFoundByIdAndType)
            {
                recordRetentionCount.MustForArg(nameof(recordRetentionCount)).NotBeNull("Must have a retention count if pruning.");
            }
            else
            {
                recordRetentionCount.MustForArg(nameof(recordRetentionCount)).BeNull("Cannot have a retention count if not pruning.");
            }

            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();

            this.Metadata = metadata;
            this.Payload = payload;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
            this.ExistingRecordEncounteredStrategy = existingRecordEncounteredStrategy;
            this.RecordRetentionCount = recordRetentionCount;
            this.InternalRecordId = internalRecordId;
            this.VersionMatchStrategy = versionMatchStrategy;
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        public StreamRecordMetadata Metadata { get; private set; }

        /// <summary>
        /// Gets the payload.
        /// </summary>
        public DescribedSerializationBase Payload { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }

        /// <summary>
        /// Gets the existing record encountered strategy.
        /// </summary>
        public ExistingRecordEncounteredStrategy ExistingRecordEncounteredStrategy { get; private set; }

        /// <summary>
        /// Gets the record retention count if <see cref="ExistingRecordEncounteredStrategy"/> set to pruning.
        /// </summary>
        public int? RecordRetentionCount { get; private set; }

        /// <summary>
        /// Gets the internal record identifier.
        /// </summary>
        public long? InternalRecordId { get; private set; }

        /// <summary>
        /// Gets the type version match strategy.
        /// </summary>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }
    }
}
