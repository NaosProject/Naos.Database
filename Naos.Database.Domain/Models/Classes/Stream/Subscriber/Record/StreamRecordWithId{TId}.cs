// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordWithId{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// A record in a <see cref="IReadWriteStream"/>.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public partial class StreamRecordWithId<TId> : IModelViaCodeGen, IHaveInternalRecordId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamRecordWithId{TId}"/> class.
        /// </summary>
        /// <param name="internalRecordId">The identifier.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="payload">The payload.</param>
        public StreamRecordWithId(
            long internalRecordId,
            StreamRecordMetadata<TId> metadata,
            StreamRecordPayloadBase payload)
        {
            metadata.MustForArg(nameof(metadata)).NotBeNull();
            payload.MustForArg(nameof(payload)).NotBeNull();

            this.InternalRecordId = internalRecordId;
            this.Metadata = metadata;
            this.Payload = payload;
        }

        /// <inheritdoc />
        public long InternalRecordId { get; private set; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        public StreamRecordMetadata<TId> Metadata { get; private set; }

        /// <summary>
        /// Gets the payload.
        /// </summary>
        public StreamRecordPayloadBase Payload { get; private set; }
    }
}
