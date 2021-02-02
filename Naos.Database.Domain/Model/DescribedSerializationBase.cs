// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DescribedSerializationBase.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.DescribedSerialization
{
    using System;

    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// A portable object that contains all the information required to deserialize
    /// some contained/subject object: the serialized payload of the subject object,
    /// the type of the subject object, and the serializer that was used to serialize the subject object.
    /// </summary>
    /// <remarks>
    /// This provides the ability to persist, transport, and route
    /// (e.g. make decisions based on properties of this object)
    /// an object through code layers that do not the object type loaded, while
    /// enabling a consumer having the object and the serializer loaded,
    /// to deserialize the object.
    /// </remarks>
    public abstract partial class DescribedSerializationBase : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DescribedSerializationBase"/> class.
        /// </summary>
        /// <param name="payloadTypeRepresentation">The type of object serialized.</param>
        /// <param name="serializerRepresentation">The serializer used to generate the payload.</param>
        protected DescribedSerializationBase(
            TypeRepresentation payloadTypeRepresentation,
            SerializerRepresentation serializerRepresentation)
        {
            if (payloadTypeRepresentation == null)
            {
                throw new ArgumentNullException(nameof(payloadTypeRepresentation));
            }

            if (serializerRepresentation == null)
            {
                throw new ArgumentNullException(nameof(serializerRepresentation));
            }

            this.PayloadTypeRepresentation = payloadTypeRepresentation;
            this.SerializerRepresentation = serializerRepresentation;
        }

        /// <summary>
        /// Gets the type of object serialized.
        /// </summary>
        public TypeRepresentation PayloadTypeRepresentation { get; private set; }

        /// <summary>
        /// Gets the representation of the serializer used to generate the payload.
        /// </summary>
        public SerializerRepresentation SerializerRepresentation { get; private set; }

        /// <summary>
        /// Gets the format that the object was serialized into.
        /// </summary>
        /// <returns>
        /// The format that the object was serialized into.
        /// </returns>
        public abstract SerializationFormat GetSerializationFormat();

        /// <summary>
        /// Gets the serialized payload encoded as a string.
        /// </summary>
        /// <remarks>
        /// <see cref="StringDescribedSerialization"/> will pass-through the payload.
        /// <see cref="BinaryDescribedSerialization"/> will Base64-encode the bytes.
        /// </remarks>
        /// <returns>
        /// The serialized payload encoded as a string.
        /// </returns>
        public abstract string GetSerializedPayloadAsEncodedString();

        /// <summary>
        /// Gets the serialized payload encoded as a byte array.
        /// </summary>
        /// <remarks>
        /// <see cref="StringDescribedSerialization"/> will UTF-8 encode the payload.
        /// <see cref="BinaryDescribedSerialization"/> will pass-through the payload.
        /// </remarks>
        /// <returns>
        /// The serialized payload encoded as a string.
        /// </returns>
        public abstract byte[] GetSerializedPayloadAsEncodedBytes();
    }
}
