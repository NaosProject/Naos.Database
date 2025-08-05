// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamBase.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// Base class implementation of an <see cref="IStandardStream"/>.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
    public abstract class StandardStreamBase : IStandardStream
    {
        private static readonly IStringSerializeAndDeserialize IdentifierSerializer = new ObcSimplifyingSerializer(new ObcAlwaysThrowingSerializer());

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardStreamBase"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="serializerFactory">The serializer factory to use to get serializers for objects (not identifiers), regardless of putting new or getting existing records.</param>
        /// <param name="defaultSerializerRepresentation">The serializer representation to use to get a serializer to use when serializing objects (not identifiers) into record payloads to put.</param>
        /// <param name="defaultSerializationFormat">The serialization format to use when serializing objects (not identifiers) into record payloads to put.</param>
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
            defaultSerializationFormat.MustForArg(nameof(defaultSerializationFormat)).NotBeEqualTo(SerializationFormat.Invalid);
            resourceLocatorProtocols.MustForArg(nameof(resourceLocatorProtocols)).NotBeNull();

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

        /// <summary>
        /// Gets the serializer to use for identifiers.
        /// Unless overriden, returns <see cref="ObcSimplifyingSerializer"/> whose fallback serializer always throws.
        /// As such, only "simple" types can be used for identifiers (e.g. string, guid).
        /// </summary>
        public virtual IStringSerializeAndDeserialize IdSerializer => IdentifierSerializer;

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
        public IStreamDistributedMutexProtocols GetStreamDistributedMutexProtocols() => new StandardStreamDistributedMutexProtocols(this);

        /// <inheritdoc />
        public abstract IStreamRepresentation StreamRepresentation { get; }

        /// <inheritdoc />
        public abstract long Execute(
            StandardGetNextUniqueLongOp operation);

        /// <inheritdoc />
        public abstract StreamRecord Execute(
            StandardGetLatestRecordOp operation);

        /// <inheritdoc />
        public abstract TryHandleRecordResult Execute(
            StandardTryHandleRecordOp operation);

        /// <inheritdoc />
        public abstract PutRecordResult Execute(
            StandardPutRecordOp operation);

        /// <inheritdoc />
        public abstract IReadOnlyCollection<long> Execute(
            StandardGetInternalRecordIdsOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            StandardUpdateHandlingStatusForStreamOp operation);

        /// <inheritdoc />
        public abstract IReadOnlyDictionary<long, HandlingStatus> Execute(
            StandardGetHandlingStatusOp operation);

        /// <inheritdoc />
        public abstract IReadOnlyList<StreamRecordHandlingEntry> Execute(
            StandardGetHandlingHistoryOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            StandardUpdateHandlingStatusForRecordOp operation);

        /// <inheritdoc />
        public abstract IReadOnlyCollection<StringSerializedIdentifier> Execute(
            StandardGetDistinctStringSerializedIdsOp operation);

        /// <inheritdoc />
        public abstract string Execute(
            StandardGetLatestStringSerializedObjectOp operation);

        /// <inheritdoc />
        public abstract CreateStreamResult Execute(
            StandardCreateStreamOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            StandardDeleteStreamOp operation);

        /// <inheritdoc />
        public abstract void Execute(
            StandardPruneStreamOp operation);
    }
}
