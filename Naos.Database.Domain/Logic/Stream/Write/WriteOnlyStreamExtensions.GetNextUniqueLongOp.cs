// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteOnlyStreamExtensions.GetNextUniqueLongOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;

    public static partial class ReadOnlyStreamExtensions
    {
        /// <summary>
        /// Get a unique 64-bit integer, in sequence, that is brokered via an internal stream construct.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The next unique long.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "long", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public static long GetNextUniqueLong(
            this IWriteOnlyStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetNextUniqueLongOp();
            var protocol = stream.GetStreamWritingProtocols();
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Get a unique 64-bit integer, in sequence, that is brokered via an internal stream construct.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>The next unique long.</returns>
        public static async Task<long> GetNextUniqueLongAsync(
            this IWriteOnlyStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetNextUniqueLongOp();
            var protocol = stream.GetStreamWritingProtocols();
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }
    }
}
