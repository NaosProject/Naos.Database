// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamReadWriteWithIdProtocols{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System;
    using System.Threading.Tasks;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// File system implementation of <see cref="IStreamReadWithIdProtocols{TId}"/>
    /// and <see cref="IStreamWriteProtocols{TId}"/>.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    public class FileStreamReadWriteWithIdProtocols<TId>
        : IStreamReadWithIdProtocols<TId>,
          IStreamWriteWithIdProtocols<TId>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "temp")]
        private FileReadWriteStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStreamReadWriteWithIdProtocols{TId}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public FileStreamReadWriteWithIdProtocols(FileReadWriteStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId> Execute(
            GetLatestRecordByIdOp<TId> operation)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task<StreamRecordWithId<TId>> ExecuteAsync(
            GetLatestRecordByIdOp<TId> operation)
        {
            throw new System.NotImplementedException();
        }
    }
}
