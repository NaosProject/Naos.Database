// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteOnlyStreamExtensions.PutAndReturnInternalRecordIdOp{TObject}.cs" company="Naos Project">
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
        /// Put an object into a stream and return the internal identifier of the record
        /// or null if the specified strategy for dealing with an existing record prevents the object from being written.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="tags">OPTIONAL tags to put with the record.  DEFAULT is no tags.</param>
        /// <param name="existingRecordStrategy">OPTIONAL strategy to use when an existing record is encountered while writing.  DEFAULT is to put a new record regardless of any existing records.</param>
        /// <param name="recordRetentionCount">OPTIONAL number of existing records to retain if <paramref name="existingRecordStrategy"/> is set to prune.  DEFAULT is n/a.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type) when looking for existing records.  DEFAULT is no filter (any version is acceptable).</param>
        /// <returns>The internal record identifier or null if the object wasn't written.</returns>
        public static long? PutAndReturnInternalRecordId<TObject>(
            this IWriteOnlyStream stream,
            TObject objectToPut,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            ExistingRecordStrategy existingRecordStrategy = ExistingRecordStrategy.None,
            int? recordRetentionCount = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new PutAndReturnInternalRecordIdOp<TObject>(objectToPut, tags, existingRecordStrategy, recordRetentionCount, versionMatchStrategy);
            var protocol = stream.GetStreamWritingProtocols<TObject>();
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Put an object into a stream and return the internal identifier of the record
        /// or null if the specified strategy for dealing with an existing record prevents the object from being written.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="tags">OPTIONAL tags to put with the record.  DEFAULT is no tags.</param>
        /// <param name="existingRecordStrategy">OPTIONAL strategy to use when an existing record is encountered while writing.  DEFAULT is to put a new record regardless of any existing records.</param>
        /// <param name="recordRetentionCount">OPTIONAL number of existing records to retain if <paramref name="existingRecordStrategy"/> is set to prune.  DEFAULT is n/a.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type) when looking for existing records.  DEFAULT is no filter (any version is acceptable).</param>
        /// <returns>The internal record identifier or null if the object wasn't written.</returns>
        public static async Task<long?> PutAndReturnInternalRecordIdAsync<TObject>(
            this IWriteOnlyStream stream,
            TObject objectToPut,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            ExistingRecordStrategy existingRecordStrategy = ExistingRecordStrategy.None,
            int? recordRetentionCount = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new PutAndReturnInternalRecordIdOp<TObject>(objectToPut, tags, existingRecordStrategy, recordRetentionCount, versionMatchStrategy);
            var protocol = stream.GetStreamWritingProtocols<TObject>();
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Put an object into a stream and return the internal identifier of the record
        /// or null if the specified strategy for dealing with an existing record prevents the object from being written.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="tags">OPTIONAL tags to put with the record.  DEFAULT is no tags.</param>
        /// <param name="existingRecordStrategy">OPTIONAL strategy to use when an existing record is encountered while writing.  DEFAULT is to put a new record regardless of any existing records.</param>
        /// <param name="recordRetentionCount">OPTIONAL number of existing records to retain if <paramref name="existingRecordStrategy"/> is set to prune.  DEFAULT is n/a.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type) when looking for existing records.  DEFAULT is no filter (any version is acceptable).</param>
        /// <returns>The internal record identifier or null if the object wasn't written.</returns>
        public static long? PutAndReturnInternalRecordId<TObject>(
            this IStreamWriteProtocols<TObject> protocol,
            TObject objectToPut,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            ExistingRecordStrategy existingRecordStrategy = ExistingRecordStrategy.None,
            int? recordRetentionCount = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new PutAndReturnInternalRecordIdOp<TObject>(objectToPut, tags, existingRecordStrategy, recordRetentionCount, versionMatchStrategy);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Put an object into a stream and return the internal identifier of the record
        /// or null if the specified strategy for dealing with an existing record prevents the object from being written.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="tags">OPTIONAL tags to put with the record.  DEFAULT is no tags.</param>
        /// <param name="existingRecordStrategy">OPTIONAL strategy to use when an existing record is encountered while writing.  DEFAULT is to put a new record regardless of any existing records.</param>
        /// <param name="recordRetentionCount">OPTIONAL number of existing records to retain if <paramref name="existingRecordStrategy"/> is set to prune.  DEFAULT is n/a.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type) when looking for existing records.  DEFAULT is no filter (any version is acceptable).</param>
        /// <returns>The internal record identifier or null if the object wasn't written.</returns>
        public static async Task<long?> PutAndReturnInternalRecordIdAsync<TObject>(
            this IStreamWriteProtocols<TObject> protocol,
            TObject objectToPut,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            ExistingRecordStrategy existingRecordStrategy = ExistingRecordStrategy.None,
            int? recordRetentionCount = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new PutAndReturnInternalRecordIdOp<TObject>(objectToPut, tags, existingRecordStrategy, recordRetentionCount, versionMatchStrategy);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Put an object into a stream and return the internal identifier of the record
        /// or null if the specified strategy for dealing with an existing record prevents the object from being written.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="tags">OPTIONAL tags to put with the record.  DEFAULT is no tags.</param>
        /// <param name="existingRecordStrategy">OPTIONAL strategy to use when an existing record is encountered while writing.  DEFAULT is to put a new record regardless of any existing records.</param>
        /// <param name="recordRetentionCount">OPTIONAL number of existing records to retain if <paramref name="existingRecordStrategy"/> is set to prune.  DEFAULT is n/a.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type) when looking for existing records.  DEFAULT is no filter (any version is acceptable).</param>
        /// <returns>The internal record identifier or null if the object wasn't written.</returns>
        public static long? PutAndReturnInternalRecordId<TObject>(
            this ISyncAndAsyncReturningProtocol<PutAndReturnInternalRecordIdOp<TObject>, long?> protocol,
            TObject objectToPut,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            ExistingRecordStrategy existingRecordStrategy = ExistingRecordStrategy.None,
            int? recordRetentionCount = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new PutAndReturnInternalRecordIdOp<TObject>(objectToPut, tags, existingRecordStrategy, recordRetentionCount, versionMatchStrategy);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Put an object into a stream and return the internal identifier of the record
        /// or null if the specified strategy for dealing with an existing record prevents the object from being written.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="tags">OPTIONAL tags to put with the record.  DEFAULT is no tags.</param>
        /// <param name="existingRecordStrategy">OPTIONAL strategy to use when an existing record is encountered while writing.  DEFAULT is to put a new record regardless of any existing records.</param>
        /// <param name="recordRetentionCount">OPTIONAL number of existing records to retain if <paramref name="existingRecordStrategy"/> is set to prune.  DEFAULT is n/a.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type) when looking for existing records.  DEFAULT is no filter (any version is acceptable).</param>
        /// <returns>The internal record identifier or null if the object wasn't written.</returns>
        public static async Task<long?> PutAndReturnInternalRecordIdAsync<TObject>(
            this ISyncAndAsyncReturningProtocol<PutAndReturnInternalRecordIdOp<TObject>, long?> protocol,
            TObject objectToPut,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            ExistingRecordStrategy existingRecordStrategy = ExistingRecordStrategy.None,
            int? recordRetentionCount = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new PutAndReturnInternalRecordIdOp<TObject>(objectToPut, tags, existingRecordStrategy, recordRetentionCount, versionMatchStrategy);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Put an object into a stream and return the internal identifier of the record
        /// or null if the specified strategy for dealing with an existing record prevents the object from being written.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="tags">OPTIONAL tags to put with the record.  DEFAULT is no tags.</param>
        /// <param name="existingRecordStrategy">OPTIONAL strategy to use when an existing record is encountered while writing.  DEFAULT is to put a new record regardless of any existing records.</param>
        /// <param name="recordRetentionCount">OPTIONAL number of existing records to retain if <paramref name="existingRecordStrategy"/> is set to prune.  DEFAULT is n/a.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type) when looking for existing records.  DEFAULT is no filter (any version is acceptable).</param>
        /// <returns>The internal record identifier or null if the object wasn't written.</returns>
        public static long? PutAndReturnInternalRecordId<TObject>(
            this IPutAndReturnInternalRecordId<TObject> protocol,
            TObject objectToPut,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            ExistingRecordStrategy existingRecordStrategy = ExistingRecordStrategy.None,
            int? recordRetentionCount = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new PutAndReturnInternalRecordIdOp<TObject>(objectToPut, tags, existingRecordStrategy, recordRetentionCount, versionMatchStrategy);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Put an object into a stream and return the internal identifier of the record
        /// or null if the specified strategy for dealing with an existing record prevents the object from being written.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="objectToPut">The object to put.</param>
        /// <param name="tags">OPTIONAL tags to put with the record.  DEFAULT is no tags.</param>
        /// <param name="existingRecordStrategy">OPTIONAL strategy to use when an existing record is encountered while writing.  DEFAULT is to put a new record regardless of any existing records.</param>
        /// <param name="recordRetentionCount">OPTIONAL number of existing records to retain if <paramref name="existingRecordStrategy"/> is set to prune.  DEFAULT is n/a.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type) when looking for existing records.  DEFAULT is no filter (any version is acceptable).</param>
        /// <returns>The internal record identifier or null if the object wasn't written.</returns>
        public static async Task<long?> PutAndReturnInternalRecordIdAsync<TObject>(
            this IPutAndReturnInternalRecordId<TObject> protocol,
            TObject objectToPut,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            ExistingRecordStrategy existingRecordStrategy = ExistingRecordStrategy.None,
            int? recordRetentionCount = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new PutAndReturnInternalRecordIdOp<TObject>(objectToPut, tags, existingRecordStrategy, recordRetentionCount, versionMatchStrategy);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }
    }
}
