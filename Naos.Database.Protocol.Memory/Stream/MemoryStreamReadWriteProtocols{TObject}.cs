// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStreamReadWriteProtocols{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.Memory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Set of protocols to work with known identifier and/or object type.
    /// Implements the <see cref="IStreamReadProtocols{TObject}" />
    /// Implements the <see cref="IStreamWriteProtocols{TObject}" />
    /// Implements the <see cref="IStreamReadProtocols{TId,TObject}" />
    /// Implements the <see cref="IStreamWriteProtocols{TId,TObject}" />.
    /// </summary>
    /// <typeparam name="TObject">The type of the t object.</typeparam>
    /// <seealso cref="IStreamReadProtocols{TObject}" />
    /// <seealso cref="IStreamWriteProtocols{TObject}" />
    /// <seealso cref="IStreamReadProtocols{TId,TObject}" />
    /// <seealso cref="IStreamWriteProtocols{TId,TObject}" />
    public partial class MemoryStreamProtocols<TObject> :
        IStreamReadProtocols<TObject>,
        IStreamWriteProtocols<TObject>
    {
        private readonly MemoryStreamReadWriteProtocols<NullStreamIdentifier, TObject> delegatedProtocols;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStreamProtocols{TObject}"/> class.
        /// </summary>
        /// <param name="readWriteStream">The stream.</param>
        public MemoryStreamProtocols(
            MemoryReadWriteStream readWriteStream)
        {
            this.delegatedProtocols = new MemoryStreamReadWriteProtocols<NullStreamIdentifier, TObject>(readWriteStream);
        }

        /// <inheritdoc />
        public void Execute(
            PutOp<TObject> operation)
        {
            var delegatedOperation = new PutAndReturnInternalRecordIdOp<TObject>(operation.ObjectToPut, operation.Tags);
            this.Execute(delegatedOperation);
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PutOp<TObject> operation)
        {
            this.Execute(operation);
            await Task.FromResult(true); // just for await
        }

        /// <inheritdoc />
        public long Execute(
            PutAndReturnInternalRecordIdOp<TObject> operation)
        {
            var delegatedOperation = new PutAndReturnInternalRecordIdOp<NullStreamIdentifier, TObject>(null, operation.ObjectToPut, operation.Tags);
            var result = this.delegatedProtocols.Execute(delegatedOperation);
            return result;
        }

        /// <inheritdoc />
        public async Task<long> ExecuteAsync(
            PutAndReturnInternalRecordIdOp<TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}
