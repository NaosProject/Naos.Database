// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordStandardizeExtensions.Handling.Query.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;

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
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new StandardGetHandlingStatusOp(
                operation.Concern,
                new RecordFilter(ids: operation.IdsToMatch, versionMatchStrategy: operation.VersionMatchStrategy),
                new HandlingFilter(),
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
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "specifiedResourceLocator", Justification = "Added for consistency with other extension methods.")]
        public static StandardGetHandlingStatusOp Standardize<TId>(
            this GetCompositeHandlingStatusByIdsOp<TId> operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

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
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new StandardGetHandlingStatusOp(
                operation.Concern,
                new RecordFilter(tags: operation.TagsToMatch, tagMatchStrategy: operation.TagMatchStrategy),
                new HandlingFilter(),
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
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new StandardGetHandlingStatusOp(
                operation.Concern,
                new RecordFilter(
                    internalRecordIds: new[]
                    {
                        operation.InternalRecordId,
                    }),
                new HandlingFilter(),
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
        public static StandardGetHandlingHistoryOp Standardize(
            this GetHandlingHistoryOp operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new StandardGetHandlingHistoryOp(
                operation.InternalRecordId,
                operation.Concern,
                specifiedResourceLocator);

            return result;
        }
    }
}
