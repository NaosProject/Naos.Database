// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullStandardStreamReadWriteProtocols{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
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
    }
}