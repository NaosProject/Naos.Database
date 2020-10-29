﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamObjectOperationsProtocol{TId,TObject}.cs" company="Naos Project">
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
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// File system implementation of <see cref="IProtocolStreamObjectReadOperations{TId,TObject}"/>
    /// and <see cref="IProtocolStreamObjectWriteOperations{TId,TObject}"/>.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public partial class FileStreamObjectOperationsProtocol<TId, TObject>
        : IProtocolStreamObjectReadOperations<TId, TObject>,
          IProtocolStreamObjectWriteOperations<TId, TObject>
    {
        private const string FileExtensionPrefixWithDot = ".value";
        private const string BinaryFileExtensionWithDot = ".value.bin";
        private const string StringFileExtensionWithDot = ".value.str";
        private const string MetadataFileExtensionWithDot = ".metadata";
        private readonly FileStream<TId> stream;
        private readonly IReturningProtocol<GetIdFromObjectOp<TId, TObject>, TId> getIdFromObjectProtocol;
        private readonly IReturningProtocol<GetTagsFromObjectOp<TObject>, IReadOnlyDictionary<string, string>> getTagsFromObjectProtocol;
        private ObcDateTimeStringSerializer dateTimeStringSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStreamObjectOperationsProtocol{TId, TObject}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public FileStreamObjectOperationsProtocol(
            FileStream<TId> stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
            this.getIdFromObjectProtocol = this.stream.BuildGetIdFromObjectProtocol<TObject>();
            this.getTagsFromObjectProtocol = this.stream.BuildGetTagsFromObjectProtocol<TObject>();
            this.dateTimeStringSerializer = new ObcDateTimeStringSerializer();
        }

        /// <inheritdoc />
        public TObject Execute(
            GetLatestByIdAndTypeOp<TId, TObject> operation)
        {
            var objectId = operation.Id;
            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);
            var serializedObjectId = (objectId is string stringKey) ? stringKey : serializer.SerializeToString(objectId);
            var rootPath = this.GetRootPath(objectId);
            // need to sort descending by time with the id search pattern
            var id = serializedObjectId.EncodeForFilePath();
            var matchingFiles = Directory.GetFiles(
                rootPath,
                Invariant($"*{id}*{FileExtensionPrefixWithDot}*"),
                SearchOption.TopDirectoryOnly);

            var match = matchingFiles.OrderByDescending(Path.GetFileName).FirstOrDefault();

            TObject result = default;
            if (match != null)
            {
                if (match.EndsWith(StringFileExtensionWithDot, StringComparison.Ordinal))
                {
                    var str = File.ReadAllText(match);
                    result = serializer.Deserialize<TObject>(str);
                }
                else if (match.EndsWith(BinaryFileExtensionWithDot, StringComparison.Ordinal))
                {
                    var bytes = File.ReadAllBytes(match);
                    result = serializer.Deserialize<TObject>(bytes);
                }
                else
                {
                    throw new InvalidOperationException(Invariant($"Unsupported file extension for the record value; path: '{match}'."));
                }
            }

            return result;
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
            PutOp<TObject> operation)
        {
            var timestamp = DateTime.UtcNow;
            var timestampString = this.dateTimeStringSerializer.SerializeToString(timestamp).Replace(":", "-");
            var id = this.getIdFromObjectProtocol.Execute(new GetIdFromObjectOp<TId, TObject>(operation.ObjectToPut));
            var typeRepresentationWithVersion = (operation.ObjectToPut?.GetType() ?? typeof(TObject)).ToRepresentation();
            var typeRepresentationWithoutVersion = typeRepresentationWithVersion.RemoveAssemblyVersions();
            var tags = this.getTagsFromObjectProtocol.Execute(new GetTagsFromObjectOp<TObject>(operation.ObjectToPut));

            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);
            var serializedObjectString = this.stream.DefaultSerializationFormat != SerializationFormat.String
                ? null
                : serializer.SerializeToString(operation.ObjectToPut);
            var serializedObjectBinary = this.stream.DefaultSerializationFormat != SerializationFormat.Binary
                ? null
                : serializer.SerializeToBytes(operation.ObjectToPut);
            var serializedId = (id is string stringKey) ? stringKey : serializer.SerializeToString(id);
            var rootPath = this.GetRootPath(id);
            // need to create the timestamp utc and put into path (should be prefixing the filename)
            var filePathId = serializedId.EncodeForFilePath();
            var baseFilePath = Path.Combine(
                rootPath,
                timestampString
              + "___"
              + filePathId);

            var metadata = new StreamRecordMetadata<TId>(
                id,
                tags,
                typeRepresentationWithVersion,
                typeRepresentationWithoutVersion,
                timestamp);

            var objectMetadataFilePath = baseFilePath + MetadataFileExtensionWithDot;
            var objectBinFilePath = baseFilePath + BinaryFileExtensionWithDot;
            var objectStrFilePath = baseFilePath + StringFileExtensionWithDot;

            var serializedMetadata = serializer.SerializeToString(metadata);
            File.WriteAllText(objectMetadataFilePath, serializedMetadata);

            if (serializedObjectBinary != null)
            {
                File.WriteAllBytes(objectBinFilePath, serializedObjectBinary);
            }
            else
            {
                File.WriteAllText(objectStrFilePath, serializedObjectString);
            }
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PutOp<TObject> operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just to get the async.
        }

        private string GetRootPath(
            TId id)
        {
            var locator = this.stream.ResourceLocatorProtocol.Execute(new GetResourceLocatorByIdOp<TId>(id));
            var fileLocator = locator as FileSystemDatabaseLocator ?? throw new InvalidOperationException(Invariant($"Resource locator extracted for ID {id} was expected to be a {nameof(FileSystemDatabaseLocator)} but was a '{locator.GetType().ToStringReadable()}'."));
            var result = Path.Combine(fileLocator.RootFolderPath, this.stream.Name);
            return result;
        }
    }
}