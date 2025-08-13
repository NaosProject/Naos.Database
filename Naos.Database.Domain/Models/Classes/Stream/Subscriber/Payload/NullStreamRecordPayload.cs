// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullStreamRecordPayload.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// Null-object pattern implementation of a <see cref="StreamRecordPayloadBase"/>.
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class NullStreamRecordPayload : StreamRecordPayloadBase, IModelViaCodeGen
    {
        /// <inheritdoc />
        public override SerializationFormat GetSerializationFormat()
        {
            var result = SerializationFormat.Null;

            return result;
        }

        /// <inheritdoc />
        public override string GetSerializedPayloadAsEncodedString()
        {
            return null;
        }

        /// <inheritdoc />
        public override byte[] GetSerializedPayloadAsEncodedBytes()
        {
            return null;
        }
    }
}
