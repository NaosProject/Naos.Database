// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyStreamExtensions.GetDistinctIdsOp{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    public static partial class ReadOnlyStreamExtensions
    {
        /// <summary>
        /// Gets the distinct identifiers for the supplied filters.
        /// </summary>
        /// <typeparam name="TId">Type of the identifier.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="objectTypes">OPTIONAL object types to match on or null when not matching on object type.  DEFAULT is not to match on object types.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="tagsToMatch">OPTIONAL tags to match or null when not matching on tags.  DEFAULT is not to match on tags.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="recordsToFilterCriteria">OPTIONAL object that specifies how to determine the records that are input into a <see cref="RecordFilter"/>.  DEFAULT is to use all records in the stream.</param>
        /// <returns>Distinct identifiers per the filters.</returns>
        public static IReadOnlyCollection<TId> GetDistinctIds<TId>(
            this IReadOnlyStream stream,
            IReadOnlyCollection<TypeRepresentation> objectTypes = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            RecordsToFilterCriteria recordsToFilterCriteria = null)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetDistinctIdsOp<TId>(objectTypes, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes, recordsToFilterCriteria);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId>();
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets the distinct identifiers for the supplied filters.
        /// </summary>
        /// <typeparam name="TId">Type of the identifier.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="objectTypes">OPTIONAL object types to match on or null when not matching on object type.  DEFAULT is not to match on object types.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="tagsToMatch">OPTIONAL tags to match or null when not matching on tags.  DEFAULT is not to match on tags.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="recordsToFilterCriteria">OPTIONAL object that specifies how to determine the records that are input into a <see cref="RecordFilter"/>.  DEFAULT is to use all records in the stream.</param>
        /// <returns>Distinct identifiers per the filters.</returns>
        public static async Task<IReadOnlyCollection<TId>> GetDistinctIdsAsync<TId>(
            this IReadOnlyStream stream,
            IReadOnlyCollection<TypeRepresentation> objectTypes = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            RecordsToFilterCriteria recordsToFilterCriteria = null)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetDistinctIdsOp<TId>(objectTypes, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes, recordsToFilterCriteria);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId>();
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Gets the distinct identifiers for the supplied filters.
        /// </summary>
        /// <typeparam name="TId">Type of the identifier.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="objectTypes">OPTIONAL object types to match on or null when not matching on object type.  DEFAULT is not to match on object types.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="tagsToMatch">OPTIONAL tags to match or null when not matching on tags.  DEFAULT is not to match on tags.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="recordsToFilterCriteria">OPTIONAL object that specifies how to determine the records that are input into a <see cref="RecordFilter"/>.  DEFAULT is to use all records in the stream.</param>
        /// <returns>Distinct identifiers per the filters.</returns>
        public static IReadOnlyCollection<TId> GetDistinctIds<TId>(
            this IStreamReadWithIdProtocols<TId> protocol,
            IReadOnlyCollection<TypeRepresentation> objectTypes = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            RecordsToFilterCriteria recordsToFilterCriteria = null)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetDistinctIdsOp<TId>(objectTypes, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes, recordsToFilterCriteria);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets the distinct identifiers for the supplied filters.
        /// </summary>
        /// <typeparam name="TId">Type of the identifier.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="objectTypes">OPTIONAL object types to match on or null when not matching on object type.  DEFAULT is not to match on object types.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="tagsToMatch">OPTIONAL tags to match or null when not matching on tags.  DEFAULT is not to match on tags.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="recordsToFilterCriteria">OPTIONAL object that specifies how to determine the records that are input into a <see cref="RecordFilter"/>.  DEFAULT is to use all records in the stream.</param>
        /// <returns>Distinct identifiers per the filters.</returns>
        public static async Task<IReadOnlyCollection<TId>> GetDistinctIdsAsync<TId>(
            this IStreamReadWithIdProtocols<TId> protocol,
            IReadOnlyCollection<TypeRepresentation> objectTypes = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            RecordsToFilterCriteria recordsToFilterCriteria = null)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetDistinctIdsOp<TId>(objectTypes, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes, recordsToFilterCriteria);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Gets the distinct identifiers for the supplied filters.
        /// </summary>
        /// <typeparam name="TId">Type of the identifier.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="objectTypes">OPTIONAL object types to match on or null when not matching on object type.  DEFAULT is not to match on object types.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="tagsToMatch">OPTIONAL tags to match or null when not matching on tags.  DEFAULT is not to match on tags.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="recordsToFilterCriteria">OPTIONAL object that specifies how to determine the records that are input into a <see cref="RecordFilter"/>.  DEFAULT is to use all records in the stream.</param>
        /// <returns>Distinct identifiers per the filters.</returns>
        public static IReadOnlyCollection<TId> GetDistinctIds<TId>(
            this ISyncAndAsyncReturningProtocol<GetDistinctIdsOp<TId>, IReadOnlyCollection<TId>> protocol,
            IReadOnlyCollection<TypeRepresentation> objectTypes = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            RecordsToFilterCriteria recordsToFilterCriteria = null)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetDistinctIdsOp<TId>(objectTypes, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes, recordsToFilterCriteria);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets the distinct identifiers for the supplied filters.
        /// </summary>
        /// <typeparam name="TId">Type of the identifier.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="objectTypes">OPTIONAL object types to match on or null when not matching on object type.  DEFAULT is not to match on object types.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="tagsToMatch">OPTIONAL tags to match or null when not matching on tags.  DEFAULT is not to match on tags.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="recordsToFilterCriteria">OPTIONAL object that specifies how to determine the records that are input into a <see cref="RecordFilter"/>.  DEFAULT is to use all records in the stream.</param>
        /// <returns>Distinct identifiers per the filters.</returns>
        public static async Task<IReadOnlyCollection<TId>> GetDistinctIdsAsync<TId>(
            this ISyncAndAsyncReturningProtocol<GetDistinctIdsOp<TId>, IReadOnlyCollection<TId>> protocol,
            IReadOnlyCollection<TypeRepresentation> objectTypes = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            RecordsToFilterCriteria recordsToFilterCriteria = null)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetDistinctIdsOp<TId>(objectTypes, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes, recordsToFilterCriteria);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Gets the distinct identifiers for the supplied filters.
        /// </summary>
        /// <typeparam name="TId">Type of the identifier.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="objectTypes">OPTIONAL object types to match on or null when not matching on object type.  DEFAULT is not to match on object types.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="tagsToMatch">OPTIONAL tags to match or null when not matching on tags.  DEFAULT is not to match on tags.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="recordsToFilterCriteria">OPTIONAL object that specifies how to determine the records that are input into a <see cref="RecordFilter"/>.  DEFAULT is to use all records in the stream.</param>
        /// <returns>Distinct identifiers per the filters.</returns>
        public static IReadOnlyCollection<TId> GetDistinctIds<TId>(
            this IGetDistinctIds<TId> protocol,
            IReadOnlyCollection<TypeRepresentation> objectTypes = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            RecordsToFilterCriteria recordsToFilterCriteria = null)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetDistinctIdsOp<TId>(objectTypes, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes, recordsToFilterCriteria);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets the distinct identifiers for the supplied filters.
        /// </summary>
        /// <typeparam name="TId">Type of the identifier.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="objectTypes">OPTIONAL object types to match on or null when not matching on object type.  DEFAULT is not to match on object types.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="tagsToMatch">OPTIONAL tags to match or null when not matching on tags.  DEFAULT is not to match on tags.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="recordsToFilterCriteria">OPTIONAL object that specifies how to determine the records that are input into a <see cref="RecordFilter"/>.  DEFAULT is to use all records in the stream.</param>
        /// <returns>Distinct identifiers per the filters.</returns>
        public static async Task<IReadOnlyCollection<TId>> GetDistinctIdsAsync<TId>(
            this IGetDistinctIds<TId> protocol,
            IReadOnlyCollection<TypeRepresentation> objectTypes = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            RecordsToFilterCriteria recordsToFilterCriteria = null)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetDistinctIdsOp<TId>(objectTypes, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes, recordsToFilterCriteria);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }
    }
}
