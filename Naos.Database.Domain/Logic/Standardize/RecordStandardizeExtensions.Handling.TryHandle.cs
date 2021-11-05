// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordStandardizeExtensions.Handling.TryHandle.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Representation.System;

    public static partial class RecordStandardizeExtensions
    {
        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <typeparam name="TObject">Type of the object in the record.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardTryHandleRecordOp Standardize<TObject>(
            this TryHandleRecordOp<TObject> operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            var result = new StandardTryHandleRecordOp(
                operation.Concern,
                operation.IdentifierType,
                typeof(TObject).ToRepresentation(),
                operation.VersionMatchStrategy,
                operation.OrderRecordsBy,
                operation.Tags,
                operation.Details,
                operation.MinimumInternalRecordId,
                operation.InheritRecordTags,
                specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <typeparam name="TId">Type of the identifier of the record.</typeparam>
        /// <typeparam name="TObject">Type of the object in the record.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardTryHandleRecordOp Standardize<TId, TObject>(
            this TryHandleRecordWithIdOp<TId, TObject> operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            var result = new StandardTryHandleRecordOp(
                operation.Concern,
                typeof(TId).ToRepresentation(),
                typeof(TObject).ToRepresentation(),
                operation.VersionMatchStrategy,
                operation.OrderRecordsBy,
                operation.Tags,
                operation.Details,
                operation.MinimumInternalRecordId,
                operation.InheritRecordTags,
                specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <typeparam name="TId">Type of the identifier of the record.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardTryHandleRecordOp Standardize<TId>(
            this TryHandleRecordWithIdOp<TId> operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            var result = new StandardTryHandleRecordOp(
                operation.Concern,
                typeof(TId).ToRepresentation(),
                operation.ObjectType,
                operation.VersionMatchStrategy,
                operation.OrderRecordsBy,
                operation.Tags,
                operation.Details,
                operation.MinimumInternalRecordId,
                operation.InheritRecordTags,
                specifiedResourceLocator);

            return result;
        }
    }
}
