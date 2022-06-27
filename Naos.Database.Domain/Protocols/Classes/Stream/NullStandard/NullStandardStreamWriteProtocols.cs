// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullStandardStreamWriteProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    /// <summary>
    /// A null object pattern implementation of an <see cref="IStreamWriteProtocols"/>.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "'Stream' is the best term we could come up with; it's potential confusion with System.IO.Stream was debated.")]
    public class NullStandardStreamWriteProtocols : IStreamWriteProtocols
    {
        private static readonly Random Random = new Random();

        /// <inheritdoc />
        public long Execute(
            GetNextUniqueLongOp operation)
        {
            var result = Random.Next();
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
