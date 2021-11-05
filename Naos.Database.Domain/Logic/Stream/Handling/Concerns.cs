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
        /// The record handling concern.
        /// </summary>
        public const string RecordHandlingConcern = "RecordHandling";

        /// <summary>
        /// The internal record identifier of global blocking when interacting with the <see cref="RecordHandlingConcern"/>.
        /// </summary>
        public const long GlobalBlockingRecordId = 0;

        /// <summary>
        /// Throws if the concern specified is reserved.
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

            if (concern == RecordHandlingConcern)
            {
                throw new ArgumentException(Invariant($"Concern '{concern}' is reserved for internal use and may not be used."));
            }
        }
    }
}
