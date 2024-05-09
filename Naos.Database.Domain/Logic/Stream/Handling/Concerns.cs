// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Concerns.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Logic and constants related to record handling concern.
    /// </summary>
    public static class Concerns
    {
        /// <summary>
        /// The default execution concern to use when handling a <see cref="ExecuteOpRequestedEvent{TOperation}"/>.
        /// </summary>
        public const string DefaultConcern = "default";

        /// <summary>
        /// The default execution concern to use when handling a <see cref="ExecuteOpRequestedEvent{TOperation}"/>.
        /// </summary>
        public const string DefaultExecutionConcern = "default-execute-op";

        /// <summary>
        /// The default concern to use when handling a <see cref="MutexObject"/>.
        /// </summary>
        public const string DefaultMutexConcern = "default-mutex";

        /// <summary>
        /// The record handling concern to use when record handling is disabled for an entire stream.
        /// </summary>
        public const string StreamHandlingDisabledConcern = "stream-handling-disabled";

        /// <summary>
        /// The record handling concern to use when record handling is disabled for a specific record.
        /// </summary>
        public const string RecordHandlingDisabledConcern = "record-handling-disabled";

        /// <summary>
        /// The internal record identifier of global blocking when interacting with the <see cref="StreamHandlingDisabledConcern"/>.
        /// </summary>
        public const long GlobalBlockingRecordId = 0;

        /// <summary>
        /// Throws if the concern specified is invalid or reserved.
        /// </summary>
        /// <param name="concern">The concern to check.</param>
        public static void ThrowIfInvalidOrReservedConcern(
            this string concern)
        {
            if (concern == null)
            {
                throw new ArgumentNullException(nameof(concern));
            }

            if (string.IsNullOrWhiteSpace(concern))
            {
                throw new ArgumentException("Cannot be white space.", nameof(concern));
            }

            if (concern == StreamHandlingDisabledConcern)
            {
                throw new ArgumentException(Invariant($"Specified concern '{concern}' is reserved for internal use and may not be used."));
            }
        }
    }
}
