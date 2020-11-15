// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStreamReadWriteProtocols{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.Memory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// Set of protocols to work with known identifier and/or object type.
    /// Implements the <see cref="IStreamReadProtocols{TObject}" />
    /// Implements the <see cref="IStreamWriteProtocols{TObject}" />
    /// Implements the <see cref="IStreamReadProtocols{TId,TObject}" />
    /// Implements the <see cref="IStreamWriteProtocols{TId,TObject}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the t identifier.</typeparam>
    /// <typeparam name="TObject">The type of the t object.</typeparam>
    /// <seealso cref="IStreamReadProtocols{TObject}" />
    /// <seealso cref="IStreamWriteProtocols{TObject}" />
    /// <seealso cref="IStreamReadProtocols{TId,TObject}" />
    /// <seealso cref="IStreamWriteProtocols{TId,TObject}" />
    public partial class MemoryStreamReadWriteProtocols<TId, TObject> :
        IStreamReadProtocols<TId, TObject>,
        IStreamWriteProtocols<TId, TObject>
    {
        private readonly MemoryReadWriteStream readWriteStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStreamReadWriteProtocols{TId,TObject}"/> class.
        /// </summary>
        /// <param name="readWriteStream">The stream.</param>
        public MemoryStreamReadWriteProtocols(
            MemoryReadWriteStream readWriteStream)
        {
            this.readWriteStream = readWriteStream;
        }

        /// <inheritdoc />
        public TObject Execute(
            GetLatestByIdAndTypeOp<TId, TObject> operation)
        {
            var serializer = this.readWriteStream.SerializerFactory.BuildSerializer(this.readWriteStream.DefaultSerializerRepresentation);
            var serializedObjectId = serializer.SerializeToString(operation.Id);
            var typeRepresentationToMatch = operation.TypeVersionMatchStrategy == TypeVersionMatchStrategy.Any
                ? typeof(TObject).ToRepresentation().RemoveAssemblyVersions()
                : typeof(TObject).ToRepresentation();

            var item = this
                      .readWriteStream.GetAllItems().OrderByDescending(_ => _.Metadata.TimestampUtc)
                      .FirstOrDefault(
                           _ => (operation.TypeVersionMatchStrategy                        == TypeVersionMatchStrategy.Any
                                    ? _.Metadata.TypeRepresentationOfObject.WithoutVersion == typeRepresentationToMatch
                                    : _.Metadata.TypeRepresentationOfObject.WithVersion    == typeRepresentationToMatch)
                             && _.Metadata.StringSerializedId.Equals(serializedObjectId));

            var resultItem = item?.Payload?.DeserializePayloadUsingSpecificFactory(this.readWriteStream.SerializerFactory);
            return (TObject)resultItem;
        }

        /// <inheritdoc />
        public async Task<TObject> ExecuteAsync(
            GetLatestByIdAndTypeOp<TId, TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public void Execute(
            PutOp<TId, TObject> operation)
        {
            var delegatedOperation = new PutAndReturnInternalRecordIdOp<TId, TObject>(operation.Id, operation.ObjectToPut, operation.Tags);
            this.Execute(delegatedOperation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PutOp<TId, TObject> operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await
        }

        /// <inheritdoc />
        public long Execute(
            PutAndReturnInternalRecordIdOp<TId, TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var serializer = this.readWriteStream.SerializerFactory.BuildSerializer(this.readWriteStream.DefaultSerializerRepresentation);
            string serializedStringId = serializer.SerializeToString(operation.Id);

            var identifierTypeRep = (operation.Id?.GetType() ?? typeof(TId)).ToRepresentation();
            var objectTypeRep = (operation.ObjectToPut?.GetType() ?? typeof(TObject)).ToRepresentation();

            var describedSerialization = operation.ObjectToPut.ToDescribedSerializationUsingSpecificFactory(
                this.readWriteStream.DefaultSerializerRepresentation,
                this.readWriteStream.SerializerFactory,
                this.readWriteStream.DefaultSerializationFormat);

            var metadata = new StreamRecordMetadata(
                serializedStringId,
                this.readWriteStream.DefaultSerializerRepresentation,
                identifierTypeRep.ToWithAndWithoutVersion(),
                objectTypeRep.ToWithAndWithoutVersion(),
                operation.Tags ?? new Dictionary<string, string>(),
                DateTime.UtcNow);
            var result = this.readWriteStream.AddItem(metadata, describedSerialization);
            return result;
        }

        /// <inheritdoc />
        public async Task<long> ExecuteAsync(
            PutAndReturnInternalRecordIdOp<TId, TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}
