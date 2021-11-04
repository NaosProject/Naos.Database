// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandlingForStreamEnabledEvent.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Handling of all records was enabled for the entire stream.
    /// </summary>
    public partial class HandlingForStreamEnabledEvent : HandlingEventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlingForStreamEnabledEvent"/> class.
        /// </summary>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        /// <param name="details">The details about the cancellation.</param>
        public HandlingForStreamEnabledEvent(
            DateTime timestampUtc,
            string details)
            : base(timestampUtc, details)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
        }
    }
}