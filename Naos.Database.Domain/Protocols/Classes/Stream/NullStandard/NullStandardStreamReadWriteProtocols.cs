// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullStandardStreamReadWriteProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Null object.
    /// Implements the <see cref="IStreamReadProtocols" />
    /// Implements the <see cref="IStreamWriteProtocols" />.
    /// </summary>
    public class NullStandardStreamReadWriteProtocols : IStreamReadProtocols, IStreamWriteProtocols
    {
        private readonly NullStandardStream nullStandardStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullStandardStreamReadWriteProtocols"/> class.
        /// </summary>
        /// <param name="nullStandardStream">The null standard stream.</param>
        public NullStandardStreamReadWriteProtocols(
            NullStandardStream nullStandardStream)
        {
            this.nullStandardStream = nullStandardStream;

            nullStandardStream.MustForArg(nameof(nullStandardStream)).NotBeNull();
        }

        /// <inheritdoc />
        public long Execute(
            GetNextUniqueLongOp operation)
        {
            var standard = operation.Standardize();
            var result = this.nullStandardStream.Execute(standard);
            return result;
        }

        /// <inheritdoc />
        public async Task<long> ExecuteAsync(
            GetNextUniqueLongOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }
    }
}