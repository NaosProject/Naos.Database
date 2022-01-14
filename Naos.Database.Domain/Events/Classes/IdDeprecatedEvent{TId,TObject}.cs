// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdDeprecatedEvent{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Event to indicate in a stream that an identifier is no longer expected to be used.  This can be useful in marking the event and also to filter out identifiers which have been removed.
    /// </summary>
    /// <typeparam name="TId">Type of identifier being deprecated.</typeparam>
    /// <typeparam name="TObject">Type of object for the identifier being deprecated.</typeparam>
    public partial class IdDeprecatedEvent<TId, TObject> : EventBase<TId>, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdDeprecatedEvent{TId, TObject}"/> class.
        /// </summary>
        /// <param name="id">The identifier being deprecated.</param>
        /// <param name="timestampUtc">The timestamp UTC.</param>
        /// <param name="details">The details.</param>
        public IdDeprecatedEvent(
            TId id,
            DateTime timestampUtc,
            string details = null)
            : base(id, timestampUtc)
        {
            this.Details = details;
        }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
