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
    /// Central home for logic around 'concern's in handling records.
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
                throw new ArgumentException("Cannot be whitespace.", nameof(concern));
            }

            if (concern == RecordHandlingConcern)
            {
                throw new ArgumentException(Invariant($"Concern '{concern}' is reserved for internal use and may not be used."));
            }
        }
    }

    /// <summary>
    /// Interface to expose record handling concern.
    /// </summary>
    public interface IHaveHandleRecordConcern
    {
        /// <summary>
        /// Gets the record handling concern.
        /// </summary>
        /// <value>The record handling concern.</value>
        string Concern { get; }
    }
}
