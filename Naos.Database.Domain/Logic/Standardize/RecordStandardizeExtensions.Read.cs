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
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardDoesAnyExistByIdOp Standardize<TId>(
            this DoesAnyExistByIdOp<TId> operation,
            IStringSerialize serializer,
            IResourceLocator specifiedResourceLocator = null)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new StandardDoesAnyExistByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                operation.ObjectType,
                operation.VersionMatchStrategy,
                specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardGetAllRecordsByIdOp Standardize<TId>(
            this GetAllRecordsByIdOp<TId> operation,
            IStringSerialize serializer,
            IResourceLocator specifiedResourceLocator = null)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new StandardGetAllRecordsByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                operation.ObjectType,
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy,
                operation.OrderRecordsBy,
                specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardGetAllRecordsMetadataByIdOp Standardize<TId>(
            this GetAllRecordsMetadataByIdOp<TId> operation,
            IStringSerialize serializer,
            IResourceLocator specifiedResourceLocator = null)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new StandardGetAllRecordsMetadataByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                operation.ObjectType,
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy,
                operation.OrderRecordsBy,
                specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardGetLatestRecordByIdOp Standardize<TId, TObject>(
            this GetLatestObjectByIdOp<TId, TObject> operation,
            IStringSerialize serializer,
            IResourceLocator specifiedResourceLocator = null)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new StandardGetLatestRecordByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                typeof(TObject).ToRepresentation(),
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy,
                specifiedResourceLocator);

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
        public static StandardGetLatestRecordByTagsOp Standardize<TObject>(
            this GetLatestObjectByTagsOp<TObject> operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            var result = new StandardGetLatestRecordByTagsOp(
                operation.Tags,
                typeof(TObject).ToRepresentation(),
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy,
                specifiedResourceLocator);

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
        public static StandardGetLatestRecordOp Standardize<TObject>(
            this GetLatestObjectOp<TObject> operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            var result = new StandardGetLatestRecordOp(
                operation.IdentifierType,
                typeof(TObject).ToRepresentation(),
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy,
                specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardGetLatestRecordByIdOp Standardize<TId, TObject>(
            this GetLatestRecordByIdOp<TId, TObject> operation,
            IStringSerialize serializer,
            IResourceLocator specifiedResourceLocator = null)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new StandardGetLatestRecordByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                typeof(TObject).ToRepresentation(),
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy,
                specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardGetLatestRecordByIdOp Standardize<TId>(
            this GetLatestRecordByIdOp<TId> operation,
            IStringSerialize serializer,
            IResourceLocator specifiedResourceLocator = null)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new StandardGetLatestRecordByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                operation.ObjectType,
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy,
                specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="serializer">The serializer for the identifier.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardGetLatestRecordMetadataByIdOp Standardize<TId>(
            this GetLatestRecordMetadataByIdOp<TId> operation,
            IStringSerialize serializer,
            IResourceLocator specifiedResourceLocator = null)
        {
            var serializedObjectId = serializer.SerializeToString(operation.Id);

            var result = new StandardGetLatestRecordMetadataByIdOp(
                serializedObjectId,
                typeof(TId).ToRepresentation(),
                operation.ObjectType,
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy,
                specifiedResourceLocator);

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
        public static StandardGetLatestRecordOp Standardize<TObject>(
            this GetLatestRecordOp<TObject> operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            var result = new StandardGetLatestRecordOp(
                operation.IdentifierType,
                typeof(TObject).ToRepresentation(),
                operation.VersionMatchStrategy,
                operation.RecordNotFoundStrategy,
                specifiedResourceLocator);

            return result;
        }
    }
}
