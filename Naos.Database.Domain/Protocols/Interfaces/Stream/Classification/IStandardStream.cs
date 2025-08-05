// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStandardStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Diagnostics.CodeAnalysis;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// A stream supporting a standard set of read, write, recording handling, and management operations.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "'Stream' is the best term we could come up with; it's potential confusion with System.IO.Stream was debated.")]
    public interface IStandardStream
        :
          IReadWriteStream,
          IRecordHandlingOnlyStream,
          IManagementOnlyStream,
          IStandardStreamReadProtocols,
          IStandardStreamWriteProtocols,
          IStandardStreamManagementProtocols,
          IStandardStreamRecordHandlingProtocols
    {
        /// <summary>
        /// Gets the resource locator protocols to use.
        /// </summary>
        IResourceLocatorProtocols ResourceLocatorProtocols { get; }

        /// <summary>
        /// Gets the serializer factory to use to get serializers for objects (not identifiers),
        /// regardless of putting new records or getting existing records.
        /// </summary>
        ISerializerFactory SerializerFactory { get; }

        /// <summary>
        /// Gets the serializer representation to use to get a serializer to use
        /// when serializing objects (not identifiers) into record payloads to put.
        /// </summary>
        /// <remarks>
        /// For ops that require object serialization (non-standard Ops), this representation is sent to the
        /// <see cref="SerializerFactory"/> to get the serializer to use.
        /// For ops that require object deserialization (non-standard Ops), it is NOT used to get a serializer.
        /// The record itself contains the serializer representation of the serializer used to create the record,
        /// and it is that representation that's sent to <see cref="SerializerFactory"/> to get the serializer to use.
        /// </remarks>
        SerializerRepresentation DefaultSerializerRepresentation { get; }

        /// <summary>
        /// Gets the serialization format to use
        /// when serializing objects (not identifiers) into record payloads to put.
        /// </summary>
        /// <remarks>
        /// For ops that require object serialization (non-standard Ops), this format is used.
        /// For ops that require object deserialization (non-standard Ops), it is NOT used.
        /// The record itself contains the format that was used to serialize the record.
        /// </remarks>
        SerializationFormat DefaultSerializationFormat { get; }

        /// <summary>
        /// Gets the string serializer to use for identifiers.
        /// </summary>
        IStringSerializeAndDeserialize IdSerializer { get; }
    }
}
