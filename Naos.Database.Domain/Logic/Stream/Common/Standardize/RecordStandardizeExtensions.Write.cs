// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordStandardizeExtensions.Write.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;

    public static partial class RecordStandardizeExtensions
    {
        /// <summary>
        /// Converts to common base format <see cref="StandardGetNextUniqueLongOp"/>.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        /// <returns>The standardized operation.</returns>
        public static StandardGetNextUniqueLongOp Standardize(
            this GetNextUniqueLongOp operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            var result = new StandardGetNextUniqueLongOp(operation.Details, specifiedResourceLocator);

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
        public static StandardPutRecordOp Standardize<TObject>(
            this PutAndReturnInternalRecordIdOp<TObject> operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            throw new NotImplementedException("This operation is transformed and then delegated to other operations, eventually making it's way into a PutWithIdAndReturnInternalRecordIdOp<TId, TObject>.");
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
        public static StandardPutRecordOp Standardize<TObject>(
            this PutOp<TObject> operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            throw new NotImplementedException("This operation is transformed and then delegated to other operations, eventually making it's way into a PutWithIdAndReturnInternalRecordIdOp<TId, TObject>.");
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
        public static StandardPutRecordOp Standardize<TId, TObject>(
            this PutWithIdAndReturnInternalRecordIdOp<TId, TObject> operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            throw new NotImplementedException("We need to migrate the standardizing behavior out of the wrapping protocol.");
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
        public static StandardPutRecordOp Standardize<TId, TObject>(
            this PutWithIdOp<TId, TObject> operation,
            IResourceLocator specifiedResourceLocator = null)
        {
            throw new NotImplementedException("This operation is transformed and then delegated to other operations, eventually making it's way into a PutWithIdAndReturnInternalRecordIdOp<TId, TObject>.");
        }
    }
}
