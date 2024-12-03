// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThrowingStandardStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// A stream that always throws <see cref="InvalidOperationException"/>.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "'Stream' is the best term we could come up with; it's potential confusion with System.IO.Stream was debated.")]
    public class ThrowingStandardStream : StandardStreamBase
    {
        private const string ExceptionMessage = "This stream always throws.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowingStandardStream"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="serializerFactory">The serializer factory to get serializers of existing records or to put new ones.</param>
        /// <param name="defaultSerializerRepresentation">The default serializer representation.</param>
        /// <param name="defaultSerializationFormat">The default serialization format.</param>
        /// <param name="resourceLocatorProtocols">Protocol to get appropriate resource locator(s).</param>
        public ThrowingStandardStream(
            string name,
            ISerializerFactory serializerFactory,
            SerializerRepresentation defaultSerializerRepresentation,
            SerializationFormat defaultSerializationFormat,
            IResourceLocatorProtocols resourceLocatorProtocols)
            : base(name, serializerFactory, defaultSerializerRepresentation, defaultSerializationFormat, resourceLocatorProtocols)
        {
        }

        /// <inheritdoc />
        public override IStreamRepresentation StreamRepresentation =>
            throw new InvalidOperationException(ExceptionMessage);

        /// <inheritdoc />
        public override long Execute(
            StandardGetNextUniqueLongOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public override StreamRecord Execute(
            StandardGetLatestRecordOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public override TryHandleRecordResult Execute(
            StandardTryHandleRecordOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public override PutRecordResult Execute(
            StandardPutRecordOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public override IReadOnlyCollection<long> Execute(
            StandardGetInternalRecordIdsOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public override void Execute(
            StandardUpdateHandlingStatusForStreamOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public override IReadOnlyDictionary<long, HandlingStatus> Execute(
            StandardGetHandlingStatusOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public override IReadOnlyList<StreamRecordHandlingEntry> Execute(
            StandardGetHandlingHistoryOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public override void Execute(
            StandardUpdateHandlingStatusForRecordOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public override IReadOnlyCollection<StringSerializedIdentifier> Execute(
            StandardGetDistinctStringSerializedIdsOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public override string Execute(
            StandardGetLatestStringSerializedObjectOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public override CreateStreamResult Execute(
            StandardCreateStreamOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public override void Execute(
            StandardDeleteStreamOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }

        /// <inheritdoc />
        public override void Execute(
            StandardPruneStreamOp operation)
        {
            throw new InvalidOperationException(ExceptionMessage);
        }
    }
}
