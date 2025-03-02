// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyStreamExtensions.DoesAnyExistById{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    public static partial class ReadOnlyStreamExtensions
    {
        /// <summary>
        /// Gets a value indicating whether or not any record by the provided identifier exists.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="tagsToMatch">OPTIONAL tags to match.  DEFAULT is no matching on tags.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        /// <param name="recordsToFilterSelectionStrategy">OPTIONAL strategy for selecting records before filtering.  DEFAULT is to select all records.</param>
        /// <returns>true if any record exists, otherwise false.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = NaosSuppressBecause.CA1004_GenericMethodsShouldProvideTypeParameter_OnlyInputsToMethodAreTypesAndItsMoreConciseToCallMethodUseGenericTypeParameters)]
        public static bool DoesAnyExistById<TId, TObject>(
            this IReadOnlyStream stream,
            TId id,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType,
            RecordsToFilterSelectionStrategy recordsToFilterSelectionStrategy = RecordsToFilterSelectionStrategy.All)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new DoesAnyExistByIdOp<TId, TObject>(id, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes, typeSelectionStrategy, recordsToFilterSelectionStrategy);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId, TObject>();
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether or not any record by the provided identifier exists.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="tagsToMatch">OPTIONAL tags to match.  DEFAULT is no matching on tags.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        /// <param name="recordsToFilterSelectionStrategy">OPTIONAL strategy for selecting records before filtering.  DEFAULT is to select all records.</param>
        /// <returns>true if any record exists, otherwise false.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = NaosSuppressBecause.CA1004_GenericMethodsShouldProvideTypeParameter_OnlyInputsToMethodAreTypesAndItsMoreConciseToCallMethodUseGenericTypeParameters)]
        public static async Task<bool> DoesAnyExistByIdAsync<TId, TObject>(
            this IReadOnlyStream stream,
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType,
            RecordsToFilterSelectionStrategy recordsToFilterSelectionStrategy = RecordsToFilterSelectionStrategy.All)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new DoesAnyExistByIdOp<TId, TObject>(id, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes, typeSelectionStrategy, recordsToFilterSelectionStrategy);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId, TObject>();
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether or not any record by the provided identifier exists.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="tagsToMatch">OPTIONAL tags to match.  DEFAULT is no matching on tags.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        /// <param name="recordsToFilterSelectionStrategy">OPTIONAL strategy for selecting records before filtering.  DEFAULT is to select all records.</param>
        /// <returns>true if any record exists, otherwise false.</returns>
        public static bool DoesAnyExistById<TId, TObject>(
            this IStreamReadWithIdProtocols<TId, TObject> protocol,
            TId id,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType,
            RecordsToFilterSelectionStrategy recordsToFilterSelectionStrategy = RecordsToFilterSelectionStrategy.All)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new DoesAnyExistByIdOp<TId, TObject>(id, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes, typeSelectionStrategy, recordsToFilterSelectionStrategy);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether or not any record by the provided identifier exists.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="tagsToMatch">OPTIONAL tags to match.  DEFAULT is no matching on tags.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        /// <param name="recordsToFilterSelectionStrategy">OPTIONAL strategy for selecting records before filtering.  DEFAULT is to select all records.</param>
        /// <returns>true if any record exists, otherwise false.</returns>
        public static async Task<bool> DoesAnyExistByIdAsync<TId, TObject>(
            this IStreamReadWithIdProtocols<TId, TObject> protocol,
            TId id,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType,
            RecordsToFilterSelectionStrategy recordsToFilterSelectionStrategy = RecordsToFilterSelectionStrategy.All)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new DoesAnyExistByIdOp<TId, TObject>(id, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes, typeSelectionStrategy, recordsToFilterSelectionStrategy);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether or not any record by the provided identifier exists.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="tagsToMatch">OPTIONAL tags to match.  DEFAULT is no matching on tags.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        /// <param name="recordsToFilterSelectionStrategy">OPTIONAL strategy for selecting records before filtering.  DEFAULT is to select all records.</param>
        /// <returns>true if any record exists, otherwise false.</returns>
        public static bool DoesAnyExistById<TId, TObject>(
            this ISyncAndAsyncReturningProtocol<DoesAnyExistByIdOp<TId, TObject>, bool> protocol,
            TId id,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType,
            RecordsToFilterSelectionStrategy recordsToFilterSelectionStrategy = RecordsToFilterSelectionStrategy.All)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new DoesAnyExistByIdOp<TId, TObject>(id, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes, typeSelectionStrategy, recordsToFilterSelectionStrategy);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether or not any record by the provided identifier exists.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="tagsToMatch">OPTIONAL tags to match.  DEFAULT is no matching on tags.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        /// <param name="recordsToFilterSelectionStrategy">OPTIONAL strategy for selecting records before filtering.  DEFAULT is to select all records.</param>
        /// <returns>true if any record exists, otherwise false.</returns>
        public static async Task<bool> DoesAnyExistByIdAsync<TId, TObject>(
            this ISyncAndAsyncReturningProtocol<DoesAnyExistByIdOp<TId, TObject>, bool> protocol,
            TId id,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType,
            RecordsToFilterSelectionStrategy recordsToFilterSelectionStrategy = RecordsToFilterSelectionStrategy.All)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new DoesAnyExistByIdOp<TId, TObject>(id, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes, typeSelectionStrategy, recordsToFilterSelectionStrategy);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether or not any record by the provided identifier exists.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="tagsToMatch">OPTIONAL tags to match.  DEFAULT is no matching on tags.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        /// <param name="recordsToFilterSelectionStrategy">OPTIONAL strategy for selecting records before filtering.  DEFAULT is to select all records.</param>
        /// <returns>true if any record exists, otherwise false.</returns>
        public static bool DoesAnyExistById<TId, TObject>(
            this IDoesAnyExistById<TId, TObject> protocol,
            TId id,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType,
            RecordsToFilterSelectionStrategy recordsToFilterSelectionStrategy = RecordsToFilterSelectionStrategy.All)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new DoesAnyExistByIdOp<TId, TObject>(id, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes, typeSelectionStrategy, recordsToFilterSelectionStrategy);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether or not any record by the provided identifier exists.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="tagsToMatch">OPTIONAL tags to match.  DEFAULT is no matching on tags.</param>
        /// <param name="tagMatchStrategy">OPTIONAL strategy to use for comparing tags.  DEFAULT is to match when a record contains all of the queried tags (with extra tags on the record ignored), when <paramref name="tagsToMatch"/> is specified.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        /// <param name="recordsToFilterSelectionStrategy">OPTIONAL strategy for selecting records before filtering.  DEFAULT is to select all records.</param>
        /// <returns>true if any record exists, otherwise false.</returns>
        public static async Task<bool> DoesAnyExistByIdAsync<TId, TObject>(
            this IDoesAnyExistById<TId, TObject> protocol,
            TId id,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType,
            RecordsToFilterSelectionStrategy recordsToFilterSelectionStrategy = RecordsToFilterSelectionStrategy.All)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new DoesAnyExistByIdOp<TId, TObject>(id, versionMatchStrategy, tagsToMatch, tagMatchStrategy, deprecatedIdTypes, typeSelectionStrategy, recordsToFilterSelectionStrategy);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }
    }
}
