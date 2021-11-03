// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryHandleStandardizeExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Representation.System;

    /// <summary>
    /// For converting the various flavors down to <see cref="StandardTryHandleRecordOp"/>.
    /// </summary>
    public static class TryHandleStandardizeExtensions
    {
        /// <summary>
        /// Converts to common base format <see cref="StandardTryHandleRecordOp"/>.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <returns>TryHandleRecordOp.</returns>
        public static StandardTryHandleRecordOp Standardize<TObject>(this TryHandleRecordOp<TObject> operation)
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
                null);

            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="StandardTryHandleRecordOp"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <returns>TryHandleRecordOp.</returns>
        public static StandardTryHandleRecordOp Standardize<TId, TObject>(
            this TryHandleRecordWithIdOp<TId, TObject> operation)
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
                null);

            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="StandardTryHandleRecordOp"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <returns>TryHandleRecordOp.</returns>
        public static StandardTryHandleRecordOp Standardize<TId>(
            this TryHandleRecordWithIdOp<TId> operation)
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
                null);

            return result;
        }
    }
}
