// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStream{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.Memory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    public class MemoryStreamObjectOperationsProtocol<TId, TObject> :
        IProtocolStreamObjectReadOperations<TId, TObject>,
        IProtocolStreamObjectWriteOperations<TId, TObject>
    {
        private readonly MemoryStream stream;
        private readonly GetTagsFromObjectProtocol<TObject> getTagsFromObjectProtocol;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStreamObjectOperationsProtocol{TId, TObject}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public MemoryStreamObjectOperationsProtocol(
            MemoryStream stream)
        {
            this.stream = stream;
            this.getTagsFromObjectProtocol = new GetTagsFromObjectProtocol<TObject>();
        }

        /// <inheritdoc />
        public override ISyncAndAsyncReturningProtocol<GetLatestByIdAndTypeOp<TId, TObject>, TObject> BuildGetLatestByIdAndTypeProtocol<TObject>()
        {
            var result = new LambdaReturningProtocol<GetLatestByIdAndTypeOp<TId, TObject>, TObject>(
                operation =>
                {
                    var typeRepresentationToMatch = operation.TypeVersionMatchStrategy == TypeVersionMatchStrategy.Any
                        ? typeof(TObject).ToRepresentation().RemoveAssemblyVersions()
                        : typeof(TObject).ToRepresentation();

                    var item = this
                              .items.OrderByDescending(_ => _.Item1.TimestampUtc)
                              .FirstOrDefault(
                                   _ => (operation.TypeVersionMatchStrategy == TypeVersionMatchStrategy.Any
                                            ? _.Item1.TypeRepresentationWithoutVersion == typeRepresentationToMatch
                                            : _.Item1.TypeRepresentationWithVersion == typeRepresentationToMatch)
                                     && _.Item1.Id.Equals(operation.Id));

                    var resultItem = item?.Item2?.DeserializePayloadUsingSpecificFactory(this.stream.SerializerFactory);
                    return (TObject)resultItem;
                });

            return result;
        }

        /// <inheritdoc />
        public TObject Execute(
            GetLatestByIdAndTypeOp<TId, TObject> operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<TObject> ExecuteAsync(
            GetLatestByIdAndTypeOp<TId, TObject> operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Execute(
            PutOp<TObject> operation)
        {

        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PutOp<TObject> operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await
        }

        /// <inheritdoc />
        public long Execute(
            PutAndReturnStreamInternalObjectIdOp<TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            string serializedStringId = null;
            var serializer = this.stream.SerializerFactory.BuildSerializer(this.stream.DefaultSerializerRepresentation);
            if (operation.ObjectToPut is IIdentifiableBy<Guid> guidId)
            {
                serializedStringId = serializer.SerializeToString(guidId);
            }

            var operationForTags = new GetTagsFromObjectOp<TObject>(operation.ObjectToPut);
            var tags = this.getTagsFromObjectProtocol.Execute(operationForTags);
            var objectTypeRep = (operation.ObjectToPut?.GetType() ?? typeof(TObject)).ToRepresentation();

            var describedSerialization = operation.ObjectToPut.ToDescribedSerializationUsingSpecificFactory(
                this.stream.DefaultSerializerRepresentation,
                this.stream.SerializerFactory,
                this.stream.DefaultSerializationFormat);

            var metadata = new StreamRecordMetadata(
                serializedStringId,
                new TypeRepresentationWithAndWithoutVersion(idTypeRep),
                new TypeRepresentationWithAndWithoutVersion(objectTypeRep),
                tags,
                DateTime.UtcNow);
            var result = this.stream.AddItem(metadata, describedSerialization);
            return result;
        }

        /// <inheritdoc />
        public async Task<long> ExecuteAsync(
            PutAndReturnStreamInternalObjectIdOp<TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}
