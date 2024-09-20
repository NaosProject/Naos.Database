// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardPruneStreamOpExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Extensions on <see cref="StandardPruneStreamOp"/>.
    /// </summary>
    public static class StandardPruneStreamOpExtensions
    {
        /// <summary>
        /// Determines whether the specified record should be pruned when executing the specified prune operation.
        /// </summary>
        /// <param name="pruneStreamOp">The prune operation.</param>
        /// <param name="internalRecordId">The internal record identifier of the record to consider for pruning.</param>
        /// <param name="recordTimestampUtc">The timestamp UTC of the record to consider for pruning.</param>
        /// <returns>
        /// <c>true</c> if the record should be pruned, otherwise <c>false</c>.
        /// </returns>
        public static bool ShouldPrune(
            this StandardPruneStreamOp pruneStreamOp,
            long internalRecordId,
            DateTime recordTimestampUtc)
        {
            pruneStreamOp.MustForArg(nameof(pruneStreamOp)).NotBeNull();

            bool? result = null;

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (((result == null) || (result == true)) && (pruneStreamOp.InternalRecordId != null))
            {
                result = internalRecordId <= pruneStreamOp.InternalRecordId;
            }

            if (((result == null) || (result == true)) && (pruneStreamOp.RecordTimestampUtc != null))
            {
                result = recordTimestampUtc <= pruneStreamOp.RecordTimestampUtc;
            }

            return result ?? false;
        }
    }
}
