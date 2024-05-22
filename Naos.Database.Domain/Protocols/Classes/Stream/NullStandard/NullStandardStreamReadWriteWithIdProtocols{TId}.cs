// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullStandardStreamReadWriteWithIdProtocols{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Null object.
    /// Implements the <see cref="IStreamReadWithIdProtocols{TId}" />
    /// Implements the <see cref="IStreamWriteWithIdProtocols{TId}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    internal class NullStandardStreamReadWriteWithIdProtocols<TId> : IStreamReadWithIdProtocols<TId>, IStreamWriteWithIdProtocols<TId>
    {
        // ReSharper disable once NotAccessedField.Local
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Keeping for future use.")]
        private NullStandardStream nullStandardStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullStandardStreamReadWriteWithIdProtocols{TId}"/> class.
        /// </summary>
        /// <param name="nullStandardStream">The null standard stream.</param>
        public NullStandardStreamReadWriteWithIdProtocols(NullStandardStream nullStandardStream)
        {
            nullStandardStream.MustForArg(nameof(nullStandardStream)).NotBeNull();

            this.nullStandardStream = nullStandardStream;
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId> Execute(
            GetLatestRecordByIdOp<TId> operation)
        {
            return null;
        }

        /// <inheritdoc />
        public async Task<StreamRecordWithId<TId>> ExecuteAsync(
            GetLatestRecordByIdOp<TId> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordWithId<TId>> Execute(
            GetAllRecordsByIdOp<TId> operation)
        {
            return new List<StreamRecordWithId<TId>>();
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecordWithId<TId>>> ExecuteAsync(
            GetAllRecordsByIdOp<TId> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public StreamRecordMetadata<TId> Execute(
            GetLatestRecordMetadataByIdOp<TId> operation)
        {
            return null;
        }

        /// <inheritdoc />
        public async Task<StreamRecordMetadata<TId>> ExecuteAsync(
            GetLatestRecordMetadataByIdOp<TId> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecordMetadata<TId>> Execute(
            GetAllRecordsMetadataByIdOp<TId> operation)
        {
            return new List<StreamRecordMetadata<TId>>();
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecordMetadata<TId>>> ExecuteAsync(
            GetAllRecordsMetadataByIdOp<TId> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public bool Execute(
            DoesAnyExistByIdOp<TId> operation)
        {
            return false;
        }

        /// <inheritdoc />
        public async Task<bool> ExecuteAsync(
            DoesAnyExistByIdOp<TId> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public string Execute(
            GetLatestStringSerializedObjectByIdOp<TId> operation)
        {
            return null;
        }

        /// <inheritdoc />
        public async Task<string> ExecuteAsync(
            GetLatestStringSerializedObjectByIdOp<TId> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<TId> Execute(
            GetDistinctIdsOp<TId> operation)
        {
            return new List<TId>();
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<TId>> ExecuteAsync(
            GetDistinctIdsOp<TId> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}