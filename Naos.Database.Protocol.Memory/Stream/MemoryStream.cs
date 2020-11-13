// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.Memory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Serialization;
    using static System.FormattableString;

    /// <summary>
    /// In memory implementation of <see cref="IStream"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public class MemoryStream : StreamBase
    {
        private readonly object streamLock = new object();

        private readonly IList<StreamRecord> records = new List<StreamRecord>();
        private bool created = false;
        private long uniqueLongForExternalProtocol = 0;
        private long uniqueLongForInMemoryEntries = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStream"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultSerializerRepresentation">The default serializer representation.</param>
        /// <param name="defaultSerializationFormat">The default serialization format.</param>
        /// <param name="serializerFactory">The serializer factory.</param>
        public MemoryStream(
            string name,
            SerializerRepresentation defaultSerializerRepresentation,
            SerializationFormat defaultSerializationFormat,
            ISerializerFactory serializerFactory)
        : base(name, new SingleResourceLocatorProtocol(new MemoryDatabaseLocator(name)))
        {
            this.DefaultSerializerRepresentation = defaultSerializerRepresentation ?? throw new ArgumentNullException(nameof(defaultSerializerRepresentation));

            if (defaultSerializationFormat == SerializationFormat.Invalid)
            {
                throw new ArgumentException(Invariant($"Cannot specify a {nameof(SerializationFormat)} of {SerializationFormat.Invalid}."));
            }

            this.DefaultSerializationFormat = defaultSerializationFormat;
            this.SerializerFactory = serializerFactory ?? throw new ArgumentNullException(nameof(serializerFactory));
            this.Id = Guid.NewGuid().ToString().ToUpperInvariant();
        }

        /// <inheritdoc />
        public override IStreamRepresentation StreamRepresentation => new MemoryStreamRepresentation(this.Name, this.Id);

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the serializer factory.
        /// </summary>
        /// <value>The serializer factory.</value>
        public ISerializerFactory SerializerFactory { get; private set; }

        /// <summary>
        /// Gets the default serializer representation.
        /// </summary>
        /// <value>The default serializer representation.</value>
        public SerializerRepresentation DefaultSerializerRepresentation { get; private set; }

        /// <summary>
        /// Gets the default serialization format.
        /// </summary>
        /// <value>The default serialization format.</value>
        public SerializationFormat DefaultSerializationFormat { get; private set; }

        /// <summary>
        /// Safely add an item to the internal collection; should really only be used in testing and is not part of the <see cref="IStream"/> language.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="payload">The payload.</param>
        /// <returns>Internal identifier.</returns>
        public long AddItem(
            StreamRecordMetadata metadata,
            DescribedSerialization payload)
        {
            lock (this.streamLock)
            {
                var id = Interlocked.Increment(ref this.uniqueLongForInMemoryEntries);
                var itemToAdd = new StreamRecord(id, metadata, payload);
                this.records.Add(itemToAdd);
                return id;
            }
        }

        /// <summary>
        /// Gets a shallow copy of the list of items at time of calling.
        /// </summary>
        /// <returns>The items at time of calling.</returns>
        public IReadOnlyCollection<StreamRecord> GetItems()
        {
            lock (this.streamLock)
            {
                var result = this.records.Select(_ => _).ToList();
                return result;
            }
        }

        /// <inheritdoc />
        public override void Execute(
            CreateStreamOp operation)
        {
            lock (this.streamLock)
            {
                if (operation == null)
                {
                    throw new ArgumentNullException(nameof(operation));
                }

                if (!Equals(operation.StreamRepresentation, this.StreamRepresentation))
                {
                    throw new ArgumentException(Invariant($"This {nameof(MemoryStream)} can only 'create' a stream with the same representation."));
                }

                if (this.created)
                {
                    switch (operation.ExistingStreamEncounteredStrategy)
                    {
                        case ExistingStreamEncounteredStrategy.Throw:
                            throw new InvalidOperationException(
                                Invariant(
                                    $"Expected stream {operation.StreamRepresentation} to not exist, it does and the operation {nameof(operation.ExistingStreamEncounteredStrategy)} is set to '{operation.ExistingStreamEncounteredStrategy}'."));
                        case ExistingStreamEncounteredStrategy.Overwrite:
                            this.records.Clear();
                            break;
                        case ExistingStreamEncounteredStrategy.Skip:
                            break;
                        default:
                            throw new NotSupportedException(
                                Invariant(
                                    $"Operation {nameof(operation.ExistingStreamEncounteredStrategy)} of '{operation.ExistingStreamEncounteredStrategy}' is not supported."));
                    }
                }

                this.created = true;
            }
        }

        /// <inheritdoc />
        public override async Task ExecuteAsync(
            CreateStreamOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just to get the async.
        }

        /// <inheritdoc />
        public override void Execute(
            DeleteStreamOp operation)
        {
            lock (this.streamLock)
            {
                if (operation == null)
                {
                    throw new ArgumentNullException(nameof(operation));
                }

                if (!Equals(operation.StreamRepresentation, this.StreamRepresentation))
                {
                    throw new ArgumentException(Invariant($"This {nameof(MemoryStream)} can only 'Delete' a stream with the same representation."));
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
                    this.records.Clear();
                }
            }
        }

        /// <inheritdoc />
        public override async Task ExecuteAsync(
            DeleteStreamOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just to get the async.
        }

        /// <inheritdoc />
        public override IProtocolStreamObjectReadOperations<TId, TObject> GetObjectReadOperationsProtocol<TId, TObject>()
        {
            var result = new MemoryStreamObjectOperationsProtocol<TId, TObject>(this);
            return result;
        }

        /// <inheritdoc />
        public override IProtocolStreamObjectReadOperations<TObject> GetObjectReadOperationsProtocol<TObject>()
        {
            var result = new MemoryStreamObjectOperationsProtocol<NullStreamObjectIdentifier, TObject>(this);
            return result;
        }

        /// <inheritdoc />
        public override IProtocolStreamObjectWriteOperations<TId, TObject> GetObjectWriteOperationsProtocol<TId, TObject>()
        {
            var result = new MemoryStreamObjectOperationsProtocol<TId, TObject>(this);
            return result;
        }

        /// <inheritdoc />
        public override IProtocolStreamObjectWriteOperations<TObject> GetObjectWriteOperationsProtocol<TObject>()
        {
            var result = new MemoryStreamObjectOperationsProtocol<NullStreamObjectIdentifier, TObject>(this);
            return result;
        }

        /// <inheritdoc />
        public override long Execute(
            GetNextUniqueLongOp operation)
        {
            var result = Interlocked.Increment(ref this.uniqueLongForExternalProtocol);
            return result;
        }

        /// <inheritdoc />
        public override async Task<long> ExecuteAsync(
            GetNextUniqueLongOp operation)
        {
            var syncResult = this.Execute(operation);
            return await Task.FromResult(syncResult);
        }
    }
}
