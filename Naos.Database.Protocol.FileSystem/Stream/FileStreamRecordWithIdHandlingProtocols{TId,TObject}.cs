// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamRecordWithIdHandlingProtocols{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using Naos.Recipes.RunWithRetry;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// File system implementation of <see cref="IStreamRecordWithIdHandlingProtocols{TId, TObject}"/>.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class FileStreamRecordWithIdHandlingProtocols<TId, TObject>
        : IStreamRecordWithIdHandlingProtocols<TId, TObject>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Temporary.")]
        private readonly FileReadWriteStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStreamRecordWithIdHandlingProtocols{TId, TObject}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public FileStreamRecordWithIdHandlingProtocols(FileReadWriteStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId, TObject> Execute(
            TryHandleRecordWithIdOp<TId, TObject> operation)
        {
            var delegatedOperation = new TryHandleRecordOp(
                operation.Concern,
                typeof(TId).ToRepresentation().ToWithAndWithoutVersion(),
                typeof(TObject).ToRepresentation().ToWithAndWithoutVersion(),
                operation.TypeVersionMatchStrategy,
                operation.SpecifiedResourceLocator,
                operation.Tags);
            var record = this.stream.Execute(delegatedOperation);

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
