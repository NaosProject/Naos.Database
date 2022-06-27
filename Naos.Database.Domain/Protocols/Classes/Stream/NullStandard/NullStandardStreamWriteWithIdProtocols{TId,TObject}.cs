// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullStandardStreamWriteWithIdProtocols{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// A null object pattern implementation of an <see cref="IStreamWriteWithIdProtocols{TId,TObject}"/>.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "'Stream' is the best term we could come up with; it's potential confusion with System.IO.Stream was debated.")]
    public class NullStandardStreamWriteWithIdProtocols<TId, TObject> : IStreamWriteWithIdProtocols<TId, TObject>
    {
        /// <inheritdoc />
        public long? Execute(
            PutWithIdAndReturnInternalRecordIdOp<TId, TObject> operation)
        {
            return null;
        }

        /// <inheritdoc />
        public async Task<long?> ExecuteAsync(
            PutWithIdAndReturnInternalRecordIdOp<TId, TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public void Execute(
            PutWithIdOp<TId, TObject> operation)
        {
            /* no-op */
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PutWithIdOp<TId, TObject> operation)
        {
            /* no-op */
            await Task.FromResult(false);
        }
    }
}
