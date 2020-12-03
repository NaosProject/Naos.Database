// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStreamRecordHandlingProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.Memory
{
    using System.Threading.Tasks;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;

    /// <summary>
    /// Set of protocols to handle <see cref="IEvent"/>'s in a stream.
    /// </summary>
    /// <seealso cref="IStreamReadProtocols" />
    /// <seealso cref="IStreamWriteProtocols" />
    public partial class MemoryStreamRecordHandlingProtocols :
        IStreamRecordHandlingProtocols
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Temporary.")]
        private readonly MemoryReadWriteStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStreamRecordHandlingProtocols"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public MemoryStreamRecordHandlingProtocols(
            MemoryReadWriteStream stream)
        {
            this.stream = stream;
        }

        /// <inheritdoc />
        public StreamRecord Execute(
            TryHandleRecordOp operation)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<StreamRecord> ExecuteAsync(
            TryHandleRecordOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}
