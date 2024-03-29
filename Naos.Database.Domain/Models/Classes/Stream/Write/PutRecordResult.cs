﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutRecordResult.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OBeautifulCode.Type;

    /// <summary>
    /// The result of executing <see cref="StandardPutRecordOp"/>.
    /// </summary>
    public partial class PutRecordResult : IModelViaCodeGen, IForsakeDeepCloneWithVariantsViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutRecordResult"/> class.
        /// </summary>
        /// <param name="internalRecordIdOfPutRecord">The internal record identifier of the new record.</param>
        /// <param name="existingRecordIds">The internal record identifier of any existing records per matching criteria.</param>
        /// <param name="prunedRecordIds">The internal record identifier of any records that were pruned.</param>
        public PutRecordResult(
            long? internalRecordIdOfPutRecord,
            IReadOnlyCollection<long> existingRecordIds = null,
            IReadOnlyCollection<long> prunedRecordIds = null)
        {
            if ((internalRecordIdOfPutRecord == null) && !(existingRecordIds?.Any() ?? false))
            {
                throw new ArgumentException(FormattableString.Invariant($"Cannot have a null {nameof(internalRecordIdOfPutRecord)} AND and an empty {nameof(existingRecordIds)}; the expectation is that the record was written OR there was an existing record."));
            }

            this.InternalRecordIdOfPutRecord = internalRecordIdOfPutRecord;
            this.ExistingRecordIds = existingRecordIds;
            this.PrunedRecordIds = prunedRecordIds;
        }

        /// <summary>
        /// Gets the internal record identifier of new record.
        /// </summary>
        public long? InternalRecordIdOfPutRecord { get; private set; }

        /// <summary>
        /// Gets the internal record identifier of any existing records per matching criteria.
        /// </summary>
        public IReadOnlyCollection<long> ExistingRecordIds { get; private set; }

        /// <summary>
        /// Gets the internal record identifier of any records that were pruned.
        /// </summary>
        public IReadOnlyCollection<long> PrunedRecordIds { get; private set; }
    }
}
