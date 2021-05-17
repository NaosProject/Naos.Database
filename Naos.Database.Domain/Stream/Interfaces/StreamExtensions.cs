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
    using OBeautifulCode.Representation.System;

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
        /// <param name="existingRecordEncounteredStrategy">Optional strategy for an existing record.</param>
        /// <param name="recordRetentionCount">Optional number of existing records to retain if <paramref name="existingRecordEncounteredStrategy"/> is set to prune.</param>
        /// <param name="typeVersionMatchStrategy">The optional type version match strategy; DEFAULT is any version.</param>
        public static void Put<TObject>(
            this IWriteOnlyStream stream,
            TObject objectToPut,
            IReadOnlyDictionary<string, string> tags = null,
            ExistingRecordEncounteredStrategy existingRecordEncounteredStrategy = ExistingRecordEncounteredStrategy.None,
            int? recordRetentionCount = null,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new PutOp<TObject>(objectToPut, tags, existingRecordEncounteredStrategy, recordRetentionCount, typeVersionMatchStrategy);
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
        /// <param name="existingRecordEncounteredStrategy">Optional strategy for an existing record.</param>
        /// <param name="recordRetentionCount">Optional number of existing records to retain if <paramref name="existingRecordEncounteredStrategy"/> is set to prune.</param>
        /// <param name="typeVersionMatchStrategy">The optional type version match strategy; DEFAULT is any version.</param>
        /// <returns>Task for async.</returns>
        public static async Task PutAsync<TObject>(
            this IWriteOnlyStream stream,
            TObject objectToPut,
            IReadOnlyDictionary<string, string> tags = null,
            ExistingRecordEncounteredStrategy existingRecordEncounteredStrategy = ExistingRecordEncounteredStrategy.None,
            int? recordRetentionCount = null,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new PutOp<TObject>(objectToPut, tags, existingRecordEncounteredStrategy, recordRetentionCount, typeVersionMatchStrategy);
            var protocol = stream.GetStreamWritingProtocols<TObject>();
            await protocol.ExecuteAsync(operation);
        }

        /// <summary>
        /// Wraps <see cref="PutWithIdOp{TId,TObject}"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="tags">The optional tags.</param>
        /// <param name="existingRecordEncounteredStrategy">Optional strategy for an existing record.</param>
        /// <param name="recordRetentionCount">Optional number of existing records to retain if <paramref name="existingRecordEncounteredStrategy"/> is set to prune.</param>
        /// <param name="typeVersionMatchStrategy">The optional type version match strategy; DEFAULT is any version.</param>
        public static void PutWithId<TId, TObject>(
            this IWriteOnlyStream stream,
            TId id,
            TObject objectToPut,
            IReadOnlyDictionary<string, string> tags = null,
            ExistingRecordEncounteredStrategy existingRecordEncounteredStrategy = ExistingRecordEncounteredStrategy.None,
            int? recordRetentionCount = null,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new PutWithIdOp<TId, TObject>(id, objectToPut, tags, existingRecordEncounteredStrategy, recordRetentionCount, typeVersionMatchStrategy);
            var protocol = stream.GetStreamWritingWithIdProtocols<TId, TObject>();
            protocol.Execute(operation);
        }

        /// <summary>
        /// Wraps <see cref="PutWithIdOp{TId, TObject}"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="tags">The optional tags.</param>
        /// <param name="existingRecordEncounteredStrategy">Optional strategy for an existing record.</param>
        /// <param name="recordRetentionCount">Optional number of existing records to retain if <paramref name="existingRecordEncounteredStrategy"/> is set to prune.</param>
        /// <param name="typeVersionMatchStrategy">The optional type version match strategy; DEFAULT is any version.</param>
        /// <returns>Task for async.</returns>
        public static async Task PutWithIdAsync<TId, TObject>(
            this IWriteOnlyStream stream,
            TId id,
            TObject objectToPut,
            IReadOnlyDictionary<string, string> tags = null,
            ExistingRecordEncounteredStrategy existingRecordEncounteredStrategy = ExistingRecordEncounteredStrategy.None,
            int? recordRetentionCount = null,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new PutWithIdOp<TId, TObject>(id, objectToPut, tags, existingRecordEncounteredStrategy, recordRetentionCount, typeVersionMatchStrategy);
            var protocol = stream.GetStreamWritingWithIdProtocols<TId, TObject>();
            await protocol.ExecuteAsync(operation);
        }

        /// <summary>
        /// Wraps <see cref="GetLatestObjectByIdOp{TId,TObject}"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="typeVersionMatchStrategy">The optional type version match strategy; DEFAULT is 'Any'.</param>
        /// <param name="existingRecordNotEncounteredStrategy">The optional strategy on how to deal with no matching record; DEFAULT is the default of the requested type or null.</param>
        /// <returns>The object.</returns>
        public static TObject GetLatestObjectById<TId, TObject>(
            this IReadOnlyStream stream,
            TId identifier,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any,
            ExistingRecordNotEncounteredStrategy existingRecordNotEncounteredStrategy = ExistingRecordNotEncounteredStrategy.ReturnDefault)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetLatestObjectByIdOp<TId, TObject>(identifier, typeVersionMatchStrategy, existingRecordNotEncounteredStrategy);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId, TObject>();
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Wraps <see cref="GetLatestObjectByIdOp{TId,TObject}"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="typeVersionMatchStrategy">The optional type version match strategy; DEFAULT is 'Any'.</param>
        /// <param name="existingRecordNotEncounteredStrategy">The optional strategy on how to deal with no matching record; DEFAULT is the default of the requested type or null.</param>
        /// <returns>The object.</returns>
        public static async Task<TObject> GetLatestObjectByIdAsync<TId, TObject>(
            this IReadOnlyStream stream,
            TId identifier,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any,
            ExistingRecordNotEncounteredStrategy existingRecordNotEncounteredStrategy = ExistingRecordNotEncounteredStrategy.ReturnDefault)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetLatestObjectByIdOp<TId, TObject>(identifier, typeVersionMatchStrategy, existingRecordNotEncounteredStrategy);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId, TObject>();
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Wraps <see cref="DoesAnyExistByIdOp{TId}"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="objectType">The optional type of the object.</param>
        /// <param name="typeVersionMatchStrategy">The optional type version match strategy; DEFAULT is 'Any'.</param>
        /// <returns>The object.</returns>
        public static bool DoesAnyExistById<TId>(
            this IReadOnlyStream stream,
            TId identifier,
            TypeRepresentation objectType = null,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new DoesAnyExistByIdOp<TId>(identifier, objectType, typeVersionMatchStrategy);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId>();
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Wraps <see cref="DoesAnyExistByIdOp{TId}"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="objectType">The optional type of the object.</param>
        /// <param name="typeVersionMatchStrategy">The optional type version match strategy; DEFAULT is 'Any'.</param>
        /// <returns>The object.</returns>
        public static async Task<bool> DoesAnyExistByIdAsync<TId>(
            this IReadOnlyStream stream,
            TId identifier,
            TypeRepresentation objectType = null,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new DoesAnyExistByIdOp<TId>(identifier, objectType, typeVersionMatchStrategy);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId>();
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Wraps <see cref="GetLatestRecordMetadataByIdOp{TId}"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        /// <param name="existingRecordNotEncounteredStrategy">The existing record not encountered strategy.</param>
        /// <returns>Matching stream record metadata.</returns>
        public static StreamRecordMetadata<TId> GetLatestRecordMetadataById<TId>(
            this IReadOnlyStream stream,
            TId identifier,
            TypeRepresentation objectType = null,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any,
            ExistingRecordNotEncounteredStrategy existingRecordNotEncounteredStrategy = ExistingRecordNotEncounteredStrategy.ReturnDefault)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetLatestRecordMetadataByIdOp<TId>(identifier, objectType, typeVersionMatchStrategy, existingRecordNotEncounteredStrategy);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId>();
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Wraps <see cref="GetAllRecordsByIdOp{TId}"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        /// <param name="existingRecordNotEncounteredStrategy">The existing record not encountered strategy.</param>
        /// <param name="orderRecordsStrategy">The order records strategy.</param>
        /// <returns>Matching stream record .</returns>
        public static IReadOnlyList<StreamRecordWithId<TId>> GetAllRecordsById<TId>(
            this IReadOnlyStream stream,
            TId identifier,
            TypeRepresentation objectType = null,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any,
            ExistingRecordNotEncounteredStrategy existingRecordNotEncounteredStrategy = ExistingRecordNotEncounteredStrategy.ReturnDefault,
            OrderRecordsStrategy orderRecordsStrategy = OrderRecordsStrategy.ByInternalRecordIdAscending)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetAllRecordsByIdOp<TId>(identifier, objectType, typeVersionMatchStrategy, existingRecordNotEncounteredStrategy, orderRecordsStrategy);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId>();
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Wraps <see cref="GetAllRecordsByIdOp{TId}"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        /// <param name="existingRecordNotEncounteredStrategy">The existing record not encountered strategy.</param>
        /// <param name="orderRecordsStrategy">The order records strategy.</param>
        /// <returns>Matching stream record .</returns>
        public static async Task<IReadOnlyList<StreamRecordWithId<TId>>> GetAllRecordsByIdAsync<TId>(
            this IReadOnlyStream stream,
            TId identifier,
            TypeRepresentation objectType = null,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any,
            ExistingRecordNotEncounteredStrategy existingRecordNotEncounteredStrategy = ExistingRecordNotEncounteredStrategy.ReturnDefault,
            OrderRecordsStrategy orderRecordsStrategy = OrderRecordsStrategy.ByInternalRecordIdAscending)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetAllRecordsByIdOp<TId>(identifier, objectType, typeVersionMatchStrategy, existingRecordNotEncounteredStrategy, orderRecordsStrategy);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId>();
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Wraps <see cref="GetAllRecordsMetadataByIdOp{TId}"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        /// <param name="existingRecordNotEncounteredStrategy">The existing record not encountered strategy.</param>
        /// <param name="orderRecordsStrategy">The order records strategy.</param>
        /// <returns>Matching stream record metadata.</returns>
        public static IReadOnlyList<StreamRecordMetadata<TId>> GetAllRecordsMetadataById<TId>(
            this IReadOnlyStream stream,
            TId identifier,
            TypeRepresentation objectType = null,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any,
            ExistingRecordNotEncounteredStrategy existingRecordNotEncounteredStrategy = ExistingRecordNotEncounteredStrategy.ReturnDefault,
            OrderRecordsStrategy orderRecordsStrategy = OrderRecordsStrategy.ByInternalRecordIdAscending)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetAllRecordsMetadataByIdOp<TId>(identifier, objectType, typeVersionMatchStrategy, existingRecordNotEncounteredStrategy, orderRecordsStrategy);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId>();
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Wraps <see cref="GetAllRecordsMetadataByIdOp{TId}"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        /// <param name="existingRecordNotEncounteredStrategy">The existing record not encountered strategy.</param>
        /// <param name="orderRecordsStrategy">The order records strategy.</param>
        /// <returns>Matching stream record metadata.</returns>
        public static async Task<IReadOnlyList<StreamRecordMetadata<TId>>> GetAllRecordsMetadataByIdAsync<TId>(
            this IReadOnlyStream stream,
            TId identifier,
            TypeRepresentation objectType = null,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any,
            ExistingRecordNotEncounteredStrategy existingRecordNotEncounteredStrategy = ExistingRecordNotEncounteredStrategy.ReturnDefault,
            OrderRecordsStrategy orderRecordsStrategy = OrderRecordsStrategy.ByInternalRecordIdAscending)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetAllRecordsMetadataByIdOp<TId>(identifier, objectType, typeVersionMatchStrategy, existingRecordNotEncounteredStrategy, orderRecordsStrategy);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId>();
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Wraps <see cref="GetLatestRecordMetadataByIdOp{TId}"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        /// <param name="existingRecordNotEncounteredStrategy">The existing record not encountered strategy.</param>
        /// <returns>Matching stream record metadata.</returns>
        public static async Task<StreamRecordMetadata<TId>> GetLatestRecordMetadataByIdAsync<TId>(
            this IReadOnlyStream stream,
            TId identifier,
            TypeRepresentation objectType = null,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any,
            ExistingRecordNotEncounteredStrategy existingRecordNotEncounteredStrategy = ExistingRecordNotEncounteredStrategy.ReturnDefault)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetLatestRecordMetadataByIdOp<TId>(identifier, objectType, typeVersionMatchStrategy, existingRecordNotEncounteredStrategy);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId>();
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }
    }
}
