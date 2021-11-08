// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteOnlyStreamExtensions.PutWithIdAndReturnInternalRecordIdOp{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    public static partial class ReadOnlyStreamExtensions
    {
        /// <summary>
        /// Put an object into a stream.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="tags">OPTIONAL tags to put with the record.  DEFAULT is no tags.</param>
        /// <param name="existingRecordStrategy">OPTIONAL strategy to use when an existing record is encountered while writing.  DEFAULT is to put a new record regardless of any existing records.</param>
        /// <param name="recordRetentionCount">OPTIONAL number of existing records to retain if <paramref name="existingRecordStrategy"/> is set to prune.  DEFAULT is n/a.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type) when looking for existing records.  DEFAULT is no filter (any version is acceptable).</param>
        /// <returns>The internal record identifier or null if the object wasn't written.</returns>
        public static long? PutWithIdAndReturnInternalRecordId<TId, TObject>(
            this IWriteOnlyStream stream,
            TId id,
            TObject objectToPut,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            ExistingRecordStrategy existingRecordStrategy = ExistingRecordStrategy.None,
            int? recordRetentionCount = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new PutWithIdAndReturnInternalRecordIdOp<TId, TObject>(id, objectToPut, tags, existingRecordStrategy, recordRetentionCount, versionMatchStrategy);
            var protocol = stream.GetStreamWritingWithIdProtocols<TId, TObject>();
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// /// Put an object into a stream.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="tags">OPTIONAL tags to put with the record.  DEFAULT is no tags.</param>
        /// <param name="existingRecordStrategy">OPTIONAL strategy to use when an existing record is encountered while writing.  DEFAULT is to put a new record regardless of any existing records.</param>
        /// <param name="recordRetentionCount">OPTIONAL number of existing records to retain if <paramref name="existingRecordStrategy"/> is set to prune.  DEFAULT is n/a.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type) when looking for existing records.  DEFAULT is no filter (any version is acceptable).</param>
        /// <returns>The internal record identifier or null if the object wasn't written.</returns>
        public static async Task<long?> PutWithIdAndReturnInternalRecordIdAsync<TId, TObject>(
            this IWriteOnlyStream stream,
            TId id,
            TObject objectToPut,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            ExistingRecordStrategy existingRecordStrategy = ExistingRecordStrategy.None,
            int? recordRetentionCount = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new PutWithIdAndReturnInternalRecordIdOp<TId, TObject>(id, objectToPut, tags, existingRecordStrategy, recordRetentionCount, versionMatchStrategy);
            var protocol = stream.GetStreamWritingWithIdProtocols<TId, TObject>();
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }
    }
}
