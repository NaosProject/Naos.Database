// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamReadWriteWithIdProtocols{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Set of protocols to work with known identifier and/or object type.
    /// Implements the <see cref="IStreamReadWithIdProtocols{TId}" />
    /// Implements the <see cref="IStreamWriteProtocols{TId}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <seealso cref="IStreamReadWithIdProtocols{TId}" />
    /// <seealso cref="IStreamWriteWithIdProtocols{TId}" />
    public class StandardStreamReadWriteWithIdProtocols<TId> :
        IStreamReadWithIdProtocols<TId>,
        IStreamWriteWithIdProtocols<TId>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "temp")]
        private readonly IStandardReadWriteStream stream;
        private readonly StandardStreamReadWriteProtocols delegatedProtocols;
        private readonly ISyncAndAsyncReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator> locatorProtocols;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardStreamReadWriteWithIdProtocols{TId}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public StandardStreamReadWriteWithIdProtocols(
            IStandardReadWriteStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
            this.delegatedProtocols = new StandardStreamReadWriteProtocols(stream);
            this.locatorProtocols = stream.ResourceLocatorProtocols.GetResourceLocatorByIdProtocol<TId>();
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId> Execute(
            GetLatestRecordByIdOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);
            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var delegatedOperation = operation.Standardize(serializer, locator);
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

        /// <inheritdoc />
        public bool Execute(
            DoesAnyExistByIdOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);
            var serializedObjectId = serializer.SerializeToString(operation.Id);
            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));
            var delegatedOperation = new DoesAnyExistByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                operation.ObjectType,
                operation.VersionMatchStrategy,
                locator);

            var result = this.delegatedProtocols.Execute(delegatedOperation);
            return result;
        }

        /// <inheritdoc />
        public async Task<bool> ExecuteAsync(
            DoesAnyExistByIdOp<TId> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordWithId<TId>> Execute(
            GetAllRecordsByIdOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);
            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var delegatedOperation = operation.Standardize(serializer, locator);
            var records = this.delegatedProtocols.Execute(delegatedOperation);

            var result = records?.Select(
                                      _ =>
                                      {
                                          var metadata = new StreamRecordMetadata<TId>(
                                              operation.Id,
                                              _.Metadata.SerializerRepresentation,
                                              _.Metadata.TypeRepresentationOfId,
                                              _.Metadata.TypeRepresentationOfObject,
                                              _.Metadata.Tags,
                                              _.Metadata.TimestampUtc,
                                              _.Metadata.ObjectTimestampUtc);

                                          var localResult = new StreamRecordWithId<TId>(_.InternalRecordId, metadata, _.Payload);
                                          return localResult;
                                      })
                                 .ToList();

            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecordWithId<TId>>> ExecuteAsync(
            GetAllRecordsByIdOp<TId> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public StreamRecordMetadata<TId> Execute(
            GetLatestRecordMetadataByIdOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);
            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var delegatedOperation = operation.Standardize(serializer, locator);
            var metadata = this.delegatedProtocols.Execute(delegatedOperation);

            if (metadata == null)
            {
                return null;
            }

            var result = new StreamRecordMetadata<TId>(
                operation.Id,
                metadata.SerializerRepresentation,
                metadata.TypeRepresentationOfId,
                metadata.TypeRepresentationOfObject,
                metadata.Tags,
                metadata.TimestampUtc,
                metadata.ObjectTimestampUtc);

            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecordMetadata<TId>> ExecuteAsync(
            GetLatestRecordMetadataByIdOp<TId> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordMetadata<TId>> Execute(
            GetAllRecordsMetadataByIdOp<TId> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);
            var locator = this.locatorProtocols.Execute(new GetResourceLocatorByIdOp<TId>(operation.Id));

            var delegatedOperation = operation.Standardize(serializer, locator);
            var records = this.delegatedProtocols.Execute(delegatedOperation);

            var result = records?.Select(
                                      _ =>
                                      {
                                          var metadata = new StreamRecordMetadata<TId>(
                                              operation.Id,
                                              _.SerializerRepresentation,
                                              _.TypeRepresentationOfId,
                                              _.TypeRepresentationOfObject,
                                              _.Tags,
                                              _.TimestampUtc,
                                              _.ObjectTimestampUtc);
                                          return metadata;
                                      })
                                 .ToList();

            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecordMetadata<TId>>> ExecuteAsync(
            GetAllRecordsMetadataByIdOp<TId> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}
