// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordPayloadBase.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Diagnostics.CodeAnalysis;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// Base class for the payload of a stream record
    /// (the serialized object that is persisted or being persisted).
    /// </summary>
    public abstract partial class StreamRecordPayloadBase : IModelViaCodeGen
    {
        /// <summary>
        /// Gets the format that the object was serialized into.
        /// </summary>
        /// <returns>
        /// The format that the object was serialized into.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Using method so that extra property isn't serialized.")]
        public abstract SerializationFormat GetSerializationFormat();

        /// <summary>
        /// Gets the serialized payload encoded as a string.
        /// </summary>
        /// <remarks>
        /// <see cref="StringStreamRecordPayload"/> will pass-through the payload.
        /// <see cref="BinaryStreamRecordPayload"/> will Base64-encode the bytes.
        /// </remarks>
        /// <returns>
        /// The serialized payload encoded as a string.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Using method so that extra property isn't serialized.")]
        public abstract string GetSerializedPayloadAsEncodedString();

        /// <summary>
        /// Gets the serialized payload encoded as a byte array.
        /// </summary>
        /// <remarks>
        /// <see cref="StringStreamRecordPayload"/> will UTF-8 encode the payload.
        /// <see cref="BinaryStreamRecordPayload"/> will pass-through the payload.
        /// </remarks>
        /// <returns>
        /// The serialized payload encoded as a byte array.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Using method so that extra property isn't serialized.")]
        public abstract byte[] GetSerializedPayloadAsEncodedBytes();
    }
}
