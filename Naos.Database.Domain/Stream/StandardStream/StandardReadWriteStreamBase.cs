// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardReadWriteStreamBase.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using static System.FormattableString;

    /// <summary>
    /// Stream interface, a stream is a list of objects ordered by timestamp.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public abstract class StandardReadWriteStreamBase :
        ReadWriteStreamBase,
        IStandardReadWriteStream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardReadWriteStreamBase"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="resourceLocatorProtocols">Protocol to get appropriate resource locator(s).</param>
        /// <param name="serializerFactory">The serializer factory to get serializers of existing records or to put new ones.</param>
        /// <param name="defaultSerializerRepresentation">The default serializer representation.</param>
        /// <param name="defaultSerializationFormat">The default serialization format.</param>
        protected StandardReadWriteStreamBase(
            string name,
            IResourceLocatorProtocols resourceLocatorProtocols,
            ISerializerFactory serializerFactory,
            SerializerRepresentation defaultSerializerRepresentation,
            SerializationFormat defaultSerializationFormat)
            : base(name, resourceLocatorProtocols)
        {
            serializerFactory.MustForArg(nameof(serializerFactory)).NotBeNull();
            defaultSerializerRepresentation.MustForArg(nameof(defaultSerializerRepresentation)).NotBeNull();

            if (defaultSerializationFormat == SerializationFormat.Invalid)
            {
                throw new ArgumentException(Invariant($"Cannot specify a {nameof(SerializationFormat)} of {SerializationFormat.Invalid}."));
            }

            this.SerializerFactory = serializerFactory;
            this.DefaultSerializerRepresentation = defaultSerializerRepresentation;
            this.DefaultSerializationFormat = defaultSerializationFormat;
        }

        /// <inheritdoc />
        public ISerializerFactory SerializerFactory { get; private set; }

        /// <inheritdoc />
        public SerializerRepresentation DefaultSerializerRepresentation { get; private set; }

        /// <inheritdoc />
        public SerializationFormat DefaultSerializationFormat { get; private set; }

        /// <inheritdoc />
        public override IStreamReadWithIdProtocols<TId> GetStreamReadingWithIdProtocols<TId>() => new StandardStreamReadWriteWithIdProtocols<TId>(this);

        /// <inheritdoc />
        public override IStreamReadWithIdProtocols<TId, TObject> GetStreamReadingWithIdProtocols<TId, TObject>() => new StandardStreamReadWriteWithIdProtocols<TId, TObject>(this);

        /// <inheritdoc />
        public override IStreamWriteProtocols GetStreamWritingProtocols() => new StandardStreamReadWriteProtocols(this);

        /// <inheritdoc />
        public override IStreamReadProtocols GetStreamReadingProtocols() => new StandardStreamReadWriteProtocols(this);

        /// <inheritdoc />
        public override IStreamReadProtocols<TObject> GetStreamReadingProtocols<TObject>() => new StandardStreamReadWriteProtocols<TObject>(this);

        /// <inheritdoc />
        public override IStreamWriteWithIdProtocols<TId> GetStreamWritingWithIdProtocols<TId>() => new StandardStreamReadWriteWithIdProtocols<TId>(this);

        /// <inheritdoc />
        public override IStreamWriteWithIdProtocols<TId, TObject> GetStreamWritingWithIdProtocols<TId, TObject>() => new StandardStreamReadWriteWithIdProtocols<TId, TObject>(this);

        /// <inheritdoc />
        public override IStreamWriteProtocols<TObject> GetStreamWritingProtocols<TObject>() => new StandardStreamReadWriteProtocols<TObject>(this);

        /// <inheritdoc />
        public IStreamManagementProtocols GetStreamManagementProtocols() => this;

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols GetStreamRecordHandlingProtocols() => new StandardStreamRecordHandlingProtocols(this);

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols<TObject> GetStreamRecordHandlingProtocols<TObject>() => new StandardStreamRecordHandlingProtocols<TObject>(this);

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId> GetStreamRecordWithIdHandlingProtocols<TId>() => new StandardStreamRecordWithIdHandlingProtocols<TId>(this);

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId, TObject> GetStreamRecordWithIdHandlingProtocols<TId, TObject>() => new StandardStreamRecordWithIdHandlingProtocols<TId, TObject>(this);

        /// <inheritdoc />
        public abstract long Execute(
            GetNextUniqueLongOp operation);

        /// <inheritdoc />
        public abstract StreamRecord Execute(
            GetLatestRecordOp operation);

        /// <inheritdoc />
        public abstract StreamRecord Execute(
            GetLatestRecordByIdOp operation);

        /// <inheritdoc />
        public abstract IReadOnlyList<StreamRecordHandlingEntry> Execute(
            GetHandlingHistoryOfRecordOp operation);

        /// <inheritdoc />
        public abstract HandlingStatus Execute(
            GetHandlingStatusOfRecordsByIdOp operation);

        /// <inheritdoc />
        public abstract HandlingStatus Execute(
            GetHandlingStatusOfRecordSetByTagOp operation);

        /// <inheritdoc />
        public abstract StreamRecord Execute(
            TryHandleRecordOp operation);

        /// <inheritdoc />
        public abstract long Execute(
            PutRecordOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            BlockRecordHandlingOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            CancelBlockedRecordHandlingOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            CancelHandleRecordExecutionRequestOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            CancelRunningHandleRecordExecutionOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            CompleteRunningHandleRecordExecutionOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            FailRunningHandleRecordExecutionOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            SelfCancelRunningHandleRecordExecutionOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            RetryFailedHandleRecordExecutionOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            CreateStreamOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            DeleteStreamOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            PruneBeforeInternalRecordDateOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            PruneBeforeInternalRecordIdOp operation);

        /// <inheritdoc />
        public async Task ExecuteAsync(
            CreateStreamOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await...
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            DeleteStreamOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await...
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PruneBeforeInternalRecordDateOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await...
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PruneBeforeInternalRecordIdOp operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await...
        }

        /// <inheritdoc />
        public abstract bool Execute(
            DoesAnyExistByIdOp operation);

        /// <inheritdoc />
        public abstract StreamRecordMetadata Execute(
            GetLatestRecordMetadataByIdOp operation);

        /// <inheritdoc />
        public abstract IReadOnlyList<StreamRecord> Execute(
            GetAllRecordsByIdOp operation);

        /// <inheritdoc />
        public abstract IReadOnlyList<StreamRecordMetadata> Execute(
            GetAllRecordsMetadataByIdOp operation);
    }
}
