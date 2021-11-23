// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecord.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// A record in a <see cref="IReadWriteStream"/>.
    /// </summary>
    public partial class StreamRecord : IModelViaCodeGen, IHaveInternalRecordId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamRecord"/> class.
        /// </summary>
        /// <remarks>
        /// The <see cref="SerializerRepresentation"/> can be found on both <see cref="Metadata"/> and <see cref="Payload"/>.
        /// This is deliberate duplication.  We have contracts that require us to return the payload only and further other
        /// contracts that require us to return the metadata only.  In both of these scenarios, the <see cref="SerializerRepresentation"/>
        /// is required.  To remove this duplication in this contract would produce a model that is worse than just maintaining the duplication.
        /// </remarks>
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

        /// <inheritdoc />
        public long InternalRecordId { get; private set; }

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        public StreamRecordMetadata Metadata { get; private set; }

        /// <summary>
        /// Gets the payload.
        /// </summary>
        public DescribedSerializationBase Payload { get; private set; }
    }
}
