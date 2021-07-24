// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullStandardReadWriteStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;

    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// A null object to be used as the id of an object in a <see cref="IReadWriteStream"/> that does not have an actual identifier.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public partial class NullStandardReadWriteStream : IStandardReadWriteStream // IModelViaCodeGen - not done each time because the Serialization tests have been removed.
    {
        /// <summary>
        /// Exception message indicating specific failure.
        /// </summary>
        public static readonly string ExceptionMessage = Invariant(
            $"This is the null object class '{nameof(NullStandardReadWriteStream)}'.  None of these methods are expected to be functional and this error was likely bad configuration.");

        /// <inheritdoc />
        public string Name => nameof(NullStandardReadWriteStream);

        /// <inheritdoc />
        public IStreamRepresentation StreamRepresentation => new NullStreamRepresentation();

        /// <inheritdoc />
        public IResourceLocatorProtocols ResourceLocatorProtocols => new NullResourceLocatorProtocols();

        /// <inheritdoc />
        public ISerializerFactory SerializerFactory => null;

        /// <inheritdoc />
        public SerializerRepresentation DefaultSerializerRepresentation => null;

        /// <inheritdoc />
        public SerializationFormat DefaultSerializationFormat => SerializationFormat.Invalid;

        /// <inheritdoc />
        public IStreamReadProtocols GetStreamReadingProtocols()
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamReadProtocols<TObject> GetStreamReadingProtocols<TObject>()
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamReadWithIdProtocols<TId> GetStreamReadingWithIdProtocols<TId>()
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamReadWithIdProtocols<TId, TObject> GetStreamReadingWithIdProtocols<TId, TObject>()
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamWriteProtocols GetStreamWritingProtocols()
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamWriteProtocols<TObject> GetStreamWritingProtocols<TObject>()
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamWriteWithIdProtocols<TId> GetStreamWritingWithIdProtocols<TId>()
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamWriteWithIdProtocols<TId, TObject> GetStreamWritingWithIdProtocols<TId, TObject>()
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamManagementProtocols GetStreamManagementProtocols()
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols GetStreamRecordHandlingProtocols()
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols<TObject> GetStreamRecordHandlingProtocols<TObject>()
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId> GetStreamRecordWithIdHandlingProtocols<TId>()
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId, TObject> GetStreamRecordWithIdHandlingProtocols<TId, TObject>()
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public PutRecordResult Execute(
            PutRecordOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IReadOnlyCollection<string> Execute(GetDistinctStringSerializedIdsOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public StreamRecord Execute(GetRecordByInternalRecordIdOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public StreamRecord Execute(GetLatestRecordOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public StreamRecordMetadata Execute(GetLatestRecordMetadataByIdOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecord> Execute(GetAllRecordsByIdOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public StreamRecord Execute(GetLatestRecordByTagOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordMetadata> Execute(GetAllRecordsMetadataByIdOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public bool Execute(DoesAnyExistByIdOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public StreamRecord Execute(GetLatestRecordByIdOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public CreateStreamResult Execute(CreateStreamOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public Task<CreateStreamResult> ExecuteAsync(CreateStreamOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public void Execute(DeleteStreamOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public Task ExecuteAsync(DeleteStreamOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public void Execute(PruneBeforeInternalRecordDateOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public Task ExecuteAsync(PruneBeforeInternalRecordDateOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public void Execute(PruneBeforeInternalRecordIdOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public Task ExecuteAsync(PruneBeforeInternalRecordIdOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public long Execute(GetNextUniqueLongOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordHandlingEntry> Execute(GetHandlingHistoryOfRecordOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public HandlingStatus Execute(GetHandlingStatusOfRecordsByIdOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public HandlingStatus Execute(GetHandlingStatusOfRecordSetByTagOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public TryHandleRecordResult Execute(TryHandleRecordOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public void Execute(BlockRecordHandlingOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public void Execute(CancelBlockedRecordHandlingOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public void Execute(CancelHandleRecordExecutionRequestOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public void Execute(CancelRunningHandleRecordExecutionOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public void Execute(CompleteRunningHandleRecordExecutionOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public void Execute(FailRunningHandleRecordExecutionOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public void Execute(SelfCancelRunningHandleRecordExecutionOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }

        /// <inheritdoc />
        public void Execute(RetryFailedHandleRecordExecutionOp operation)
        {
            throw new NotImplementedException(ExceptionMessage);
        }
    }
}
