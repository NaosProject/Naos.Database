// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStreamObjectOperationsProtocol{TObject}.cs" company="Naos Project">
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
    /// Implements the <see cref="Naos.Database.Domain.IStreamReadingProtocols{TObject}" />
    /// Implements the <see cref="Naos.Database.Domain.IStreamWritingProtocols{TObject}" />
    /// Implements the <see cref="Naos.Database.Domain.IStreamReadingProtocols{TId, TObject}" />
    /// Implements the <see cref="Naos.Database.Domain.IStreamWritingProtocols{TId, TObject}" />
    /// </summary>
    /// <typeparam name="TObject">The type of the t object.</typeparam>
    /// <seealso cref="Naos.Database.Domain.IStreamReadingProtocols{TObject}" />
    /// <seealso cref="Naos.Database.Domain.IStreamWritingProtocols{TObject}" />
    /// <seealso cref="Naos.Database.Domain.IStreamReadingProtocols{TId, TObject}" />
    /// <seealso cref="Naos.Database.Domain.IStreamWritingProtocols{TId, TObject}" />
    public partial class MemoryStreamProtocols<TObject> :
        IStreamReadingProtocols<TObject>,
        IStreamWritingProtocols<TObject>
    {
        private readonly MemoryStreamProtocols<NullStreamObjectIdentifier, TObject> chainingProtocols;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStreamProtocols{TObject}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public MemoryStreamProtocols(
            MemoryStream stream)
        {
            this.chainingProtocols = new MemoryStreamProtocols<NullStreamObjectIdentifier, TObject>(stream);
        }

        /// <inheritdoc />
        public void Execute(
            PutOp<TObject> operation)
        {
            var chainOperation = new PutAndReturnInternalRecordIdOp<TObject>(operation.ObjectToPut, operation.Tags);
            this.Execute(chainOperation);
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
            var chainOperation = new PutAndReturnInternalRecordIdOp<NullStreamObjectIdentifier, TObject>(null, operation.ObjectToPut, operation.Tags);
            var result = this.chainingProtocols.Execute(chainOperation);
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
