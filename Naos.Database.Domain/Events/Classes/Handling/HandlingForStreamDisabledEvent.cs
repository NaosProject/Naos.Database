// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandlingForStreamDisabledEvent.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Handling of all records was disabled for the entire stream.
    /// </summary>
    public partial class HandlingForStreamDisabledEvent : HandlingEventBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlingForStreamDisabledEvent"/> class.
        /// </summary>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        /// <param name="details">Details about the event.</param>
        public HandlingForStreamDisabledEvent(
            DateTime timestampUtc,
            string details)
            : base(timestampUtc, details)
        {
            details.MustForArg(nameof(details)).NotBeNullNorWhiteSpace();
        }
    }
}
