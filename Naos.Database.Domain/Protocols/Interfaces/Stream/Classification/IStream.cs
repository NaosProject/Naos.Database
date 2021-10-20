// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Serialization;

    /// <summary>
    /// Stream interface, a stream is a list of records ordered by timestamp.
    /// </summary>
    public interface IStream
    {
        /// <summary>
        /// Gets the name of the stream.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a representation of the stream.
        /// </summary>
        /// <remarks>
        /// This enables the stream to be passed thru process boundaries.
        /// The stream provides a representation of itself, which can be serialized
        /// and then later deserialized and available to execute operations against.
        /// </remarks>
        IStreamRepresentation StreamRepresentation { get; }

        /// <summary>
        /// Gets the resource locator protocols to use.
        /// </summary>
        IResourceLocatorProtocols ResourceLocatorProtocols { get; }

        /// <summary>
        /// Gets the serializer factory.
        /// </summary>
        /// <remarks>
        /// This is used whenever a serializer is needed (serializing or deserializing identifiers and record payload).
        /// </remarks>
        ISerializerFactory SerializerFactory { get; }

        /// <summary>
        /// Gets the default serializer representation.
        /// </summary>
        /// <remarks>
        /// This is always used to serialize and deserialize identifiers into/from <see cref="StreamRecordMetadata.StringSerializedId"/>.
        /// Also, in the absence of a specified serializer representation, it is used to serialize the record payload.
        /// It is NOT used to deserialize the record payload because the record itself contains the serializer representation
        /// of the serializer used to create the record.
        /// </remarks>
        SerializerRepresentation DefaultSerializerRepresentation { get; }

        /// <summary>
        /// Gets the default serialization format.
        /// </summary>
        /// <remarks>
        /// In the absence of a specified serialization format, this one is used when serializing the record payload.
        /// It is NOT used when deserializing the record payload because the record itself contains the format that was
        /// used to serialize the record.
        /// </remarks>
        SerializationFormat DefaultSerializationFormat { get; }
    }
}
