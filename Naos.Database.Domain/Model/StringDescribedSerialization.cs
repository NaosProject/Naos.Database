// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringDescribedSerialization.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.DescribedSerialization
{
    using System.Text;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// <see cref="DescribedSerializationBase"/> with a string payload.
    /// </summary>
    public partial class StringDescribedSerialization : DescribedSerializationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringDescribedSerialization"/> class.
        /// </summary>
        /// <param name="payloadTypeRepresentation">The type of object serialized.</param>
        /// <param name="serializerRepresentation">The serializer used to generate the payload.</param>
        /// <param name="serializedPayload">The serialized payload.</param>
        public StringDescribedSerialization(
            TypeRepresentation payloadTypeRepresentation,
            SerializerRepresentation serializerRepresentation,
            string serializedPayload)
            : base(payloadTypeRepresentation, serializerRepresentation)
        {
            this.SerializedPayload = serializedPayload;
        }

        /// <summary>
        /// Gets the serialized payload.
        /// </summary>
        public string SerializedPayload { get; private set; }

        /// <inheritdoc />
        public override SerializationFormat GetSerializationFormat()
        {
            var result = SerializationFormat.String;

            return result;
        }

        /// <inheritdoc />
        public override string GetSerializedPayloadAsEncodedString()
        {
            var result = this.SerializedPayload;

            return result;
        }

        /// <inheritdoc />
        public override byte[] GetSerializedPayloadAsEncodedBytes()
        {
            var result = this.SerializedPayload == null
                ? null
                : Encoding.UTF8.GetBytes(this.SerializedPayload);

            return result;
        }
    }
}