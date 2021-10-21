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
        /// <param name="existingRecordStrategy">OPTIONAL strategy to use when an existing record is encountered while writing.  DEFAULT is to put a new record regardless of any existing records.</param>
        /// <param name="recordRetentionCount">OPTIONAL number of existing records to retain if <paramref name="existingRecordStrategy"/> is set to prune.  DEFAULT is n/a.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the object type when looking for existing records.  DEFAULT is no filter (any version is a match).</param>
        /// <param name="internalRecordId">OPTIONAL internal record identifier to use.  DEFAULT is to let the stream assign the identifier.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        public StandardPutRecordOp(
            StreamRecordMetadata metadata,
            DescribedSerializationBase payload,
            ExistingRecordStrategy existingRecordStrategy = ExistingRecordStrategy.None,
            int? recordRetentionCount = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            long? internalRecordId = null,
            IResourceLocator specifiedResourceLocator = null)
        {
            metadata.MustForArg(nameof(metadata)).NotBeNull();
            payload.MustForArg(nameof(payload)).NotBeNull();

            if (existingRecordStrategy == ExistingRecordStrategy.PruneIfFoundById
             || existingRecordStrategy == ExistingRecordStrategy.PruneIfFoundByIdAndType)
            {
                recordRetentionCount.MustForArg(nameof(recordRetentionCount)).NotBeNull("Must have a retention count if pruning.").And().BeGreaterThanOrEqualTo((int?)0);
            }
            else
            {
                recordRetentionCount.MustForArg(nameof(recordRetentionCount)).BeNull("Cannot have a retention count if not pruning.");
            }

            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();

            this.Metadata = metadata;
            this.Payload = payload;
            this.ExistingRecordStrategy = existingRecordStrategy;
            this.RecordRetentionCount = recordRetentionCount;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.InternalRecordId = internalRecordId;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        public StreamRecordMetadata Metadata { get; private set; }

        /// <summary>
        /// Gets the payload.
        /// </summary>
        public DescribedSerializationBase Payload { get; private set; }

        /// <summary>
        /// Gets the strategy to use when an existing record is encountered while writing.
        /// </summary>
        public ExistingRecordStrategy ExistingRecordStrategy { get; private set; }

        /// <summary>
        /// Gets the number of existing records to retain if <see cref="ExistingRecordStrategy"/> is set to prune.
        /// </summary>
        public int? RecordRetentionCount { get; private set; }

        /// <summary>
        /// Gets the strategy to use to filter on the version of the object type when looking for existing records.
        /// </summary>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the internal record identifier to use.
        /// </summary>
        public long? InternalRecordId { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
