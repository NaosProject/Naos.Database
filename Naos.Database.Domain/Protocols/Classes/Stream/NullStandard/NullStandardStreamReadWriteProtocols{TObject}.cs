// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullStandardStreamReadWriteProtocols{TObject}.cs" company="Naos Project">
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
    /// Implements the <see cref="IStreamReadProtocols{TObject}" />
    /// Implements the <see cref="IStreamWriteProtocols{TObject}" />.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    internal class NullStandardStreamReadWriteProtocols<TObject> : IStreamReadProtocols<TObject>, IStreamWriteProtocols<TObject>
    {
        // ReSharper disable once NotAccessedField.Local
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Keeping for future use.")]
        private NullStandardStream nullStandardStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullStandardStreamReadWriteProtocols{TObject}"/> class.
        /// </summary>
        /// <param name="nullStandardStream">The null standard stream.</param>
        public NullStandardStreamReadWriteProtocols(NullStandardStream nullStandardStream)
        {
            nullStandardStream.MustForArg(nameof(nullStandardStream)).NotBeNull();

            this.nullStandardStream = nullStandardStream;
        }

        /// <inheritdoc />
        public TObject Execute(
            GetLatestObjectOp<TObject> operation)
        {
            return default;
        }

        /// <inheritdoc />
        public async Task<TObject> ExecuteAsync(
            GetLatestObjectOp<TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public TObject Execute(
            GetLatestObjectByTagsOp<TObject> operation)
        {
            return default;
        }

        /// <inheritdoc />
        public async Task<TObject> ExecuteAsync(
            GetLatestObjectByTagsOp<TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public StreamRecord<TObject> Execute(
            GetLatestRecordOp<TObject> operation)
        {
            return null;
        }

        /// <inheritdoc />
        public async Task<StreamRecord<TObject>> ExecuteAsync(
            GetLatestRecordOp<TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public long? Execute(
            PutAndReturnInternalRecordIdOp<TObject> operation)
        {
            return null;
        }

        /// <inheritdoc />
        public async Task<long?> ExecuteAsync(
            PutAndReturnInternalRecordIdOp<TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public void Execute(
            PutOp<TObject> operation)
        {
            /* no-op */
        }

        /// <inheritdoc />
        public async Task ExecuteAsync(
            PutOp<TObject> operation)
        {
            /* no-op */
            await Task.FromResult(false);
        }

        /// <inheritdoc />
        public IReadOnlyList<TObject> Execute(
            GetAllObjectsOp<TObject> operation)
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var result = new List<TObject>();

            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<TObject>> ExecuteAsync(
            GetAllObjectsOp<TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public IReadOnlyList<StreamRecord<TObject>> Execute(
            GetAllRecordsOp<TObject> operation)
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var result = new List<StreamRecord<TObject>>();

            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<StreamRecord<TObject>>> ExecuteAsync(
            GetAllRecordsOp<TObject> operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}