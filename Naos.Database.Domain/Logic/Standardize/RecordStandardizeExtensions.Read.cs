// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordStandardizeExtensions.Read.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;

    public static partial class RecordStandardizeExtensions
    {
        /// <summary>
        /// Converts to common base format <see cref="StandardDoesAnyExistByIdOp"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="locator">The locator determined by the identifier.</param>
        /// <returns>The standardized operation.</returns>
        public static StandardDoesAnyExistByIdOp Standardize<TId>(
            this DoesAnyExistByIdOp<TId> operation,
            IStringSerialize serializer,
            IResourceLocator locator)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new StandardDoesAnyExistByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                operation.ObjectType,
                operation.VersionMatchStrategy,
                locator);

            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="StandardGetAllRecordsByIdOp"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="locator">The locator determined by the identifier.</param>
        /// <returns>The standardized operation.</returns>
        public static StandardGetAllRecordsByIdOp Standardize<TId>(
            this GetAllRecordsByIdOp<TId> operation,
            IStringSerialize serializer,
            IResourceLocator locator)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new StandardGetAllRecordsByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                operation.ObjectType,
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy,
                operation.OrderRecordsBy,
                locator);

            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="StandardGetAllRecordsMetadataByIdOp"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="locator">The locator determined by the identifier.</param>
        /// <returns>The standardized operation.</returns>
        public static StandardGetAllRecordsMetadataByIdOp Standardize<TId>(
            this GetAllRecordsMetadataByIdOp<TId> operation,
            IStringSerialize serializer,
            IResourceLocator locator)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new StandardGetAllRecordsMetadataByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                operation.ObjectType,
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy,
                operation.OrderRecordsBy,
                locator);

            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="StandardGetLatestRecordByIdOp"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="locator">The locator determined by the identifier.</param>
        /// <returns>The standardized operation.</returns>
        public static StandardGetLatestRecordByIdOp Standardize<TId, TObject>(
            this GetLatestObjectByIdOp<TId, TObject> operation,
            IStringSerialize serializer,
            IResourceLocator locator)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new StandardGetLatestRecordByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                typeof(TObject).ToRepresentation(),
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy,
                locator);

            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="StandardGetLatestRecordOp"/>.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <returns>The standardized operation.</returns>
        public static StandardGetLatestRecordByTagOp Standardize<TObject>(
            this GetLatestObjectByTagOp<TObject> operation)
        {
            var result = new StandardGetLatestRecordByTagOp(
                operation.Tag,
                typeof(TObject).ToRepresentation(),
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy);
            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="StandardGetLatestRecordOp"/>.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <returns>The standardized operation.</returns>
        public static StandardGetLatestRecordOp Standardize<TObject>(
            this GetLatestObjectOp<TObject> operation)
        {
            var result = new StandardGetLatestRecordOp(
                operation.IdentifierType,
                typeof(TObject).ToRepresentation(),
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy);

            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="StandardGetLatestRecordByIdOp"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="locator">The locator determined by the identifier.</param>
        /// <returns>The standardized operation.</returns>
        public static StandardGetLatestRecordByIdOp Standardize<TId, TObject>(
            this GetLatestRecordByIdOp<TId, TObject> operation,
            IStringSerialize serializer,
            IResourceLocator locator)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new StandardGetLatestRecordByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                typeof(TObject).ToRepresentation(),
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy,
                locator);

            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="StandardGetLatestRecordByIdOp"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="locator">The locator determined by the identifier.</param>
        /// <returns>The standardized operation.</returns>
        public static StandardGetLatestRecordByIdOp Standardize<TId>(
            this GetLatestRecordByIdOp<TId> operation,
            IStringSerialize serializer,
            IResourceLocator locator)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new StandardGetLatestRecordByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                operation.ObjectType,
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy,
                locator);

            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="StandardGetLatestRecordMetadataByIdOp"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="locator">The locator determined by the identifier.</param>
        /// <returns>The standardized operation.</returns>
        public static StandardGetLatestRecordMetadataByIdOp Standardize<TId>(
            this GetLatestRecordMetadataByIdOp<TId> operation,
            IStringSerialize serializer,
            IResourceLocator locator)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new StandardGetLatestRecordMetadataByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                operation.ObjectType,
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy,
                locator);

            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="StandardGetLatestRecordOp"/>.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <returns>The standardized operation.</returns>
        public static StandardGetLatestRecordOp Standardize<TObject>(
            this GetLatestRecordOp<TObject> operation)
        {
            var result = new StandardGetLatestRecordOp(
                operation.IdentifierType,
                typeof(TObject).ToRepresentation(),
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy);
            return result;
        }
    }
}
