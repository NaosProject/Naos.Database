// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamReadWriteWithIdProtocols{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.String.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// File system implementation of <see cref="IStreamReadWithIdProtocols{TId,TObject}"/>
    /// and <see cref="IStreamWriteWithIdProtocols{TId,TObject}"/>.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class FileStreamReadWriteWithIdProtocols<TId, TObject>
        : IStreamReadWithIdProtocols<TId, TObject>,
          IStreamWriteWithIdProtocols<TId, TObject>
    {
        private readonly FileReadWriteStream stream;
        private readonly ISyncAndAsyncReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator> locatorProtocols;
        private readonly IStreamReadProtocols delegatedProtocols;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStreamReadWriteWithIdProtocols{TId,TObject}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public FileStreamReadWriteWithIdProtocols(
            FileReadWriteStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
            this.delegatedProtocols = stream.GetStreamReadingProtocols();

            this.locatorProtocols = this.stream.ResourceLocatorProtocols.GetResourceLocatorByIdProtocol<TId>();
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public TObject Execute(
            GetLatestObjectByIdOp<TId, TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var resourceLocator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));
            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);
            var stringSerializedId = this.ConvertIdToString(operation.Id, serializer);
            var delegatedOperation = new GetLatestRecordByIdOp(
                resourceLocator,
                stringSerializedId,
                typeof(TId).ToRepresentation().ToWithAndWithoutVersion(),
                typeof(TObject).ToRepresentation().ToWithAndWithoutVersion(),
                operation.TypeVersionMatchStrategy);
            var record = this.stream.Execute(delegatedOperation);
            var result = record.Payload.DeserializePayloadUsingSpecificFactory<TObject>(this.stream.SerializerFactory);
            return result;
        }

        /// <inheritdoc />
        public async Task<TObject> ExecuteAsync(
            GetLatestObjectByIdOp<TId, TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "fileStream is only disposed once.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        public long Execute(
            PutWithIdAndReturnInternalRecordIdOp<TId, TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var identifierTypeRepresentation = (operation.Id?.GetType() ?? typeof(TId)).ToRepresentation();
            var objectTypeRepresentation = (operation.ObjectToPut?.GetType() ?? typeof(TObject)).ToRepresentation();

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);
            var objectTimestamp = operation.ObjectToPut is IHaveTimestampUtc objectWithTimestamp
                ? objectWithTimestamp.TimestampUtc
                : (DateTime?)null;
            var stringSerializedId = this.ConvertIdToString(operation.Id, serializer);
            var metadata = new StreamRecordMetadata(
                stringSerializedId,
                this.stream.DefaultSerializerRepresentation,
                identifierTypeRepresentation.ToWithAndWithoutVersion(),
                objectTypeRepresentation.ToWithAndWithoutVersion(),
                operation.Tags,
                DateTime.UtcNow,
                objectTimestamp);

            var payload = operation.ObjectToPut.ToDescribedSerializationUsingSpecificSerializer(
                serializer,
                this.stream.DefaultSerializationFormat);

            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));
            var result = this.stream.Execute(new PutRecordOp(locator, metadata, payload));
            return result;
        }

        private string ConvertIdToString(
            TId identifier,
            ISerializer serializer)
        {
            var identifierType = identifier?.GetType() ?? typeof(TId);
            if (identifierType == typeof(string)
             || identifierType == typeof(long)
             || identifierType == typeof(int)
             || identifierType == typeof(short))
            {
                return identifier?.ToString();
            }
            else
            {
                var result = serializer.SerializeToString(identifier);
                return result;
            }
        }

        /// <inheritdoc />
        public async Task<long> ExecuteAsync(
            PutWithIdAndReturnInternalRecordIdOp<TId, TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public void Execute(
            PutWithIdOp<TId, TObject> operation)
        {
            var delegatedOperation = new PutWithIdAndReturnInternalRecordIdOp<TId, TObject>(operation.Id, operation.ObjectToPut, operation.Tags);
            this.Execute(delegatedOperation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PutWithIdOp<TId, TObject> operation)
        {
            this.Execute(operation);
            await Task.FromResult(true);
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId, TObject> Execute(
            GetLatestRecordByIdOp<TId, TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var recordSerializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);
            var serializedObjectId = recordSerializer.SerializeToString(operation.Id);
            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var delegatedOperation = new GetLatestRecordByIdOp(
                locator,
                serializedObjectId,
                typeof(TId).ToRepresentation().ToWithAndWithoutVersion(),
                typeof(TObject).ToRepresentation().ToWithAndWithoutVersion(),
                operation.TypeVersionMatchStrategy);

            var record = this.delegatedProtocols.Execute(delegatedOperation);

            var metadata = new StreamRecordMetadata<TId>(
                operation.Id,
                record.Metadata.SerializerRepresentation,
                record.Metadata.TypeRepresentationOfId,
                record.Metadata.TypeRepresentationOfObject,
                record.Metadata.Tags,
                record.Metadata.TimestampUtc,
                record.Metadata.ObjectTimestampUtc);

            var payload = record.Payload.DeserializePayloadUsingSpecificFactory<TObject>(this.stream.SerializerFactory);

            var result = new StreamRecordWithId<TId, TObject>(record.InternalRecordId, metadata, payload);
            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecordWithId<TId, TObject>> ExecuteAsync(
            GetLatestRecordByIdOp<TId, TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}
