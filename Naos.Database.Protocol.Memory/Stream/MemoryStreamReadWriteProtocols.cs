// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStreamReadWriteProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.Memory
{
    using System.Linq;
    using System.Threading.Tasks;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;

    /// <summary>
    /// Set of protocols to work with known identifier and/or object type.
    /// Implements the <see cref="IStreamReadProtocols" />
    /// Implements the <see cref="IStreamWriteProtocols" />.
    /// </summary>
    /// <seealso cref="IStreamReadProtocols" />
    /// <seealso cref="IStreamWriteProtocols" />
    public partial class MemoryStreamReadWriteProtocols :
        IStreamReadProtocols,
        IStreamWriteProtocols
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "temp")]
        private readonly MemoryReadWriteStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStreamReadWriteProtocols"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public MemoryStreamReadWriteProtocols(
            MemoryReadWriteStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
        }

        /// <inheritdoc />
        public StreamRecord Execute(
            GetLatestRecordOp operation)
        {
            StreamRecord result = null;
            this.stream.RunLockedOperationOnRecordList(
                records =>
                {
                    result =
                        records.OrderByDescending(_ => _.InternalRecordId)
                               .FirstOrDefault(
                                    _ => _.Metadata.FuzzyMatchTypes(
                                        operation.IdentifierType,
                                        operation.ObjectType,
                                        operation.TypeVersionMatchStrategy));
                });

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
            return await this.stream.ExecuteAsync(operation);
        }

        /// <inheritdoc />
        public StreamRecord Execute(
            GetLatestRecordByIdOp operation)
        {
            StreamRecord result = null;
            this.stream.RunLockedOperationOnRecordList(
                records =>
                {
                    result =
                        records.OrderByDescending(_ => _.InternalRecordId)
                               .FirstOrDefault(
                                    _ => _.Metadata.FuzzyMatchTypesAndId(
                                        operation.StringSerializedId,
                                        operation.IdentifierType,
                                        operation.ObjectType,
                                        operation.TypeVersionMatchStrategy));
                });

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
    }
}
