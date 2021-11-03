// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamBase.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// Stream interface, a stream is a list of objects ordered by timestamp.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public abstract class StandardStreamBase : IStandardStream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardStreamBase"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="serializerFactory">The serializer factory to get serializers of existing records or to put new ones.</param>
        /// <param name="defaultSerializerRepresentation">The default serializer representation.</param>
        /// <param name="defaultSerializationFormat">The default serialization format.</param>
        /// <param name="resourceLocatorProtocols">Protocol to get appropriate resource locator(s).</param>
        protected StandardStreamBase(
            string name,
            ISerializerFactory serializerFactory,
            SerializerRepresentation defaultSerializerRepresentation,
            SerializationFormat defaultSerializationFormat,
            IResourceLocatorProtocols resourceLocatorProtocols)
        {
            name.MustForArg(nameof(name)).NotBeNullNorWhiteSpace();
            serializerFactory.MustForArg(nameof(serializerFactory)).NotBeNull();
            defaultSerializerRepresentation.MustForArg(nameof(defaultSerializerRepresentation)).NotBeNull();
            resourceLocatorProtocols.MustForArg(nameof(resourceLocatorProtocols)).NotBeNull();

            if (defaultSerializationFormat == SerializationFormat.Invalid)
            {
                throw new ArgumentException(FormattableString.Invariant($"Cannot specify a {nameof(SerializationFormat)} of {SerializationFormat.Invalid}."));
            }

            this.Name = name;
            this.SerializerFactory = serializerFactory;
            this.DefaultSerializerRepresentation = defaultSerializerRepresentation;
            this.DefaultSerializationFormat = defaultSerializationFormat;
            this.ResourceLocatorProtocols = resourceLocatorProtocols;
        }

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public ISerializerFactory SerializerFactory { get; private set; }

        /// <inheritdoc />
        public SerializerRepresentation DefaultSerializerRepresentation { get; private set; }

        /// <inheritdoc />
        public SerializationFormat DefaultSerializationFormat { get; private set; }

        /// <inheritdoc />
        public IResourceLocatorProtocols ResourceLocatorProtocols { get; private set; }

        /// <inheritdoc />
        public IStreamReadWithIdProtocols<TId> GetStreamReadingWithIdProtocols<TId>() => new StandardStreamReadWriteWithIdProtocols<TId>(this);

        /// <inheritdoc />
        public IStreamReadWithIdProtocols<TId, TObject> GetStreamReadingWithIdProtocols<TId, TObject>() => new StandardStreamReadWriteWithIdProtocols<TId, TObject>(this);

        /// <inheritdoc />
        public IStreamWriteProtocols GetStreamWritingProtocols() => new StandardStreamReadWriteProtocols(this);

        /// <inheritdoc />
        public IStreamReadProtocols GetStreamReadingProtocols() => new StandardStreamReadWriteProtocols(this);

        /// <inheritdoc />
        public IStreamReadProtocols<TObject> GetStreamReadingProtocols<TObject>() => new StandardStreamReadWriteProtocols<TObject>(this);

        /// <inheritdoc />
        public IStreamWriteWithIdProtocols<TId> GetStreamWritingWithIdProtocols<TId>() => new StandardStreamReadWriteWithIdProtocols<TId>(this);

        /// <inheritdoc />
        public IStreamWriteWithIdProtocols<TId, TObject> GetStreamWritingWithIdProtocols<TId, TObject>() => new StandardStreamReadWriteWithIdProtocols<TId, TObject>(this);

        /// <inheritdoc />
        public IStreamWriteProtocols<TObject> GetStreamWritingProtocols<TObject>() => new StandardStreamReadWriteProtocols<TObject>(this);

        /// <inheritdoc />
        public IStreamManagementProtocols GetStreamManagementProtocols() => new StandardStreamManagementProtocols(this);

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols GetStreamRecordHandlingProtocols() => new StandardStreamRecordHandlingProtocols(this);

        /// <inheritdoc />
        public IStreamRecordHandlingProtocols<TObject> GetStreamRecordHandlingProtocols<TObject>() => new StandardStreamRecordHandlingProtocols<TObject>(this);

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId> GetStreamRecordWithIdHandlingProtocols<TId>() => new StandardStreamRecordWithIdHandlingProtocols<TId>(this);

        /// <inheritdoc />
        public IStreamRecordWithIdHandlingProtocols<TId, TObject> GetStreamRecordWithIdHandlingProtocols<TId, TObject>() => new StandardStreamRecordWithIdHandlingProtocols<TId, TObject>(this);

        /// <inheritdoc />
        public abstract IStreamRepresentation StreamRepresentation { get; }

        /// <inheritdoc />
        public abstract long Execute(
            StandardGetNextUniqueLongOp operation);

        /// <inheritdoc />
        public abstract StreamRecord Execute(
            StandardGetRecordByInternalRecordIdOp operation);

        /// <inheritdoc />
        public abstract StreamRecord Execute(
            StandardGetLatestRecordOp operation);

        /// <inheritdoc />
        public abstract StreamRecord Execute(
            StandardGetLatestRecordByIdOp operation);

        /// <inheritdoc />
        public abstract TryHandleRecordResult Execute(
            StandardTryHandleRecordOp operation);

        /// <inheritdoc />
        public abstract PutRecordResult Execute(
            StandardPutRecordOp operation);

        /// <inheritdoc />
        public abstract bool Execute(
            StandardDoesAnyExistByIdOp operation);

        /// <inheritdoc />
        public abstract StreamRecordMetadata Execute(
            StandardGetLatestRecordMetadataByIdOp operation);

        /// <inheritdoc />
        public abstract IReadOnlyList<StreamRecord> Execute(
            StandardGetAllRecordsByIdOp operation);

        /// <inheritdoc />
        public abstract IReadOnlyList<StreamRecordMetadata> Execute(
            StandardGetAllRecordsMetadataByIdOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            StandardUpdateStreamHandlingOp operation);

        /// <inheritdoc />
        public abstract IReadOnlyCollection<HandlingStatus> Execute(
            StandardGetRecordHandlingStatusOp operation);

        /// <inheritdoc />
        public abstract IReadOnlyList<StreamRecordHandlingEntry> Execute(
            StandardGetHandlingHistoryOfRecordOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            StandardUpdateRecordHandlingOp operation);

        /// <inheritdoc />
        public abstract IReadOnlyCollection<string> Execute(
            StandardGetDistinctStringSerializedIdsOp operation);

        /// <inheritdoc />
        public abstract StreamRecord Execute(
            StandardGetLatestRecordByTagOp operation);

        /// <inheritdoc />
        public abstract StandardCreateStreamResult Execute(
            StandardCreateStreamOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            StandardDeleteStreamOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            StandardPruneStreamOp operation);
    }
}
