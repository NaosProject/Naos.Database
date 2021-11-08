// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamReadWriteProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Set of protocols to execute read and write operations on a stream,
    /// without a typed identifier and without a typed record payload.
    /// </summary>
    public class StandardStreamReadWriteProtocols :
        IStreamReadProtocols,
        IStreamWriteProtocols
    {
        private readonly IStandardStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardStreamReadWriteProtocols"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public StandardStreamReadWriteProtocols(
            IStandardStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            this.stream = stream;
        }

        /// <inheritdoc />
        public long Execute(
            GetNextUniqueLongOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var standardOp = operation.Standardize();

            var result = this.stream.Execute(standardOp);

            return result;
        }

        /// <inheritdoc />
        public async Task<long> ExecuteAsync(
            GetNextUniqueLongOp operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }
    }
}
