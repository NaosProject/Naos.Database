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
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// In memory implementation of <see cref="IStream{TId}"/>.
    /// </summary>
    /// <typeparam name="TId">The type of the ID.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public class MemoryStream<TId> : StreamBase<TId>
    {
        private readonly SerializerRepresentation defaultSerializerRepresentation;
        private readonly SerializationFormat defaultSerializationFormat;
        private readonly ISerializerFactory serializerFactory;
        private readonly IList<MemoryRecord<TId>> items = new List<MemoryRecord<TId>>();
        private bool created = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStream{TId}"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultSerializerRepresentation">The default serializer representation.</param>
        /// <param name="defaultSerializationFormat">The default serialization format.</param>
        /// <param name="serializerFactory">The serializer factory.</param>
        /// <param name="protocolGetIdByTypeProtocol">Optional, id extractor protocols by type; DEFAULT will look for implementation of <see cref="IIdentifiable"/> on the object written.</param>
        /// <param name="protocolGetTagsByTypeProtocol">Optional tag extractor protocols by type; DEFAULT will look for implementation of <see cref="IHaveTags"/> on the object written.</param>
        public MemoryStream(
            string name,
            SerializerRepresentation defaultSerializerRepresentation,
            SerializationFormat defaultSerializationFormat,
            ISerializerFactory serializerFactory,
            ISyncAndAsyncReturningProtocol<GetProtocolByTypeOp, IProtocol> protocolGetIdByTypeProtocol = null,
            ISyncAndAsyncReturningProtocol<GetProtocolByTypeOp, IProtocol> protocolGetTagsByTypeProtocol = null)
        : base(name, new SingleResourceLocatorProtocol<TId>(new MemoryDatabaseLocator(name)), protocolGetIdByTypeProtocol, protocolGetTagsByTypeProtocol)
        {
            this.defaultSerializerRepresentation = defaultSerializerRepresentation ?? throw new ArgumentNullException(nameof(defaultSerializerRepresentation));

            if (defaultSerializationFormat == SerializationFormat.Invalid)
            {
                throw new ArgumentException(Invariant($"Cannot specify a {nameof(SerializationFormat)} of {SerializationFormat.Invalid}."));
            }

            this.defaultSerializationFormat = defaultSerializationFormat;
            this.serializerFactory = serializerFactory ?? throw new ArgumentNullException(nameof(serializerFactory));
            this.Id = Guid.NewGuid().ToString().ToUpperInvariant();
        }

        /// <inheritdoc />
        public override IStreamRepresentation<TId> StreamRepresentation => new MemoryStreamRepresentation<TId>(this.Name, this.Id);

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; private set; }

        /// <inheritdoc />
        public override void Execute(
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

            if (this.created)
            {
                switch (operation.ExistingStreamEncounteredStrategy)
                {
                    case ExistingStreamEncounteredStrategy.Throw:
                        throw new InvalidOperationException(Invariant($"Expected stream {operation.StreamRepresentation} to not exist, it does and the operation {nameof(operation.ExistingStreamEncounteredStrategy)} is set to '{operation.ExistingStreamEncounteredStrategy}'."));
                    case ExistingStreamEncounteredStrategy.Overwrite:
                        this.items.Clear();
                        break;
                    case ExistingStreamEncounteredStrategy.Skip:
                        break;
                    default:
                        throw new NotSupportedException(Invariant($"Operation {nameof(operation.ExistingStreamEncounteredStrategy)} of '{operation.ExistingStreamEncounteredStrategy}' is not supported."));
                }
            }

            this.created = true;
        }

        /// <inheritdoc />
        public override async Task ExecuteAsync(
            CreateStreamOp<TId> operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just to get the async.
        }

        /// <inheritdoc />
        public override void Execute(
            DeleteStreamOp<TId> operation)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (operation.StreamRepresentation != this.StreamRepresentation)
            {
                throw new ArgumentException(Invariant($"This {nameof(MemoryStream<TId>)} can only 'Delete' a stream with the same representation."));
            }

            if (!this.created)
            {
                switch (operation.ExistingStreamNotEncounteredStrategy)
                {
                    case ExistingStreamNotEncounteredStrategy.Throw:
                        throw new InvalidOperationException(
                            Invariant(
                                $"Expected stream {operation.StreamRepresentation} to exist, it does not and the operation {nameof(operation.ExistingStreamNotEncounteredStrategy)} is '{operation.ExistingStreamNotEncounteredStrategy}'."));
                    case ExistingStreamNotEncounteredStrategy.Skip:
                        break;
                }
            }
            else
            {
                this.items.Clear();
            }
        }

        /// <inheritdoc />
        public override async Task ExecuteAsync(
            DeleteStreamOp<TId> operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just to get the async.
        }

        /// <inheritdoc />
        public override ISyncAndAsyncReturningProtocol<GetLatestByIdAndTypeOp<TId, TObject>, TObject> BuildGetLatestByIdAndTypeProtocol<TObject>()
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
        public override ISyncAndAsyncVoidProtocol<PutOp<TObject>> BuildPutProtocol<TObject>()
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
    }
}
