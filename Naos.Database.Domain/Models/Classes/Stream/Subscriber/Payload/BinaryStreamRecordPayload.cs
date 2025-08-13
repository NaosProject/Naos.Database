// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryStreamRecordPayload.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// The binary payload of a stream record
    /// (the serialized object that is persisted or being persisted).
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class BinaryStreamRecordPayload : StreamRecordPayloadBase, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryStreamRecordPayload"/> class.
        /// </summary>
        /// <param name="serializedPayload">The serialized payload.</param>
        public BinaryStreamRecordPayload(
            byte[] serializedPayload)
        {
            this.SerializedPayload = serializedPayload;
        }

        /// <summary>
        /// Gets the serialized payload.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = NaosSuppressBecause.CA1819_PropertiesShouldNotReturnArrays_DataPayloadsAreCommonlyRepresentedAsByteArrays)]
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
