﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamRecordWithIdHandlingProtocols{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Threading.Tasks;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// Set of protocols to execute record handling operations
    /// with a typed identifier and with a typed record payload.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class StandardStreamRecordWithIdHandlingProtocols<TId, TObject> :
        IStreamRecordWithIdHandlingProtocols<TId, TObject>
    {
        private readonly IStandardStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardStreamRecordWithIdHandlingProtocols{TId, TObject}"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public StandardStreamRecordWithIdHandlingProtocols(
            IStandardStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
        }

        /// <inheritdoc />
        public StreamRecordWithId<TId, TObject> Execute(
            TryHandleRecordWithIdOp<TId, TObject> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            var tryHandleResult = this.stream.Execute(standardOp);

            var record = tryHandleResult.RecordToHandle;

            if (record == null)
            {
                return null;
            }

            var payload = record.GetDescribedSerialization().DeserializePayloadUsingSpecificFactory<TObject>(
                this.stream.SerializerFactory);

            var metadata = record.Metadata.ToStreamRecordMetadata<TId>(this.stream);

            var result = new StreamRecordWithId<TId, TObject>(record.InternalRecordId, metadata, payload);

            return result;
        }

        /// <inheritdoc />
        public async Task<StreamRecordWithId<TId, TObject>> ExecuteAsync(
            TryHandleRecordWithIdOp<TId, TObject> operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }
    }
}
