// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamReadWriteProtocols{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using Naos.Recipes.RunWithRetry;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.String.Recipes;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// File system implementation of <see cref="IStreamReadProtocols{TId,TObject}"/>
    /// and <see cref="IStreamWriteProtocols{TId,TObject}"/>.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public partial class FileStreamReadWriteProtocols<TId, TObject>
        : IStreamReadProtocols<TId, TObject>,
          IStreamWriteProtocols<TId, TObject>
    {
        private readonly FileReadWriteStream stream;
        private readonly ISyncAndAsyncReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator> resourceLocatorUsingIdProtocols;
        private readonly ISerializer serializer;
        private readonly SerializerRepresentation serializerRepresentation;
        private readonly string binaryFileExtension = "bin";

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStreamReadWriteProtocols{TId,TObject}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public FileStreamReadWriteProtocols(
            FileReadWriteStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;

            this.resourceLocatorUsingIdProtocols = this.stream.ResourceLocatorProtocols.GetResourceLocatorByIdProtocol<TId>();
            this.serializerRepresentation = this.stream.DefaultSerializerRepresentation;
            this.serializer = this.stream.SerializerFactory.BuildSerializer(this.serializerRepresentation);
        }

        /// <inheritdoc />
        public TObject Execute(
            GetLatestByIdAndTypeOp<TId, TObject> operation)
        {
            var task = this.ExecuteAsync(operation);
            var result = Run.TaskUntilCompletion(task);
            return result;
        }

        /// <inheritdoc />
        public Task<TObject> ExecuteAsync(
            GetLatestByIdAndTypeOp<TId, TObject> operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public long Execute(
            PutAndReturnInternalRecordIdOp<TId, TObject> operation)
        {
            var resourceLocator = this.resourceLocatorUsingIdProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var identifierTypeRepresentation = (operation.Id?.GetType() ?? typeof(TId)).ToRepresentation();
            var objectTypeRepresentation = (operation.ObjectToPut?.GetType() ?? typeof(TObject)).ToRepresentation();

            // lock current max record id file and update
            // generate file name to be used of each file

            var stringSerializedId = this.serializer.SerializeToString(operation.Id);
            var streamRecordMetadata = new StreamRecordMetadata(
                stringSerializedId,
                this.serializerRepresentation,
                identifierTypeRepresentation.ToWithAndWithoutVersion(),
                objectTypeRepresentation.ToWithAndWithoutVersion(),
                operation.Tags,
                DateTime.UtcNow);

            var stringSerializedMetadata = this.serializer.SerializeToString(streamRecordMetadata);
            byte[] serializedBytes = null;
            string serializedString = null;
            string fileExtension = null;
            switch (this.stream.DefaultSerializationFormat)
            {
                case SerializationFormat.String:
                    serializedString = this.serializer.SerializeToString(operation.ObjectToPut);
                    fileExtension = this.serializerRepresentation.SerializationKind.ToString().ToLowerFirstCharacter();
                    break;
                case SerializationFormat.Binary:
                    serializedBytes = this.serializer.SerializeToBytes(operation.ObjectToPut);
                    fileExtension = this.binaryFileExtension;
                    break;
                default:
                    throw new NotSupportedException(Invariant($"{nameof(SerializationFormat)} from {nameof(this.stream)} of '{this.stream.DefaultSerializationFormat}' is not supported."));
            }

            //write meta to metdata file path
            //write payload to correct file path

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<long> ExecuteAsync(
            PutAndReturnInternalRecordIdOp<TId, TObject> operation)
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
            await Task.FromResult(true);
        }
    }
}
