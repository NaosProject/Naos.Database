// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordStandardizeExtensions.Management.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
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
        public static StandardPruneStreamOp Standardize(
            this PruneBeforeInternalRecordDateOp operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            var result = new StandardPruneStreamOp(
                null,
                operation.InternalRecordDate,
                operation.Details,
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
        public static StandardPruneStreamOp Standardize(
            this PruneBeforeInternalRecordIdOp operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            var result = new StandardPruneStreamOp(
                operation.InternalRecordId,
                null,
                operation.Details,
                specifiedResourceLocator);

            return result;
        }
    }
}
