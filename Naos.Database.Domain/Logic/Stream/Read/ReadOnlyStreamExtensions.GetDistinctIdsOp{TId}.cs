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
        /// <param name="objectTypes">The object types to match on or null when not matching on object type.</param>
        /// <param name="versionMatchStrategy">The strategy to use to filter on the version of the identifier and/or object type.</param>
        /// <param name="tagsToMatch">The tags to match or null when not matching on tags.</param>
        /// <param name="tagMatchStrategy">The strategy to use for comparing tags when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">The object types used in a record that indicates an identifier deprecation.</param>
        /// <returns>Distinct identifiers per the filters.</returns>
        public static IReadOnlyCollection<TId> GetDistinctIds<TId>(
            this IReadOnlyStream stream,
            IReadOnlyCollection<TypeRepresentation> objectTypes = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetDistinctIdsOp<TId>(objectTypes, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId>();
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets the distinct identifiers for the supplied filters.
        /// </summary>
        /// <typeparam name="TId">Type of the identifier.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="objectTypes">The object types to match on or null when not matching on object type.</param>
        /// <param name="versionMatchStrategy">The strategy to use to filter on the version of the identifier and/or object type.</param>
        /// <param name="tagsToMatch">The tags to match or null when not matching on tags.</param>
        /// <param name="tagMatchStrategy">The strategy to use for comparing tags when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">The object types used in a record that indicates an identifier deprecation.</param>
        /// <returns>Distinct identifiers per the filters.</returns>
        public static async Task<IReadOnlyCollection<TId>> GetDistinctIdsAsync<TId>(
            this IReadOnlyStream stream,
            IReadOnlyCollection<TypeRepresentation> objectTypes = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetDistinctIdsOp<TId>(objectTypes, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId>();
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Gets the distinct identifiers for the supplied filters.
        /// </summary>
        /// <typeparam name="TId">Type of the identifier.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="objectTypes">The object types to match on or null when not matching on object type.</param>
        /// <param name="versionMatchStrategy">The strategy to use to filter on the version of the identifier and/or object type.</param>
        /// <param name="tagsToMatch">The tags to match or null when not matching on tags.</param>
        /// <param name="tagMatchStrategy">The strategy to use for comparing tags when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">The object types used in a record that indicates an identifier deprecation.</param>
        /// <returns>Distinct identifiers per the filters.</returns>
        public static IReadOnlyCollection<TId> GetDistinctIds<TId>(
            this IStreamReadWithIdProtocols<TId> protocol,
            IReadOnlyCollection<TypeRepresentation> objectTypes = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetDistinctIdsOp<TId>(objectTypes, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets the distinct identifiers for the supplied filters.
        /// </summary>
        /// <typeparam name="TId">Type of the identifier.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="objectTypes">The object types to match on or null when not matching on object type.</param>
        /// <param name="versionMatchStrategy">The strategy to use to filter on the version of the identifier and/or object type.</param>
        /// <param name="tagsToMatch">The tags to match or null when not matching on tags.</param>
        /// <param name="tagMatchStrategy">The strategy to use for comparing tags when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">The object types used in a record that indicates an identifier deprecation.</param>
        /// <returns>Distinct identifiers per the filters.</returns>
        public static async Task<IReadOnlyCollection<TId>> GetDistinctIdsAsync<TId>(
            this IStreamReadWithIdProtocols<TId> protocol,
            IReadOnlyCollection<TypeRepresentation> objectTypes = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetDistinctIdsOp<TId>(objectTypes, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Gets the distinct identifiers for the supplied filters.
        /// </summary>
        /// <typeparam name="TId">Type of the identifier.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="objectTypes">The object types to match on or null when not matching on object type.</param>
        /// <param name="versionMatchStrategy">The strategy to use to filter on the version of the identifier and/or object type.</param>
        /// <param name="tagsToMatch">The tags to match or null when not matching on tags.</param>
        /// <param name="tagMatchStrategy">The strategy to use for comparing tags when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">The object types used in a record that indicates an identifier deprecation.</param>
        /// <returns>Distinct identifiers per the filters.</returns>
        public static IReadOnlyCollection<TId> GetDistinctIds<TId>(
            this ISyncAndAsyncReturningProtocol<GetDistinctIdsOp<TId>, IReadOnlyCollection<TId>> protocol,
            IReadOnlyCollection<TypeRepresentation> objectTypes = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetDistinctIdsOp<TId>(objectTypes, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets the distinct identifiers for the supplied filters.
        /// </summary>
        /// <typeparam name="TId">Type of the identifier.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="objectTypes">The object types to match on or null when not matching on object type.</param>
        /// <param name="versionMatchStrategy">The strategy to use to filter on the version of the identifier and/or object type.</param>
        /// <param name="tagsToMatch">The tags to match or null when not matching on tags.</param>
        /// <param name="tagMatchStrategy">The strategy to use for comparing tags when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">The object types used in a record that indicates an identifier deprecation.</param>
        /// <returns>Distinct identifiers per the filters.</returns>
        public static async Task<IReadOnlyCollection<TId>> GetDistinctIdsAsync<TId>(
            this ISyncAndAsyncReturningProtocol<GetDistinctIdsOp<TId>, IReadOnlyCollection<TId>> protocol,
            IReadOnlyCollection<TypeRepresentation> objectTypes = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetDistinctIdsOp<TId>(objectTypes, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Gets the distinct identifiers for the supplied filters.
        /// </summary>
        /// <typeparam name="TId">Type of the identifier.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="objectTypes">The object types to match on or null when not matching on object type.</param>
        /// <param name="versionMatchStrategy">The strategy to use to filter on the version of the identifier and/or object type.</param>
        /// <param name="tagsToMatch">The tags to match or null when not matching on tags.</param>
        /// <param name="tagMatchStrategy">The strategy to use for comparing tags when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">The object types used in a record that indicates an identifier deprecation.</param>
        /// <returns>Distinct identifiers per the filters.</returns>
        public static IReadOnlyCollection<TId> GetDistinctIds<TId>(
            this IGetDistinctIds<TId> protocol,
            IReadOnlyCollection<TypeRepresentation> objectTypes = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetDistinctIdsOp<TId>(objectTypes, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets the distinct identifiers for the supplied filters.
        /// </summary>
        /// <typeparam name="TId">Type of the identifier.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="objectTypes">The object types to match on or null when not matching on object type.</param>
        /// <param name="versionMatchStrategy">The strategy to use to filter on the version of the identifier and/or object type.</param>
        /// <param name="tagsToMatch">The tags to match or null when not matching on tags.</param>
        /// <param name="tagMatchStrategy">The strategy to use for comparing tags when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">The object types used in a record that indicates an identifier deprecation.</param>
        /// <returns>Distinct identifiers per the filters.</returns>
        public static async Task<IReadOnlyCollection<TId>> GetDistinctIdsAsync<TId>(
            this IGetDistinctIds<TId> protocol,
            IReadOnlyCollection<TypeRepresentation> objectTypes = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetDistinctIdsOp<TId>(objectTypes, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }
    }
}
