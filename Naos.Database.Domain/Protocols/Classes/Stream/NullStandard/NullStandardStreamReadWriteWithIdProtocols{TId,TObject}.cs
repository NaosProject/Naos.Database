// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullStandardStreamReadWriteWithIdProtocols{TId,TObject}.cs" company="Naos Project">
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
    /// Implements the <see cref="IStreamReadWithIdProtocols{TId, TObject}" />
    /// Implements the <see cref="IStreamWriteWithIdProtocols{TId, TObject}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    internal class NullStandardStreamReadWriteWithIdProtocols<TId, TObject> : IStreamReadWithIdProtocols<TId, TObject>, IStreamWriteWithIdProtocols<TId, TObject>
    {
        // ReSharper disable once NotAccessedField.Local
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Keeping for future use.")]
        private NullStandardStream nullStandardStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullStandardStreamReadWriteWithIdProtocols{TId, TObject}"/> class.
        /// </summary>
        /// <param name="nullStandardStream">The null standard stream.</param>
        public NullStandardStreamReadWriteWithIdProtocols(NullStandardStream nullStandardStream)
        {
            nullStandardStream.MustForArg(nameof(nullStandardStream)).NotBeNull();

            this.nullStandardStream = nullStandardStream;
        }

        /// <inheritdoc />
        public TObject Execute(
            GetLatestObjectByIdOp<TId, TObject> operation)
        {
            return default;
        }

        /// <inheritdoc />
        public async Task<TObject> ExecuteAsync(
            GetLatestObjectByIdOp<TId, TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<TObject> Execute(
            GetLatestObjectsByIdOp<TId, TObject> operation)
        {
            return default;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<TObject>> ExecuteAsync(
            GetLatestObjectsByIdOp<TId, TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId, TObject> Execute(
            GetLatestRecordByIdOp<TId, TObject> operation)
        {
            return null;
        }

        /// <inheritdoc />
        public async Task<StreamRecordWithId<TId, TObject>> ExecuteAsync(
            GetLatestRecordByIdOp<TId, TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

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

        /// <inheritdoc />
        public bool Execute(
            DoesAnyExistByIdOp<TId, TObject> operation)
        {
            return false;
        }

        /// <inheritdoc />
        public async Task<bool> ExecuteAsync(
            DoesAnyExistByIdOp<TId, TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<TObject> Execute(
            GetAllObjectsByIdOp<TId, TObject> operation)
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var result = new List<TObject>();

            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<TObject>> ExecuteAsync(
            GetAllObjectsByIdOp<TId, TObject> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }
    }
}