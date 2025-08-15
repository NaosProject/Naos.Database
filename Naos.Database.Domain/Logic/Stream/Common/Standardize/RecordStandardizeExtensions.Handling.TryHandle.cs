// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordStandardizeExtensions.Handling.TryHandle.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;

    public static partial class RecordStandardizeExtensions
    {
        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardTryHandleRecordOp Standardize<TObject>(
            this TryHandleRecordOp<TObject> operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new StandardTryHandleRecordOp(
                operation.Concern,
                new RecordFilter(
                    idTypes: new[]
                    {
                        operation.IdentifierType,
                    },
                    objectTypes: new[]
                    {
                        typeof(TObject).ToRepresentation(),
                    },
                    versionMatchStrategy: operation.VersionMatchStrategy,
                    tags: operation.TagsToMatch,
                    tagMatchStrategy: operation.TagMatchStrategy),
                operation.OrderRecordsBy,
                operation.Details,
                operation.MinimumInternalRecordId,
                operation.InheritRecordTags,
                operation.Tags,
                StreamRecordItemsToInclude.MetadataAndPayload,
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
        public static StandardTryHandleRecordOp Standardize<TId, TObject>(
            this TryHandleRecordWithIdOp<TId, TObject> operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new StandardTryHandleRecordOp(
                operation.Concern,
                new RecordFilter(
                    idTypes: new[]
                    {
                        typeof(TId).ToRepresentation(),
                    },
                    objectTypes: new[]
                    {
                        typeof(TObject).ToRepresentation(),
                    },
                    versionMatchStrategy: operation.VersionMatchStrategy,
                    tags: operation.TagsToMatch,
                    tagMatchStrategy: operation.TagMatchStrategy),
                operation.OrderRecordsBy,
                operation.Details,
                operation.MinimumInternalRecordId,
                operation.InheritRecordTags,
                operation.Tags,
                StreamRecordItemsToInclude.MetadataAndPayload,
                specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardTryHandleRecordOp Standardize<TId>(
            this TryHandleRecordWithIdOp<TId> operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new StandardTryHandleRecordOp(
                operation.Concern,
                new RecordFilter(
                    idTypes: new[]
                    {
                        typeof(TId).ToRepresentation(),
                    },
                    objectTypes: new[]
                    {
                        operation.ObjectType,
                    },
                    versionMatchStrategy: operation.VersionMatchStrategy,
                    tags: operation.TagsToMatch,
                    tagMatchStrategy: operation.TagMatchStrategy),
                operation.OrderRecordsBy,
                operation.Details,
                operation.MinimumInternalRecordId,
                operation.InheritRecordTags,
                operation.Tags,
                StreamRecordItemsToInclude.MetadataAndPayload,
                specifiedResourceLocator);

            return result;
        }
    }
}
