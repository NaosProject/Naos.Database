// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutAndReturnInternalRecordIdOp{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Put the object to the stream and return it's internal identifier.
    /// NOTE: this is only unique local and sequential in the context of the stream itself and should generally not be used.
    /// There are occasions where this can make sense, i.e. auditing the local identifier that was received when queueing work.
    /// </summary>
    /// <typeparam name="TObject">Type of data being written.</typeparam>
    public partial class PutAndReturnInternalRecordIdOp<TObject> : ReturningOperationBase<long?>, IHaveTags, ISpecifyResourceLocator, IForsakeDeepCloneWithVariantsViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutAndReturnInternalRecordIdOp{TObject}"/> class.
        /// </summary>
        /// <param name="objectToPut">The object to put into a stream.</param>
        /// <param name="tags">Optional tags to put with the record.</param>
        /// <param name="existingRecordEncounteredStrategy">Optional strategy for an existing record.</param>
        /// <param name="recordRetentionCount">Optional number of existing records to retain if <paramref name="existingRecordEncounteredStrategy"/> is set to prune.</param>
        /// <param name="versionMatchStrategy">The optional type version match strategy; DEFAULT is any version.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        public PutAndReturnInternalRecordIdOp(
            TObject objectToPut,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            ExistingRecordEncounteredStrategy existingRecordEncounteredStrategy = ExistingRecordEncounteredStrategy.None,
            int? recordRetentionCount = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IResourceLocator specifiedResourceLocator = null)
        {
            if (existingRecordEncounteredStrategy == ExistingRecordEncounteredStrategy.PruneIfFoundById
             || existingRecordEncounteredStrategy == ExistingRecordEncounteredStrategy.PruneIfFoundByIdAndType)
            {
                recordRetentionCount.MustForArg(nameof(recordRetentionCount)).NotBeNull("Must have a retention count if pruning.");
            }
            else
            {
                recordRetentionCount.MustForArg(nameof(recordRetentionCount)).BeNull("Cannot have a retention count if not pruning.");
            }

            this.ObjectToPut = objectToPut;
            this.Tags = tags;
            this.ExistingRecordEncounteredStrategy = existingRecordEncounteredStrategy;
            this.RecordRetentionCount = recordRetentionCount;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <value>The object.</value>
        public TObject ObjectToPut { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }

        /// <summary>
        /// Gets the existing record encountered strategy.
        /// </summary>
        /// <value>The existing record encountered strategy.</value>
        public ExistingRecordEncounteredStrategy ExistingRecordEncounteredStrategy { get; private set; }

        /// <summary>
        /// Gets the record retention count if <see cref="ExistingRecordEncounteredStrategy"/> set to pruning.
        /// </summary>
        /// <value>The record retention count if <see cref="ExistingRecordEncounteredStrategy"/> set to pruning.</value>
        public int? RecordRetentionCount { get; private set; }

        /// <summary>
        /// Gets the type version match strategy.
        /// </summary>
        /// <value>The type version match strategy.</value>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
