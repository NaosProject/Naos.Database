// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRecordExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Stream record related extensions.
    /// </summary>
    public static class StreamRecordExtensions
    {
        /// <summary>
        /// Gets the described serialization of the specified stream record.
        /// </summary>
        /// <param name="streamRecord">The stream record.</param>
        /// <returns>
        /// The described serialization of the stream record.
        /// </returns>
        public static DescribedSerializationBase GetDescribedSerialization(
            this StreamRecord streamRecord)
        {
            streamRecord.MustForArg(nameof(streamRecord)).NotBeNull();

            var result = GetDescribedSerialization(
                streamRecord.Metadata.TypeRepresentationOfObject,
                streamRecord.Metadata.SerializerRepresentation,
                streamRecord.Payload);

            return result;
        }

        /// <summary>
        /// Gets the described serialization of the specified stream record.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="streamRecord">The stream record.</param>
        /// <returns>
        /// The described serialization of the stream record.
        /// </returns>
        public static DescribedSerializationBase GetDescribedSerialization<TId>(
            this StreamRecordWithId<TId> streamRecord)
        {
            streamRecord.MustForArg(nameof(streamRecord)).NotBeNull();

            var result = GetDescribedSerialization(
                streamRecord.Metadata.TypeRepresentationOfObject,
                streamRecord.Metadata.SerializerRepresentation,
                streamRecord.Payload);

            return result;
        }

        /// <summary>
        /// Converts the specified <see cref="DescribedSerializationBase"/> to a <see cref="StreamRecordPayloadBase"/>.
        /// </summary>
        /// <param name="describedSerialization">The stream record.</param>
        /// <returns>
        /// The described serialization of the stream record.
        /// </returns>
        public static StreamRecordPayloadBase ToStreamRecordPayload(
            this DescribedSerializationBase describedSerialization)
        {
            describedSerialization.MustForArg(nameof(describedSerialization)).NotBeNull();

            StreamRecordPayloadBase result;

            if (describedSerialization is StringDescribedSerialization stringDescribedSerialization)
            {
                result = new StringStreamRecordPayload(stringDescribedSerialization.SerializedPayload);
            }
            else if (describedSerialization is BinaryDescribedSerialization binaryDescribedSerialization)
            {
                result = new BinaryStreamRecordPayload(binaryDescribedSerialization.SerializedPayload);
            }
            else if (describedSerialization is NullDescribedSerialization nullDescribedSerialization)
            {
                result = new NullStreamRecordPayload();
            }
            else
            {
                throw new NotSupportedException(Invariant($"This type of {nameof(DescribedSerializationBase)} is not supported: {describedSerialization.GetType().ToStringReadable()}."));
            }

            return result;
        }

        private static DescribedSerializationBase GetDescribedSerialization(
            TypeRepresentationWithAndWithoutVersion typeRepresentationOfObject,
            SerializerRepresentation serializerRepresentation,
            StreamRecordPayloadBase payload)
        {
            DescribedSerializationBase result;

            var payloadTypeRepresentation = typeRepresentationOfObject.WithVersion;

            if (payload is StringStreamRecordPayload stringStreamRecordPayload)
            {
                result = new StringDescribedSerialization(
                    payloadTypeRepresentation,
                    serializerRepresentation,
                    stringStreamRecordPayload.SerializedPayload);
            }
            else if (payload is BinaryStreamRecordPayload binaryStreamRecordPayload)
            {
                result = new BinaryDescribedSerialization(
                    payloadTypeRepresentation,
                    serializerRepresentation,
                    binaryStreamRecordPayload.SerializedPayload);
            }
            else if (payload is NullStreamRecordPayload)
            {
                result = new NullDescribedSerialization(
                    payloadTypeRepresentation,
                    serializerRepresentation);
            }
            else
            {
                throw new NotSupportedException(Invariant($"This type of {nameof(StreamRecordPayloadBase)} is not supported: {payload.GetType().ToStringReadable()}."));
            }

            return result;
        }
    }
}
