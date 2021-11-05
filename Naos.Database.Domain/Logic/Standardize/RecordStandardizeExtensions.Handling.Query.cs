// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordStandardizeExtensions.Handling.Query.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;

    public static partial class RecordStandardizeExtensions
    {
        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardGetHandlingStatusOp Standardize(
            this GetCompositeHandlingStatusByIdsOp operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            var result = new StandardGetHandlingStatusOp(
                operation.Concern,
                null,
                operation.IdsToMatch,
                operation.VersionMatchStrategy,
                specifiedResourceLocator: specifiedResourceLocator);

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
        public static StandardGetHandlingStatusOp Standardize<TId>(
            this GetCompositeHandlingStatusByIdsOp<TId> operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            throw new NotImplementedException("We need to migrate the standardizing behavior out of the wrapping protocol.");
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardGetHandlingStatusOp Standardize(
            this GetCompositeHandlingStatusByTagsOp operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            var result = new StandardGetHandlingStatusOp(
                operation.Concern,
                null,
                null,
                null,
                operation.TagsToMatch,
                operation.TagMatchStrategy,
                specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardGetHandlingStatusOp Standardize(
            this GetHandlingStatusOp operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            var result = new StandardGetHandlingStatusOp(
                operation.Concern,
                operation.InternalRecordId,
                specifiedResourceLocator: specifiedResourceLocator);

            return result;
        }

        /// <summary>
        /// Converts the operation to it's standardized form.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>
        /// The standardized operation.
        /// </returns>
        public static StandardGetHandlingHistoryOp Standardize(
            this GetHandlingHistoryOp operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            var result = new StandardGetHandlingHistoryOp(
                operation.InternalRecordId,
                operation.Concern,
                specifiedResourceLocator);

            return result;
        }
    }
}
