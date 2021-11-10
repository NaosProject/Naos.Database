// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Diagnostics.CodeAnalysis;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// Stream interface, a stream is a list of records ordered by timestamp.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "'Stream' is the best term we could come up with; it's potential confusion with System.IO.Stream was debated.")]
    public interface IStream : IHaveStreamRepresentation
    {
        /// <summary>
        /// Gets the name of the stream.
        /// </summary>
        string Name { get; }

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
