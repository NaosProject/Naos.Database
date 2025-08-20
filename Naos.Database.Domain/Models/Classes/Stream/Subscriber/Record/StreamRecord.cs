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
            StreamRecordPayloadBase payload)
        {
            // Note: We should rip out SerializerRepresentation from Metadata and put it into StreamRecordPayloadBase.
            // This would make the StreamRecordPayloadBase a lightweight DescribedSerializationBase.
            // It would only be missing the Object Type Representation, which already exists in Metadata and we want
            // it in Metadata so that we can get at that information with Metadata-only queries.
            // Note We could always create a new Op that returns just a DescribedSerializationBase
            // (not a record, not metadata, but JUST a DescribedSerializationBase).  In fact we already have the extension
            // method GetDescribedSerialization on StreamRecord so we utilize that, or if we needed a higher performing
            // implementation we could add a new Standard op and have implementers like Sql Server only pull the information
            // required to build the Described Serialization.
            // When we moved SerializerFactory from IStream to IStandardStream we removed the coupling
            // of serialization logic to the definition of a Stream.
            // Keeping SerializerRepresentation in StreamRecordMetadata is now incorrect because it persists
            // that coupling.  We could not expose any operations that return metadata with this
            // information but the issue is that if you needed to get a record w/out deserializing
            // (or decrypting or decompressing, etc) in the case of a message router that is making a
            // decision on the tags or other metadata and passing the record along without the ability
            // to interpret the record.  To keep this ability and decouple the serialization logic from
            // the non standard stream we can move the SerializerRepresentation into the StreamRecordPayloadBase
            // which would then only expose a serialization contract.

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
        public StreamRecordPayloadBase Payload { get; private set; }
    }
}
