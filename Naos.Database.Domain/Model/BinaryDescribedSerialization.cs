// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryDescribedSerialization.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.DescribedSerialization
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// <see cref="DescribedSerializationBase"/> with a binary payload.
    /// </summary>
    public partial class BinaryDescribedSerialization : DescribedSerializationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryDescribedSerialization"/> class.
        /// </summary>
        /// <param name="payloadTypeRepresentation">The type of object serialized.</param>
        /// <param name="serializerRepresentation">The serializer used to generate the payload.</param>
        /// <param name="serializedPayload">The serialized payload.</param>
        public BinaryDescribedSerialization(
            TypeRepresentation payloadTypeRepresentation,
            SerializerRepresentation serializerRepresentation,
            byte[] serializedPayload)
            : base(payloadTypeRepresentation, serializerRepresentation)
        {
            this.SerializedPayload = serializedPayload;
        }

        /// <summary>
        /// Gets the serialized payload.
        /// </summary>
        /// <remarks>
        /// We are intentionally choosing a byte[] over an IReadOnlyList{byte} for performance
        /// reasons.  The typical usages of this data require a byte array and not a List.
        /// A List incurs the cost of frequent conversion to a byte[].
        /// </remarks>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = ObcSuppressBecause.CA1819_PropertiesShouldNotReturnArrays_DataPayloadsAreCommonlyRepresentedAsByteArrays)]
        public byte[] SerializedPayload { get; private set; }

        /// <inheritdoc />
        public override SerializationFormat GetSerializationFormat()
        {
            var result = SerializationFormat.Binary;

            return result;
        }

        /// <inheritdoc />
        public override string GetSerializedPayloadAsEncodedString()
        {
            var result = this.SerializedPayload == null
                ? null
                : Convert.ToBase64String(this.SerializedPayload);

            return result;
        }

        /// <inheritdoc />
        public override byte[] GetSerializedPayloadAsEncodedBytes()
        {
            var result = this.SerializedPayload;

            return result;
        }
    }
}