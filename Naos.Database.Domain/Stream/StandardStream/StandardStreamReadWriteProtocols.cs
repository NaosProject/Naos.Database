// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamReadWriteProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Set of protocols to work with known identifier and/or object type.
    /// Implements the <see cref="IStreamReadProtocols" />
    /// Implements the <see cref="IStreamWriteProtocols" />.
    /// </summary>
    /// <seealso cref="IStreamReadProtocols" />
    /// <seealso cref="IStreamWriteProtocols" />
    public class StandardStreamReadWriteProtocols :
        IStreamReadProtocols,
        IStreamWriteProtocols
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "temp")]
        private readonly IStandardReadWriteStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardStreamReadWriteProtocols"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public StandardStreamReadWriteProtocols(
            IStandardReadWriteStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
        }

        /// <inheritdoc />
        public StreamRecord Execute(
            GetLatestRecordOp operation)
        {
            var result = this.stream.Execute(operation);
            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecord> ExecuteAsync(
            GetLatestRecordOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public long Execute(
            GetNextUniqueLongOp operation)
        {
            return this.stream.Execute(operation);
        }

        /// <inheritdoc />
        public async Task<long> ExecuteAsync(
            GetNextUniqueLongOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public StreamRecord Execute(
            GetLatestRecordByIdOp operation)
        {
            var result = this.stream.Execute(operation);
            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecord> ExecuteAsync(
            GetLatestRecordByIdOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public long Execute(
            PutRecordOp operation)
        {
            var result = this.stream.Execute(operation);
            return result;
        }

        /// <inheritdoc />
        public async Task<long> ExecuteAsync(
            PutRecordOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public bool Execute(
            DoesAnyExistByIdOp operation)
        {
            var result = this.stream.Execute(operation);
            return result;
        }

        /// <inheritdoc />
        public async Task<bool> ExecuteAsync(
            DoesAnyExistByIdOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecord> Execute(
            GetAllRecordsByIdOp operation)
        {
            var result = this.stream.Execute(operation);
            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecord>> ExecuteAsync(
            GetAllRecordsByIdOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public StreamRecordMetadata Execute(
            GetLatestRecordMetadataByIdOp operation)
        {
            var result = this.stream.Execute(operation);
            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecordMetadata> ExecuteAsync(
            GetLatestRecordMetadataByIdOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordMetadata> Execute(
            GetAllRecordsMetadataByIdOp operation)
        {
            var result = this.stream.Execute(operation);
            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecordMetadata>> ExecuteAsync(
            GetAllRecordsMetadataByIdOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}
