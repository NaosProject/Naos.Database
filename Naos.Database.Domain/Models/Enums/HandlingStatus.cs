// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandlingStatus.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// The record handling status.
    /// </summary>
    /// <remarks>
    /// The status encapsulates both the state and the transition into that state.
    /// For example, there are four "AvailableXXX" statuses.  In any of those cases the
    /// record is in the Available state, but they are differentiated so that it is
    /// clear what transition triggered the entry into that state.
    /// </remarks>
    public enum HandlingStatus
    {
        /// <summary>
        /// There is an unknown status.
        /// </summary>
        Unknown,

        /// <summary>
        /// The record is available to be handled for a specified concern
        /// (this is the default status; when there has been no handling activity at all on a record for a concern).
        /// </summary>
        AvailableByDefault,

        /// <summary>
        /// The record is being handled for a specified concern.
        /// </summary>
        Running,

        /// <summary>
        /// The record was handled for a specified concern and executed without an error.
        /// </summary>
        Completed,

        /// <summary>
        /// The record was handled for a specified concern but an error occurred when executing.
        /// </summary>
        Failed,

        /// <summary>
        /// The record is available to be handled for a specified concern after it <see cref="Failed"/>.
        /// </summary>
        AvailableAfterFailure,

        /// <summary>
        /// The record is available to be handled for a specified concern after it was <see cref="Running"/> and then externally cancelled.
        /// </summary>
        AvailableAfterExternalCancellation,

        /// <summary>
        /// The record is available to be handled for a specified concern after it was <see cref="Running"/> and then it cancelled itself.
        /// </summary>
        AvailableAfterSelfCancellation,

        /// <summary>
        /// Handling of all records is disabled for the entire stream; no record can be handled for any concern.
        /// </summary>
        DisabledForStream,

        /// <summary>
        /// Handling of the record is disabled for all concerns; the record cannot be handled for any concern.
        /// </summary>
        DisabledForRecord,
    }
}
