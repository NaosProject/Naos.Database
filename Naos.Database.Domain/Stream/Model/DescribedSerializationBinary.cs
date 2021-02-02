// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DescribedSerializationBinary.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// <see cref="DescribedSerializationBase"/> with a binary payload.
    /// </summary>
    public partial class DescribedSerializationBinary : DescribedSerializationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DescribedSerializationBinary"/> class.
        /// </summary>
        /// <param name="payloadTypeRepresentation">The payload type representation.</param>
        /// <param name="serializerRepresentation">The serializer representation.</param>
        /// <param name="serializedPayload">The serialized payload.</param>
        public DescribedSerializationBinary(
            TypeRepresentation payloadTypeRepresentation,
            SerializerRepresentation serializerRepresentation,
            IReadOnlyList<byte> serializedPayload)
            : base(payloadTypeRepresentation, serializerRepresentation)
        {
            this.SerializedPayload = serializedPayload;
        }

        /// <summary>
        /// Gets the serialized payload.
        /// </summary>
        /// <value>The serialized payload.</value>
        public IReadOnlyList<byte> SerializedPayload { get; private set; }

        /// <inheritdoc />
        public override SerializationFormat SerializationFormat => SerializationFormat.Binary;
    }
}