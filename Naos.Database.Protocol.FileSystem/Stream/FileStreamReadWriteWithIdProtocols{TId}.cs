// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamReadWriteWithIdProtocols{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System;
    using System.Threading.Tasks;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;

    /// <summary>
    /// File system implementation of <see cref="IStreamReadWithIdProtocols{TId}"/>
    /// and <see cref="IStreamWriteProtocols{TId}"/>.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    public class FileStreamReadWriteWithIdProtocols<TId>
        : IStreamReadWithIdProtocols<TId>,
          IStreamWriteWithIdProtocols<TId>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "temp")]
        private readonly FileReadWriteStream stream;

        private readonly IStreamReadProtocols delegatedProtocols;
        private readonly ISyncAndAsyncReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator> locatorProtocols;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStreamReadWriteWithIdProtocols{TId}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public FileStreamReadWriteWithIdProtocols(FileReadWriteStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
            this.delegatedProtocols = stream.GetStreamReadingProtocols();
            this.locatorProtocols = stream.ResourceLocatorProtocols.GetResourceLocatorByIdProtocol<TId>();
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId> Execute(
            GetLatestRecordByIdOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);
            var serializedObjectId = serializer.SerializeToString(operation.Id);
            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var delegatedOperation = new GetLatestRecordByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                operation.ObjectType,
                operation.TypeVersionMatchStrategy,
                locator);

            var record = this.delegatedProtocols.Execute(delegatedOperation);

            if (record == null)
            {
                return null;
            }

            var metadata = new StreamRecordMetadata<TId>(
                operation.Id,
                record.Metadata.SerializerRepresentation,
                record.Metadata.TypeRepresentationOfId,
                record.Metadata.TypeRepresentationOfObject,
                record.Metadata.Tags,
                record.Metadata.TimestampUtc,
                record.Metadata.ObjectTimestampUtc);

            var result = new StreamRecordWithId<TId>(record.InternalRecordId, metadata, record.Payload);
            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecordWithId<TId>> ExecuteAsync(
            GetLatestRecordByIdOp<TId> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}
