// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardReadWriteStreamBase.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Serialization;

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
        /// <param name="serializerFactory">The serializer factory to get serializers of existing records or to put new ones.</param>
        /// <param name="defaultSerializerRepresentation">The default serializer representation.</param>
        /// <param name="defaultSerializationFormat">The default serialization format.</param>
        /// <param name="resourceLocatorProtocols">Protocol to get appropriate resource locator(s).</param>
        protected StandardReadWriteStreamBase(
            string name,
            ISerializerFactory serializerFactory,
            SerializerRepresentation defaultSerializerRepresentation,
            SerializationFormat defaultSerializationFormat,
            IResourceLocatorProtocols resourceLocatorProtocols)
            : base(name, serializerFactory, defaultSerializerRepresentation, defaultSerializationFormat, resourceLocatorProtocols)
        {
        }

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
            StandardGetNextUniqueLongOp operation);

        /// <inheritdoc />
        public abstract override StreamRecord Execute(
            StandardGetRecordByInternalRecordIdOp operation);

        /// <inheritdoc />
        public abstract override StreamRecord Execute(
            StandardGetLatestRecordOp operation);

        /// <inheritdoc />
        public abstract override StreamRecord Execute(
            StandardGetLatestRecordByIdOp operation);

        /// <inheritdoc />
        public abstract IReadOnlyList<StreamRecordHandlingEntry> Execute(
            GetHandlingHistoryOfRecordOp operation);

        /// <inheritdoc />
        public abstract HandlingStatus Execute(
            GetHandlingStatusOfRecordByInternalRecordIdOp operation);

        /// <inheritdoc />
        public abstract HandlingStatus Execute(
            GetHandlingStatusOfRecordsByIdOp operation);

        /// <inheritdoc />
        public abstract HandlingStatus Execute(
            GetHandlingStatusOfRecordSetByTagOp operation);

        /// <inheritdoc />
        public abstract TryHandleRecordResult Execute(
            StandardTryHandleRecordOp operation);

        /// <inheritdoc />
        public abstract PutRecordResult Execute(
            StandardPutRecordOp operation);

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
        public abstract CreateStreamResult Execute(
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
        public async Task<CreateStreamResult> ExecuteAsync(
            CreateStreamOp operation)
        {
            var syncResult = this.Execute(operation);
            return await Task.FromResult(syncResult);
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
        public abstract override bool Execute(
            StandardDoesAnyExistByIdOp operation);

        /// <inheritdoc />
        public abstract override StreamRecordMetadata Execute(
            StandardGetLatestRecordMetadataByIdOp operation);

        /// <inheritdoc />
        public abstract override IReadOnlyList<StreamRecord> Execute(
            StandardGetAllRecordsByIdOp operation);

        /// <inheritdoc />
        public abstract override IReadOnlyList<StreamRecordMetadata> Execute(
            StandardGetAllRecordsMetadataByIdOp operation);
    }
}
