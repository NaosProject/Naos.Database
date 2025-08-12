// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringStreamRecordPayload.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// The string payload of a stream record
    /// (the serialized object that is persisted or being persisted).
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class StringStreamRecordPayload : StreamRecordPayloadBase, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringStreamRecordPayload"/> class.
        /// </summary>
        /// <param name="serializedPayload">The serialized payload.</param>
        public StringStreamRecordPayload(
            string serializedPayload)
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
    }
}
