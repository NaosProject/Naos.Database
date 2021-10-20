// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutOp{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Put the object to a <see cref="IWriteOnlyStream"/>.
    /// </summary>
    /// <typeparam name="TObject">Type of data being written.</typeparam>
    public partial class PutOp<TObject> : VoidOperationBase, IHaveTags, IForsakeDeepCloneWithVariantsViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutOp{TObject}"/> class.
        /// </summary>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="tags">OPTIONAL tags to put with the record.  DEFAULT is no tags.</param>
        /// <param name="existingRecordEncounteredStrategy">Optional strategy for an existing record.</param>
        /// <param name="recordRetentionCount">Optional number of existing records to retain if <paramref name="existingRecordEncounteredStrategy"/> is set to prune.</param>
        /// <param name="versionMatchStrategy">The optional type version match strategy; DEFAULT is any version.</param>
        public PutOp(
            TObject objectToPut,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            ExistingRecordEncounteredStrategy existingRecordEncounteredStrategy = ExistingRecordEncounteredStrategy.None,
            int? recordRetentionCount = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
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

            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();

            this.ObjectToPut = objectToPut;
            this.Tags = tags;
            this.ExistingRecordEncounteredStrategy = existingRecordEncounteredStrategy;
            this.RecordRetentionCount = recordRetentionCount;
            this.VersionMatchStrategy = versionMatchStrategy;
        }

        /// <summary>
        /// Gets the object to put.
        /// </summary>
        public TObject ObjectToPut { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }

        /// <summary>
        /// Gets the existing record encountered strategy.
        /// </summary>
        public ExistingRecordEncounteredStrategy ExistingRecordEncounteredStrategy { get; private set; }

        /// <summary>
        /// Gets the record retention count if <see cref="ExistingRecordEncounteredStrategy"/> set to pruning.
        /// </summary>
        public int? RecordRetentionCount { get; private set; }

        /// <summary>
        /// Gets the type version match strategy.
        /// </summary>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }
    }
}
