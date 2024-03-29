﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyStreamExtensions.GetLatestObjectByTagsOp{TObject}.cs" company="Naos Project">
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
        /// Gets the most recent object with the specified tag.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="tagsToMatch">The tags to match.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <returns>The object.</returns>
        public static TObject GetLatestObjectByTags<TObject>(
            this IReadOnlyStream stream,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetLatestObjectByTagsOp<TObject>(tagsToMatch, tagMatchStrategy, versionMatchStrategy, recordNotFoundStrategy);
            var protocol = stream.GetStreamReadingProtocols<TObject>();
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets the most recent object with the specified tag.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="tagsToMatch">The tags to match.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <returns>The object.</returns>
        public static async Task<TObject> GetLatestObjectByTagsAsync<TObject>(
            this IReadOnlyStream stream,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetLatestObjectByTagsOp<TObject>(tagsToMatch, tagMatchStrategy, versionMatchStrategy, recordNotFoundStrategy);
            var protocol = stream.GetStreamReadingProtocols<TObject>();
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Gets the most recent object with the specified tag.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="tagsToMatch">The tags to match.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <returns>The object.</returns>
        public static TObject GetLatestObjectByTags<TObject>(
            this IStreamReadProtocols<TObject> protocol,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetLatestObjectByTagsOp<TObject>(tagsToMatch, tagMatchStrategy, versionMatchStrategy, recordNotFoundStrategy);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets the most recent object with the specified tag.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="tagsToMatch">The tags to match.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <returns>The object.</returns>
        public static async Task<TObject> GetLatestObjectByTagsAsync<TObject>(
            this IStreamReadProtocols<TObject> protocol,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetLatestObjectByTagsOp<TObject>(tagsToMatch, tagMatchStrategy, versionMatchStrategy, recordNotFoundStrategy);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Gets the most recent object with the specified tag.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="tagsToMatch">The tags to match.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <returns>The object.</returns>
        public static TObject GetLatestObjectByTags<TObject>(
            this ISyncAndAsyncReturningProtocol<GetLatestObjectByTagsOp<TObject>, TObject> protocol,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetLatestObjectByTagsOp<TObject>(tagsToMatch, tagMatchStrategy, versionMatchStrategy, recordNotFoundStrategy);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets the most recent object with the specified tag.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="tagsToMatch">The tags to match.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <returns>The object.</returns>
        public static async Task<TObject> GetLatestObjectByTagsAsync<TObject>(
            this ISyncAndAsyncReturningProtocol<GetLatestObjectByTagsOp<TObject>, TObject> protocol,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetLatestObjectByTagsOp<TObject>(tagsToMatch, tagMatchStrategy, versionMatchStrategy, recordNotFoundStrategy);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Gets the most recent object with the specified tag.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="tagsToMatch">The tags to match.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <returns>The object.</returns>
        public static TObject GetLatestObjectByTags<TObject>(
            this IGetLatestObjectByTags<TObject> protocol,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetLatestObjectByTagsOp<TObject>(tagsToMatch, tagMatchStrategy, versionMatchStrategy, recordNotFoundStrategy);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets the most recent object with the specified tag.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="tagsToMatch">The tags to match.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <returns>The object.</returns>
        public static async Task<TObject> GetLatestObjectByTagsAsync<TObject>(
            this IGetLatestObjectByTags<TObject> protocol,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetLatestObjectByTagsOp<TObject>(tagsToMatch, tagMatchStrategy, versionMatchStrategy, recordNotFoundStrategy);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }
    }
}
