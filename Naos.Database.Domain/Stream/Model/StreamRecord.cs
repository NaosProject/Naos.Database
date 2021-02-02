// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecord.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Database.Domain.DescribedSerialization;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// The record in a <see cref="IReadWriteStream"/>; metadata and the actual object.
    /// </summary>
    public partial class StreamRecord : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamRecord"/> class.
        /// </summary>
        /// <param name="internalRecordId">The identifier.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="payload">The payload.</param>
        public StreamRecord(
            long internalRecordId,
            StreamRecordMetadata metadata,
            DescribedSerializationBase payload)
        {
            metadata.MustForArg(nameof(metadata)).NotBeNull();
            payload.MustForArg(nameof(payload)).NotBeNull();

            this.InternalRecordId = internalRecordId;
            this.Metadata = metadata;
            this.Payload = payload;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public long InternalRecordId { get; private set; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        /// <value>The metadata.</value>
        public StreamRecordMetadata Metadata { get; private set; }

        /// <summary>
        /// Gets the payload.
        /// </summary>
        /// <value>The payload.</value>
        public DescribedSerializationBase Payload { get; private set; }
    }
}
