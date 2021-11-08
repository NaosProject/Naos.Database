// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutWithIdAndReturnInternalRecordIdOp{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Put the object and return it's internal record identifier from the storage medium
    /// or null if the specified strategy for dealing with an existing record prevents the object from being written.
    /// NOTE: this is only unique locally and sequential in the context of the medium itself and should generally not be used.
    /// There are occasions where this can make sense, i.e. auditing the local identifier that was received when queueing work.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public partial class PutWithIdAndReturnInternalRecordIdOp<TId, TObject> : ReturningOperationBase<long?>, IHaveId<TId>, IHaveTags, IForsakeDeepCloneWithVariantsViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutWithIdAndReturnInternalRecordIdOp{TId,TObject}"/> class.
        /// </summary>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="tags">OPTIONAL tags to put with the record.  DEFAULT is no tags.</param>
        /// <param name="existingRecordStrategy">OPTIONAL strategy to use when an existing record is encountered while writing.  DEFAULT is to put a new record regardless of any existing records.</param>
        /// <param name="recordRetentionCount">OPTIONAL number of existing records to retain if <paramref name="existingRecordStrategy"/> is set to prune.  DEFAULT is n/a.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type) when looking for existing records.  DEFAULT is no filter (any version is acceptable).</param>
        public PutWithIdAndReturnInternalRecordIdOp(
            TId id,
            TObject objectToPut,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            ExistingRecordStrategy existingRecordStrategy = ExistingRecordStrategy.None,
            int? recordRetentionCount = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
        {
            tags.MustForArg(nameof(tags)).NotContainAnyNullElementsWhenNotNull();
            existingRecordStrategy.MustForArg(nameof(existingRecordStrategy)).NotBeEqualTo(ExistingRecordStrategy.Unknown);

            if ((existingRecordStrategy == ExistingRecordStrategy.PruneIfFoundById) || (existingRecordStrategy == ExistingRecordStrategy.PruneIfFoundByIdAndType))
            {
                recordRetentionCount.MustForArg(nameof(recordRetentionCount)).NotBeNull("Must have a retention count if pruning.").And().BeGreaterThanOrEqualTo((int?)0);
            }
            else
            {
                recordRetentionCount.MustForArg(nameof(recordRetentionCount)).BeNull("Cannot have a retention count if not pruning.");
            }

            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();

            this.Id = id;
            this.ObjectToPut = objectToPut;
            this.Tags = tags;
            this.ExistingRecordStrategy = existingRecordStrategy;
            this.RecordRetentionCount = recordRetentionCount;
            this.VersionMatchStrategy = versionMatchStrategy;
        }

        /// <inheritdoc />
        public TId Id { get; private set; }

        /// <summary>
        /// Gets the object to put.
        /// </summary>
        public TObject ObjectToPut { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }

        /// <summary>
        /// Gets the strategy to use when an existing record is encountered while writing.
        /// </summary>
        public ExistingRecordStrategy ExistingRecordStrategy { get; private set; }

        /// <summary>
        /// Gets the number of existing records to retain if <see cref="ExistingRecordStrategy"/> is set to prune.
        /// </summary>
        public int? RecordRetentionCount { get; private set; }

        /// <summary>
        /// Gets the strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type) when looking for existing records.
        /// </summary>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }
    }
}
