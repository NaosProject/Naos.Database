// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Concerns.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using static System.FormattableString;

    /// <summary>
    /// Logic and constants related to record handling concern.
    /// </summary>
    public static class Concerns
    {
        /// <summary>
        /// The record handling concern to use when record handling is disabled for an entire stream.
        /// </summary>
        public const string StreamHandlingDisabledConcern = "StreamHandlingDisabled";

        /// <summary>
        /// The record handling concern to use when record handling is disabled for a specific record.
        /// </summary>
        public const string RecordHandlingDisabledConcern = "RecordHandlingDisabled";

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
