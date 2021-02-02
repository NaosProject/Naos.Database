// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DescribedSerializationString.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// <see cref="DescribedSerializationBase"/> with a string payload.
    /// </summary>
    public partial class DescribedSerializationString : DescribedSerializationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DescribedSerializationString"/> class.
        /// </summary>
        /// <param name="payloadTypeRepresentation">The payload type representation.</param>
        /// <param name="serializerRepresentation">The serializer representation.</param>
        /// <param name="serializedPayload">The serialized payload.</param>
        public DescribedSerializationString(
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
        /// <value>The serialized payload.</value>
        public string SerializedPayload { get; private set; }

        /// <inheritdoc />
        public override SerializationFormat SerializationFormat => SerializationFormat.String;
    }
}