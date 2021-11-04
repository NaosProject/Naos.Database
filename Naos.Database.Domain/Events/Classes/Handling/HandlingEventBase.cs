// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandlingEventBase.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Base class implementation of <see cref="IHandlingEvent"/>.
    /// </summary>
    public abstract partial class HandlingEventBase : EventBase, IHandlingEvent, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlingEventBase"/> class.
        /// </summary>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        /// <param name="details">Details about the event.</param>
        protected HandlingEventBase(
            DateTime timestampUtc,
            string details)
            : base(timestampUtc)
        {
            this.Details = details;
        }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
