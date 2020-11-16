// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Extension methods on <see cref="IReadOnlyStream"/>, <see cref="IWriteOnlyStream"/>, <see cref="IStream"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public static class StreamExtensions
    {
        /// <summary>
        /// Wraps <see cref="GetNextUniqueLongOp"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>Result of <see cref="GetNextUniqueLongOp"/> execution.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "long", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
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
        /// Wraps <see cref="GetNextUniqueLongOp"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>Result of <see cref="GetNextUniqueLongOp"/> execution.</returns>
        public static async Task<long> GetNextUniqueLongAsync(
            this IWriteOnlyStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetNextUniqueLongOp();
            var protocol = stream.GetStreamWritingProtocols();
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Wraps <see cref="PutOp{TObject}"/>.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="tags">The optional tags.</param>
        public static void Put<TObject>(
            this IWriteOnlyStream stream,
            TObject objectToPut,
            IReadOnlyDictionary<string, string> tags = null)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new PutOp<TObject>(objectToPut, tags);
            var protocol = stream.GetStreamWritingProtocols<TObject>();
            protocol.Execute(operation);
        }

        /// <summary>
        /// Wraps <see cref="PutOp{TObject}"/>.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="tags">The optional tags.</param>
        /// <returns>Task for async.</returns>
        public static async Task PutAsync<TObject>(
            this IWriteOnlyStream stream,
            TObject objectToPut,
            IReadOnlyDictionary<string, string> tags = null)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new PutOp<TObject>(objectToPut, tags);
            var protocol = stream.GetStreamWritingProtocols<TObject>();
            await protocol.ExecuteAsync(operation);
        }

        /// <summary>
        /// Wraps <see cref="PutOp{TId, TObject}"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="tags">The optional tags.</param>
        public static void Put<TId, TObject>(
            this IWriteOnlyStream stream,
            TId id,
            TObject objectToPut,
            IReadOnlyDictionary<string, string> tags = null)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new PutOp<TId, TObject>(id, objectToPut, tags);
            var protocol = stream.GetStreamWritingProtocols<TId, TObject>();
            protocol.Execute(operation);
        }

        /// <summary>
        /// Wraps <see cref="PutOp{TObject}"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="tags">The optional tags.</param>
        /// <returns>Task for async.</returns>
        public static async Task PutAsync<TId, TObject>(
            this IWriteOnlyStream stream,
            TId id,
            TObject objectToPut,
            IReadOnlyDictionary<string, string> tags = null)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new PutOp<TId, TObject>(id, objectToPut, tags);
            var protocol = stream.GetStreamWritingProtocols<TId, TObject>();
            await protocol.ExecuteAsync(operation);
        }

        /// <summary>
        /// Wraps <see cref="GetLatestByIdAndTypeOp{TId,TObject}"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="typeVersionMatchStrategy">The optional type version match strategy; DEFAULT is 'Any'.</param>
        /// <returns>The object.</returns>
        public static TObject GetLatestByIdAndType<TId, TObject>(
            this IReadOnlyStream stream,
            TId identifier,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetLatestByIdAndTypeOp<TId, TObject>(identifier, typeVersionMatchStrategy);
            var protocol = stream.GetStreamReadingProtocols<TId, TObject>();
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Wraps <see cref="GetLatestByIdAndTypeOp{TId,TObject}"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="typeVersionMatchStrategy">The optional type version match strategy; DEFAULT is 'Any'.</param>
        /// <returns>The object.</returns>
        public static async Task<TObject> GetLatestByIdAndTypeAsync<TId, TObject>(
            this IReadOnlyStream stream,
            TId identifier,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetLatestByIdAndTypeOp<TId, TObject>(identifier, typeVersionMatchStrategy);
            var protocol = stream.GetStreamReadingProtocols<TId, TObject>();
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }
    }
}
