// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetRecordStandardizeExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{

    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// For converting the various flavors down to <see cref="GetLatestRecordOp"/>.
    /// </summary>
    public static class GetRecordStandardizeExtensions
    {        /// <summary>
        /// Converts to common base format <see cref="GetLatestRecordOp"/>.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <returns>GetLatestRecordOp.</returns>
        public static GetLatestRecordOp Standardize<TObject>(
            this GetLatestRecordOp<TObject> operation)
        {
            var result = new GetLatestRecordOp(
                operation.IdentifierType,
                typeof(TObject).ToRepresentation(),
                operation.VersionMatchStrategy,
                operation.ExistingRecordNotEncounteredStrategy,
                operation.SpecifiedResourceLocator);
            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="GetLatestRecordOp"/>.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <returns>GetLatestObjectOp.</returns>
        public static GetLatestRecordOp Standardize<TObject>(
            this GetLatestObjectOp<TObject> operation)
        {
            var result = new GetLatestRecordOp(
                operation.IdentifierType,
                typeof(TObject).ToRepresentation(),
                operation.VersionMatchStrategy,
                operation.ExistingRecordNotEncounteredStrategy,
                operation.SpecifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="GetLatestRecordMetadataByIdOp"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="locator">The locator determined by the identifier.</param>
        /// <returns>GetLatestRecordMetadataByIdOp.</returns>
        public static GetLatestRecordMetadataByIdOp Standardize<TId>(
            this GetLatestRecordMetadataByIdOp<TId> operation,
            IStringSerialize serializer,
            IResourceLocator locator)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new GetLatestRecordMetadataByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                operation.ObjectType,
                operation.VersionMatchStrategy,
                operation.ExistingRecordNotEncounteredStrategy,
                locator);

            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="GetLatestRecordByIdOp"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="locator">The locator determined by the identifier.</param>
        /// <returns>GetLatestRecordOp.</returns>
        public static GetLatestRecordByIdOp Standardize<TId>(
            this GetLatestRecordByIdOp<TId> operation,
            IStringSerialize serializer,
            IResourceLocator locator)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new GetLatestRecordByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                operation.ObjectType,
                operation.VersionMatchStrategy,
                operation.ExistingRecordNotEncounteredStrategy,
                locator);

            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="GetAllRecordsMetadataByIdOp"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="locator">The locator determined by the identifier.</param>
        /// <returns>GetAllRecordsByIdOp.</returns>
        public static GetAllRecordsMetadataByIdOp Standardize<TId>(
            this GetAllRecordsMetadataByIdOp<TId> operation,
            IStringSerialize serializer,
            IResourceLocator locator)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new GetAllRecordsMetadataByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                operation.ObjectType,
                operation.VersionMatchStrategy,
                operation.ExistingRecordNotEncounteredStrategy,
                operation.OrderRecordsStrategy,
                locator);

            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="GetAllRecordsByIdOp"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="locator">The locator determined by the identifier.</param>
        /// <returns>GetAllRecordsByIdOp.</returns>
        public static GetAllRecordsByIdOp Standardize<TId>(
            this GetAllRecordsByIdOp<TId> operation,
            IStringSerialize serializer,
            IResourceLocator locator)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new GetAllRecordsByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                operation.ObjectType,
                operation.VersionMatchStrategy,
                operation.ExistingRecordNotEncounteredStrategy,
                operation.OrderRecordsStrategy,
                locator);

            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="GetLatestRecordByIdOp"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="locator">The locator determined by the identifier.</param>
        /// <returns>GetLatestRecordOp.</returns>
        public static GetLatestRecordByIdOp Standardize<TId, TObject>(
            this GetLatestRecordByIdOp<TId, TObject> operation,
            IStringSerialize serializer,
            IResourceLocator locator)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new GetLatestRecordByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                typeof(TObject).ToRepresentation(),
                operation.VersionMatchStrategy,
                operation.ExistingRecordNotEncounteredStrategy,
                locator);

            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="GetLatestRecordByIdOp"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="locator">The locator determined by the identifier.</param>
        /// <returns>GetLatestObjectOp.</returns>
        public static GetLatestRecordByIdOp Standardize<TId, TObject>(
            this GetLatestObjectByIdOp<TId, TObject> operation,
            IStringSerialize serializer,
            IResourceLocator locator)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new GetLatestRecordByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                typeof(TObject).ToRepresentation(),
                operation.VersionMatchStrategy,
                operation.ExistingRecordNotEncounteredStrategy,
                locator);

            return result;
        }
    }
}
