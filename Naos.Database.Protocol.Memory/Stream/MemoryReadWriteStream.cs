// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryReadWriteStream.cs" company="Naos Project">
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
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using static System.FormattableString;

    /// <summary>
    /// In memory implementation of <see cref="IReadWriteStream"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public class MemoryReadWriteStream :
        ReadWriteStreamBase,
        IStreamManagementProtocolFactory,
        IStreamRecordHandlingProtocolFactory,
        IStreamManagementProtocols,
        IReturningProtocol<GetNextUniqueLongOp, long>,
        IReturningProtocol<GetHandlingHistoryOfRecordOp, IReadOnlyList<StreamRecordHandlingEntry>>,
        IReturningProtocol<GetHandlingStatusOfRecordSetByIdOp, HandlingStatus>,
        IReturningProtocol<GetHandlingStatusOfRecordSetByTagOp, HandlingStatus>,
        IReturningProtocol<TryHandleRecordOp, StreamRecord>
    {
        private readonly object streamLock = new object();

        private readonly List<StreamRecord> records = new List<StreamRecord>();
        private bool created = false;
        private long uniqueLongForExternalProtocol = 0;
        private long uniqueLongForInMemoryEntries = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryReadWriteStream"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultSerializerRepresentation">The default serializer representation.</param>
        /// <param name="defaultSerializationFormat">The default serialization format.</param>
        /// <param name="serializerFactory">The serializer factory.</param>
        public MemoryReadWriteStream(
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
        /// Safely add an item to the internal collection; should really only be used in testing and is not part of the <see cref="IReadWriteStream"/> language.
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
        /// Runs the locked operation on record list.
        /// </summary>
        /// <param name="lockedRecordReader">The locked record reader.</param>
        public void RunLockedOperationOnRecordList(Action<IReadOnlyList<StreamRecord>> lockedRecordReader)
        {
            lock (this.streamLock)
            {
                lockedRecordReader((IReadOnlyList<StreamRecord>)this.records);
            }
        }

        /// <inheritdoc />
        public void Execute(
            CreateStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            lock (this.streamLock)
            {
                if (operation == null)
                {
                    throw new ArgumentNullException(nameof(operation));
                }

                if (!Equals(operation.StreamRepresentation, this.StreamRepresentation))
                {
                    throw new ArgumentException(Invariant($"This {nameof(MemoryReadWriteStream)} can only 'create' a stream with the same representation."));
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
        public async Task ExecuteAsync(
            CreateStreamOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just to get the async.
        }

        /// <inheritdoc />
        public void Execute(
            DeleteStreamOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            lock (this.streamLock)
            {
                if (operation == null)
                {
                    throw new ArgumentNullException(nameof(operation));
                }

                if (!Equals(operation.StreamRepresentation, this.StreamRepresentation))
                {
                    throw new ArgumentException(Invariant($"This {nameof(MemoryReadWriteStream)} can only 'Delete' a stream with the same representation."));
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
        public async Task ExecuteAsync(
            DeleteStreamOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just to get the async.
        }

        /// <inheritdoc />
        public override IStreamReadWithIdProtocols<TId> GetStreamReadingWithIdProtocols<TId>() => new MemoryStreamReadWriteWithIdProtocols<TId>(this);

        /// <inheritdoc />
        public override IStreamReadWithIdProtocols<TId, TObject> GetStreamReadingWithIdProtocols<TId, TObject>()
        {
            var result = new MemoryStreamReadWriteWithIdProtocols<TId, TObject>(this);
            return result;
        }

        /// <inheritdoc />
        public override IStreamWriteProtocols GetStreamWritingProtocols() => new MemoryStreamReadWriteProtocols(this);

        /// <inheritdoc />
        public override IStreamReadProtocols GetStreamReadingProtocols() => new MemoryStreamReadWriteProtocols(this);

        /// <inheritdoc />
        public override IStreamReadProtocols<TObject> GetStreamReadingProtocols<TObject>() => new MemoryStreamReadWriteProtocols<TObject>(this);

        /// <inheritdoc />
        public override IStreamWriteWithIdProtocols<TId> GetStreamWritingWithIdProtocols<TId>() => new MemoryStreamReadWriteWithIdProtocols<TId>(this);

        /// <inheritdoc />
        public override IStreamWriteWithIdProtocols<TId, TObject> GetStreamWritingWithIdProtocols<TId, TObject>() => new MemoryStreamReadWriteWithIdProtocols<TId, TObject>(this);

        /// <inheritdoc />
        public override IStreamWriteProtocols<TObject> GetStreamWritingProtocols<TObject>()
        {
            var result = new MemoryStreamReadWriteProtocols<TObject>(this);
            return result;
        }

        /// <summary>
        /// Executes the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>System.Int64.</returns>
        public long Execute(
            GetNextUniqueLongOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            var result = Interlocked.Increment(ref this.uniqueLongForExternalProtocol);
            return result;
        }

        /// <inheritdoc />
        public IStreamManagementProtocols GetStreamManagementProtocols() => this;

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols GetStreamRecordHandlingProtocols() => new MemoryStreamRecordHandlingProtocols(this);

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols<TObject> GetStreamRecordHandlingProtocols<TObject>() => new MemoryStreamRecordHandlingProtocols<TObject>(this);

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId> GetStreamRecordWithIdHandlingProtocols<TId>() => new MemoryStreamRecordWithIdHandlingProtocols<TId>(this);

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId, TObject> GetStreamRecordWithIdHandlingProtocols<TId, TObject>() => new MemoryStreamRecordWithIdHandlingProtocols<TId, TObject>(this);

        /// <inheritdoc />
        public void Execute(
            PruneBeforeInternalRecordDateOp operation)
        {
            lock (this.streamLock)
            {
                var newList = this.records.Where(_ => _.Metadata.TimestampUtc >= operation.MaxInternalRecordDate);
                this.records.Clear();
                this.records.AddRange(newList);
            }
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PruneBeforeInternalRecordDateOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await...
        }

        /// <inheritdoc />
        public void Execute(
            PruneBeforeInternalRecordIdOp operation)
        {
            lock (this.streamLock)
            {
                var newList = this.records.Where(_ => _.InternalRecordId >= operation.MaxInternalRecordId);
                this.records.Clear();
                this.records.AddRange(newList);
            }
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PruneBeforeInternalRecordIdOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await...
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordHandlingEntry> Execute(
            GetHandlingHistoryOfRecordOp operation)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public HandlingStatus Execute(
            GetHandlingStatusOfRecordSetByIdOp operation)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public HandlingStatus Execute(
            GetHandlingStatusOfRecordSetByTagOp operation)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public StreamRecord Execute(
            TryHandleRecordOp operation)
        {
            throw new System.NotImplementedException();
        }
    }
}
