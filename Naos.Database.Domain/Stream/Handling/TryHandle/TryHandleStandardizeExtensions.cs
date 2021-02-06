// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryHandleStandardizeExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Representation.System;

    /// <summary>
    /// For converting the various flavors down to <see cref="TryHandleRecordOp"/>.
    /// </summary>
    public static class TryHandleStandardizeExtensions
    {
        /// <summary>
        /// Converts to common base format <see cref="TryHandleRecordOp"/>.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <returns>TryHandleRecordOp.</returns>
        public static TryHandleRecordOp Standardize<TObject>(this TryHandleRecordOp<TObject> operation)
        {
            var result = new TryHandleRecordOp(
                operation.Concern,
                operation.IdentifierType,
                typeof(TObject).ToRepresentation(),
                operation.TypeVersionMatchStrategy,
                operation.OrderRecordsStrategy,
                operation.SpecifiedResourceLocator,
                operation.Tags,
                operation.Details,
                operation.MinimumInternalRecordId);

            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="TryHandleRecordOp"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <returns>TryHandleRecordOp.</returns>
        public static TryHandleRecordOp Standardize<TId, TObject>(
            this TryHandleRecordWithIdOp<TId, TObject> operation)
        {
            var result = new TryHandleRecordOp(
                operation.Concern,
                typeof(TId).ToRepresentation(),
                typeof(TObject).ToRepresentation(),
                operation.TypeVersionMatchStrategy,
                operation.OrderRecordsStrategy,
                operation.SpecifiedResourceLocator,
                operation.Tags,
                operation.Details,
                operation.MinimumInternalRecordId);

            return result;
        }

        /// <summary>
        /// Converts to common base format <see cref="TryHandleRecordOp"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="operation">The operation.</param>
        /// <returns>TryHandleRecordOp.</returns>
        public static TryHandleRecordOp Standardize<TId>(
            this TryHandleRecordWithIdOp<TId> operation)
        {
            var result = new TryHandleRecordOp(
                operation.Concern,
                typeof(TId).ToRepresentation(),
                operation.ObjectType,
                operation.TypeVersionMatchStrategy,
                operation.OrderRecordsStrategy,
                operation.SpecifiedResourceLocator,
                operation.Tags,
                operation.Details,
                operation.MinimumInternalRecordId);

            return result;
        }
    }
}
