// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecord{TObject}.cs" company="Naos Project">
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
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public partial class StreamRecord<TObject> : IModelViaCodeGen, IHaveInternalRecordId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamRecord{TObject}"/> class.
        /// </summary>
        /// <param name="internalRecordId">The identifier.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="payload">The payload.</param>
        public StreamRecord(
            long internalRecordId,
            StreamRecordMetadata metadata,
            TObject payload)
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
        public StreamRecordMetadata Metadata { get; private set; }

        /// <summary>
        /// Gets the payload.
        /// </summary>
        public TObject Payload { get; private set; }
    }
}
