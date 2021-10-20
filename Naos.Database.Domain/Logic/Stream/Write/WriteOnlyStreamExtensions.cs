// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteOnlyStreamExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Extension methods on <see cref="IWriteOnlyStream"/>.
    /// </summary>
    public static partial class WriteOnlyStreamExtensions
    {
        /// <summary>
        /// Wraps <see cref="PutOp{TObject}"/>.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="tags">The optional tags.</param>
        /// <param name="existingRecordEncounteredStrategy">Optional strategy for an existing record.</param>
        /// <param name="recordRetentionCount">Optional number of existing records to retain if <paramref name="existingRecordEncounteredStrategy"/> is set to prune.</param>
        /// <param name="versionMatchStrategy">The optional type version match strategy; DEFAULT is any version.</param>
        public static void Put<TObject>(
            this IWriteOnlyStream stream,
            TObject objectToPut,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            ExistingRecordEncounteredStrategy existingRecordEncounteredStrategy = ExistingRecordEncounteredStrategy.None,
            int? recordRetentionCount = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new PutOp<TObject>(objectToPut, tags, existingRecordEncounteredStrategy, recordRetentionCount, versionMatchStrategy);
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
        /// <param name="versionMatchStrategy">The optional type version match strategy; DEFAULT is any version.</param>
        /// <returns>Task for async.</returns>
        public static async Task PutAsync<TObject>(
            this IWriteOnlyStream stream,
            TObject objectToPut,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            ExistingRecordEncounteredStrategy existingRecordEncounteredStrategy = ExistingRecordEncounteredStrategy.None,
            int? recordRetentionCount = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new PutOp<TObject>(objectToPut, tags, existingRecordEncounteredStrategy, recordRetentionCount, versionMatchStrategy);
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
        /// <param name="versionMatchStrategy">The optional type version match strategy; DEFAULT is any version.</param>
        public static void PutWithId<TId, TObject>(
            this IWriteOnlyStream stream,
            TId id,
            TObject objectToPut,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            ExistingRecordEncounteredStrategy existingRecordEncounteredStrategy = ExistingRecordEncounteredStrategy.None,
            int? recordRetentionCount = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new PutWithIdOp<TId, TObject>(id, objectToPut, tags, existingRecordEncounteredStrategy, recordRetentionCount, versionMatchStrategy);
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
        /// <param name="versionMatchStrategy">The optional type version match strategy; DEFAULT is any version.</param>
        /// <returns>Task for async.</returns>
        public static async Task PutWithIdAsync<TId, TObject>(
            this IWriteOnlyStream stream,
            TId id,
            TObject objectToPut,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            ExistingRecordEncounteredStrategy existingRecordEncounteredStrategy = ExistingRecordEncounteredStrategy.None,
            int? recordRetentionCount = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new PutWithIdOp<TId, TObject>(id, objectToPut, tags, existingRecordEncounteredStrategy, recordRetentionCount, versionMatchStrategy);
            var protocol = stream.GetStreamWritingWithIdProtocols<TId, TObject>();
            await protocol.ExecuteAsync(operation);
        }
    }
}
