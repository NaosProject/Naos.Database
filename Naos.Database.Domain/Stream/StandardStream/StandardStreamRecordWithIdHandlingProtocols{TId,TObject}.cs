// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamRecordWithIdHandlingProtocols{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Threading.Tasks;
    using Naos.Database.Domain.DescribedSerialization;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// Set of protocols to handle <see cref="IEvent"/>'s in a stream.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    /// <seealso cref="IStreamReadWithIdProtocols{TId,TObject}" />
    /// <seealso cref="IStreamWriteWithIdProtocols{TId,TObject}" />
    public class StandardStreamRecordWithIdHandlingProtocols<TId, TObject> :
        IStreamRecordWithIdHandlingProtocols<TId, TObject>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Temporary.")]
        private readonly IStandardReadWriteStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardStreamRecordWithIdHandlingProtocols{TId, TObject}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public StandardStreamRecordWithIdHandlingProtocols(
            IStandardReadWriteStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();
            this.stream = stream;
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId, TObject> Execute(
            TryHandleRecordWithIdOp<TId, TObject> operation)
        {
            var delegatedOperation = operation.Standardize();
            var tryHandleResult = this.stream.Execute(delegatedOperation);
            var record = tryHandleResult.RecordToHandle;

            if (record?.Payload == null)
            {
                return null;
            }

            var serializer = this.stream.SerializerFactory.BuildSerializer(record.Payload.SerializerRepresentation);
            var payload = record.Payload.DeserializePayloadUsingSpecificSerializer<TObject>(serializer);
            var id = serializer.Deserialize<TId>(record.Metadata.StringSerializedId);
            var metadata = new StreamRecordMetadata<TId>(
                id,
                record.Metadata.SerializerRepresentation,
                record.Metadata.TypeRepresentationOfId,
                record.Metadata.TypeRepresentationOfObject,
                record.Metadata.Tags,
                record.Metadata.TimestampUtc,
                record.Metadata.ObjectTimestampUtc);

            var result = new StreamRecordWithId<TId, TObject>(record.InternalRecordId, metadata, payload);
            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecordWithId<TId, TObject>> ExecuteAsync(
            TryHandleRecordWithIdOp<TId, TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}
