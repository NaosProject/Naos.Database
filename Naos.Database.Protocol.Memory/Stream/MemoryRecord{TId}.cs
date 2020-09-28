// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryRecord{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.Memory
{
    using System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// Record in the <see cref="MemoryStream{TId}"/>.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    public partial class MemoryRecord<TId> : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryRecord{TId}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="describedSerialization">Paylod of object.</param>
        /// <param name="dateTimeUtc">The timestamp in UTC.</param>
        public MemoryRecord(
            TId id,
            DescribedSerialization describedSerialization,
            DateTime dateTimeUtc)
        {
            this.Id = id;
            this.DescribedSerialization = describedSerialization ?? throw new ArgumentNullException(nameof(describedSerialization));
            this.DateTimeUtc = dateTimeUtc;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public TId Id { get; private set; }

        /// <summary>
        /// Gets the described serialization.
        /// </summary>
        /// <value>The described serialization.</value>
        public DescribedSerialization DescribedSerialization { get; private set; }

        /// <summary>
        /// Gets the timestamp in UTC.
        /// </summary>
        /// <value>The timestamp in UTC.</value>
        public DateTime DateTimeUtc { get; private set; }
    }
}
