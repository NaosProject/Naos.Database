// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStreamReadWriteWithIdProtocols{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.Memory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// Set of protocols to work with known identifier and/or object type.
    /// Implements the <see cref="IStreamReadWithIdProtocols{TId}" />
    /// Implements the <see cref="IStreamWriteProtocols{TId}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <seealso cref="IStreamReadWithIdProtocols{TId}" />
    /// <seealso cref="IStreamWriteWithIdProtocols{TId}" />
    public partial class MemoryStreamReadWriteWithIdProtocols<TId> :
        IStreamReadWithIdProtocols<TId>,
        IStreamWriteWithIdProtocols<TId>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "temp")]
        private readonly MemoryReadWriteStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStreamReadWriteWithIdProtocols{TId}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public MemoryStreamReadWriteWithIdProtocols(
            MemoryReadWriteStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId> Execute(
            GetLatestRecordByIdOp<TId> operation)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<StreamRecordWithId<TId>> ExecuteAsync(
            GetLatestRecordByIdOp<TId> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}
