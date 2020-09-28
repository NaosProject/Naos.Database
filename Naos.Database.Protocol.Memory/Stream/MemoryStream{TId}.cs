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
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// In memory implementation of <see cref="IStream{TId}"/>.
    /// </summary>
    /// <typeparam name="TId">The type of the ID.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public class MemoryStream<TId> : IStream<TId>, IProtocolResourceLocator<TId>
    {
        private readonly SerializerRepresentation defaultSerializerRepresentation;
        private readonly SerializationFormat defaultSerializationFormat;
        private readonly ISerializerFactory serializerFactory;
        private readonly ISyncAndAsyncReturningProtocol<GetProtocolByTypeOp, IProtocol> getProtocolByTypeProtocol;
        private readonly IList<MemoryRecord<TId>> items = new List<MemoryRecord<TId>>();
        private readonly NullResourceLocator nullResourceLocator = new NullResourceLocator();

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStream{TId}"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultSerializerRepresentation">The default serializer representation.</param>
        /// <param name="defaultSerializationFormat">The default serialization format.</param>
        /// <param name="serializerFactory">The serializer factory.</param>
        /// <param name="getProtocolByTypeProtocol">The protocol to get, by type, other protocols.</param>
        public MemoryStream(
            string name,
            SerializerRepresentation defaultSerializerRepresentation,
            SerializationFormat defaultSerializationFormat,
            ISerializerFactory serializerFactory,
            ISyncAndAsyncReturningProtocol<GetProtocolByTypeOp, IProtocol> getProtocolByTypeProtocol = null)
        {
            this.defaultSerializerRepresentation = defaultSerializerRepresentation ?? throw new ArgumentNullException(nameof(defaultSerializerRepresentation));

            if (defaultSerializationFormat == SerializationFormat.Invalid)
            {
                throw new ArgumentException(Invariant($"Cannot specify a {nameof(SerializationFormat)} of {SerializationFormat.Invalid}."));
            }

            this.defaultSerializationFormat = defaultSerializationFormat;
            this.serializerFactory = serializerFactory ?? throw new ArgumentNullException(nameof(serializerFactory));
            this.Name = name;
            this.getProtocolByTypeProtocol = getProtocolByTypeProtocol ?? new ProtocolFactory(new Dictionary<Type, Func<IProtocol>>());
        }

        /// <inheritdoc />
        public ISyncAndAsyncReturningProtocol<GetTagsFromObjectOp<TObject>, IReadOnlyDictionary<string, string>>
            BuildGetTagsFromObjectProtocol<TObject>()
        {
            var operation = new GetProtocolByTypeOp(
                typeof(ISyncAndAsyncReturningProtocol<GetTagsFromObjectOp<TObject>, IReadOnlyDictionary<string, string>>).ToRepresentation(),
                MissingProtocolStrategy.Null);
            var result = this.getProtocolByTypeProtocol.Execute(operation);
            if (result != null)
            {
                return (ISyncAndAsyncReturningProtocol<GetTagsFromObjectOp<TObject>, IReadOnlyDictionary<string, string>>)result;
            }
            else
            {
                return new GetTagsFromObjectProtocol<TObject>();
            }
        }

        /// <inheritdoc />
        public ISyncAndAsyncReturningProtocol<GetIdFromObjectOp<TId, TObject>, TId> BuildGetIdFromObjectProtocol<TObject>()
        {
            var operation = new GetProtocolByTypeOp(
                typeof(ISyncAndAsyncReturningProtocol<GetIdFromObjectOp<TId, TObject>, TId>).ToRepresentation(),
                MissingProtocolStrategy.Null);
            var result = this.getProtocolByTypeProtocol.Execute(operation);
            if (result != null)
            {
                return (ISyncAndAsyncReturningProtocol<GetIdFromObjectOp<TId, TObject>, TId>)result;
            }
            else
            {
                return new GetIdFromObjectProtocol<TId, TObject>();
            }
        }

        /// <inheritdoc />
        public ISyncAndAsyncReturningProtocol<GetLatestByIdAndTypeOp<TId, TObject>, TObject> BuildGetLatestByIdAndTypeProtocol<TObject>()
        {
            var result = new LambdaReturningProtocol<GetLatestByIdAndTypeOp<TId, TObject>, TObject>(
                operation =>
                {
                    var typeRepresentation = typeof(TObject).ToRepresentation();
                    var item = this.items.OrderByDescending(_ => _.DateTimeUtc).FirstOrDefault(_ => _.DescribedSerialization.PayloadTypeRepresentation == typeRepresentation && _.Id.Equals(operation.Id));
                    var resultItem = item?.DescribedSerialization.DeserializePayloadUsingSpecificFactory(this.serializerFactory);
                    return (TObject)resultItem;
                });

            return result;
        }

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public IProtocolResourceLocator<TId> ResourceLocatorProtocol => this;

        /// <inheritdoc />
        public StreamRepresentation<TId> StreamRepresentation => new StreamRepresentation<TId>(this.Name);

        /// <inheritdoc />
        public void Execute(
            CreateStreamOp<TId> operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (operation.StreamRepresentation != this.StreamRepresentation)
            {
                throw new ArgumentException(Invariant($"This {nameof(MemoryStream<TId>)} can only 'create' a stream with the same representation."));
            }

            // nothing else to do...
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            CreateStreamOp<TId> operation)
        {
            var resultNotUsed = await Task.FromResult(false); // just to get the async.
            this.Execute(operation);
        }

        /// <inheritdoc />
        public ISyncAndAsyncVoidProtocol<PutOp<TObject>> BuildPutProtocol<TObject>()
        {
            void Action(PutOp<TObject> operation)
            {
                var operationForId = new GetIdFromObjectOp<TId, TObject>(operation.ObjectToPut);
                var id = this.BuildGetIdFromObjectProtocol<TObject>().Execute(operationForId);
                var describedSerialization = operation.ObjectToPut.ToDescribedSerializationUsingSpecificFactory(
                    this.defaultSerializerRepresentation,
                    this.serializerFactory,
                    this.defaultSerializationFormat);
                var memoryRecord = new MemoryRecord<TId>(id, describedSerialization, DateTime.UtcNow);
                this.items.Add(memoryRecord);
            }

            var result = new LambdaVoidProtocol<PutOp<TObject>>(Action);
            return result;
        }

        /// <inheritdoc />
        public IResourceLocator Execute(
            GetResourceLocatorByIdOp<TId> operation)
        {
            return this.nullResourceLocator;
        }

        /// <inheritdoc />
        public async Task<IResourceLocator> ExecuteAsync(
            GetResourceLocatorByIdOp<TId> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<IResourceLocator> Execute(
            GetAllResourceLocatorsOp operation)
        {
            var result = new[]
                         {
                             this.nullResourceLocator,
                         };

            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<IResourceLocator>> ExecuteAsync(
            GetAllResourceLocatorsOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}
