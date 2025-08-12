// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordStandardizeExtensions.Write.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    public static partial class RecordStandardizeExtensions
    {
        /// <summary>
        /// Converts to common base format <see cref="StandardGetNextUniqueLongOp"/>.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>The standardized operation.</returns>
        public static StandardGetNextUniqueLongOp Standardize(
            this GetNextUniqueLongOp operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new StandardGetNextUniqueLongOp(operation.Details, specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "specifiedResourceLocator", Justification = "Added for consistency with other extension methods.")]
        public static StandardPutRecordOp Standardize<TObject>(
            this PutAndReturnInternalRecordIdOp<TObject> operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            throw new NotImplementedException(Invariant($"This operation is transformed and then delegated to other operations, eventually making it's way into a PutWithIdAndReturnInternalRecordIdOp<TId, TObject>."));
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "specifiedResourceLocator", Justification = "Added for consistency with other extension methods.")]
        public static StandardPutRecordOp Standardize<TObject>(
            this PutOp<TObject> operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            throw new NotImplementedException(Invariant($"This operation is transformed and then delegated to other operations, eventually making it's way into a PutWithIdAndReturnInternalRecordIdOp{{TId, TObject}}."));
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardPutRecordOp Standardize<TId, TObject>(
            this PutWithIdAndReturnInternalRecordIdOp<TId, TObject> operation,
            IStandardStream stream,
            IResourceLocator specifiedResourceLocator = null)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();
            stream.MustForArg(nameof(stream)).NotBeNull();

            var serializedStringId = stream.IdSerializer.SerializeToString(operation.Id);

            var identifierTypeRep = typeof(TId).ToRepresentation();

            var objectTypeRep = operation.TypeSelectionStrategy.Apply(operation.ObjectToPut).ToRepresentation();

            var describedSerialization = operation.ObjectToPut.ToDescribedSerializationUsingSpecificFactory(
                stream.DefaultSerializerRepresentation,
                SerializerRepresentationSelectionStrategy.UseSpecifiedRepresentation,
                stream.SerializerFactory,
                stream.DefaultSerializationFormat);

            var objectTimestamp = operation.ObjectToPut is IHaveTimestampUtc objectWithTimestamp
                ? objectWithTimestamp.TimestampUtc
                : (DateTime?)null;

            var metadata = new StreamRecordMetadata(
                serializedStringId,
                stream.DefaultSerializerRepresentation,
                identifierTypeRep.ToWithAndWithoutVersion(),
                objectTypeRep.ToWithAndWithoutVersion(),
                operation.Tags,
                DateTime.UtcNow,
                objectTimestamp);

            var payload = describedSerialization.ToStreamRecordPayload();

            var result = new StandardPutRecordOp(
                metadata,
                payload,
                operation.ExistingRecordStrategy,
                operation.RecordRetentionCount,
                operation.VersionMatchStrategy,
                null,
                specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "specifiedResourceLocator", Justification = "Added for consistency with other extension methods.")]
        public static StandardPutRecordOp Standardize<TId, TObject>(
            this PutWithIdOp<TId, TObject> operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            throw new NotImplementedException(Invariant($"This operation is transformed and then delegated to other operations, eventually making it's way into a PutWithIdAndReturnInternalRecordIdOp<TId, TObject>."));
        }
    }
}
