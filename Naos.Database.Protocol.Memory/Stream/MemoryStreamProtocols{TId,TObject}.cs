// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStreamProtocols{TId,TObject}.cs" company="Naos Project">
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
    /// Implements the <see cref="Naos.Database.Domain.IStreamReadingProtocols{TObject}" />
    /// Implements the <see cref="Naos.Database.Domain.IStreamWritingProtocols{TObject}" />
    /// Implements the <see cref="Naos.Database.Domain.IStreamReadingProtocols{TId, TObject}" />
    /// Implements the <see cref="Naos.Database.Domain.IStreamWritingProtocols{TId, TObject}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the t identifier.</typeparam>
    /// <typeparam name="TObject">The type of the t object.</typeparam>
    /// <seealso cref="Naos.Database.Domain.IStreamReadingProtocols{TObject}" />
    /// <seealso cref="Naos.Database.Domain.IStreamWritingProtocols{TObject}" />
    /// <seealso cref="Naos.Database.Domain.IStreamReadingProtocols{TId, TObject}" />
    /// <seealso cref="Naos.Database.Domain.IStreamWritingProtocols{TId, TObject}" />
    public partial class MemoryStreamProtocols<TId, TObject> :
        IStreamReadingProtocols<TId, TObject>,
        IStreamWritingProtocols<TId, TObject>
    {
        private readonly MemoryStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStreamProtocols{TId, TObject}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public MemoryStreamProtocols(
            MemoryStream stream)
        {
            this.stream = stream;
        }

        /// <inheritdoc />
        public TObject Execute(
            GetLatestByIdAndTypeOp<TId, TObject> operation)
        {
            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);
            var serializedObjectId = serializer.SerializeToString(operation.Id);
            var typeRepresentationToMatch = operation.TypeVersionMatchStrategy == TypeVersionMatchStrategy.Any
                ? typeof(TObject).ToRepresentation().RemoveAssemblyVersions()
                : typeof(TObject).ToRepresentation();

            var item = this
                      .stream.GetItems().OrderByDescending(_ => _.Metadata.TimestampUtc)
                      .FirstOrDefault(
                           _ => (operation.TypeVersionMatchStrategy                        == TypeVersionMatchStrategy.Any
                                    ? _.Metadata.TypeRepresentationOfObject.WithoutVersion == typeRepresentationToMatch
                                    : _.Metadata.TypeRepresentationOfObject.WithVersion    == typeRepresentationToMatch)
                             && _.Metadata.StringSerializedObjectId.Equals(serializedObjectId));

            var resultItem = item?.Payload?.DeserializePayloadUsingSpecificFactory(this.stream.SerializerFactory);
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
            var chainOperation = new PutAndReturnInternalRecordIdOp<TId, TObject>(operation.Id, operation.ObjectToPut, operation.Tags);
            this.Execute(chainOperation);
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

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);
            string serializedStringId = serializer.SerializeToString(operation.Id);

            var identifierTypeRep = (operation.Id?.GetType() ?? typeof(TId)).ToRepresentation();
            var objectTypeRep = (operation.ObjectToPut?.GetType() ?? typeof(TObject)).ToRepresentation();

            var describedSerialization = operation.ObjectToPut.ToDescribedSerializationUsingSpecificFactory(
                this.stream.DefaultSerializerRepresentation,
                this.stream.SerializerFactory,
                this.stream.DefaultSerializationFormat);

            var metadata = new StreamRecordMetadata(
                serializedStringId,
                identifierTypeRep.ToWithAndWithoutVersion(),
                objectTypeRep.ToWithAndWithoutVersion(),
                operation.Tags ?? new Dictionary<string, string>(),
                DateTime.UtcNow);
            var result = this.stream.AddItem(metadata, describedSerialization);
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
