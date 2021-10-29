// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardPruneStreamOpExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;

    /// <summary>
    /// Extensions on <see cref="StandardPruneStreamOp"/>.
    /// </summary>
    public static class StandardPruneStreamOpExtensions
    {
        /// <summary>
        /// Check whether the operation coverage includes the provided inputs.
        /// </summary>
        /// <param name="pruneStreamOp">The prune operation.</param>
        /// <param name="internalRecordId">The internal record identifier.</param>
        /// <param name="internalRecordTimestampUtc">The internal record timestamp UTC.</param>
        /// <returns><c>true</c> if record should be included, <c>false</c> otherwise.</returns>
        public static bool ShouldInclude(
            this StandardPruneStreamOp pruneStreamOp,
            long internalRecordId,
            DateTime internalRecordTimestampUtc)
        {
            bool? result = null;

            if ((result == null || result == true) && pruneStreamOp.InternalRecordId != null)
            {
                result = internalRecordId <= pruneStreamOp.InternalRecordId;
            }

            if ((result == null || result == true) && pruneStreamOp.InternalRecordDate != null)
            {
                result = internalRecordTimestampUtc <= pruneStreamOp.InternalRecordDate;
            }

            return result ?? false;
        }
    }
}
