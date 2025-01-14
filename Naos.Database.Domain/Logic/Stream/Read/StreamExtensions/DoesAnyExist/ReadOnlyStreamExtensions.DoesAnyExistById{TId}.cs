// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyStreamExtensions.DoesAnyExistById{TId}.cs" company="Naos Project">
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
        /// Gets a value indicating whether or not any record by the provided identifier exists.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        /// <returns>true if any record exists, otherwise false.</returns>
        public static bool DoesAnyExistById<TId>(
            this IReadOnlyStream stream,
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new DoesAnyExistByIdOp<TId>(id, objectType, versionMatchStrategy, deprecatedIdTypes, typeSelectionStrategy);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId>();
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether or not any record by the provided identifier exists.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        /// <returns>true if any record exists, otherwise false.</returns>
        public static async Task<bool> DoesAnyExistByIdAsync<TId>(
            this IReadOnlyStream stream,
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new DoesAnyExistByIdOp<TId>(id, objectType, versionMatchStrategy, deprecatedIdTypes, typeSelectionStrategy);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId>();
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether or not any record by the provided identifier exists.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        /// <returns>true if any record exists, otherwise false.</returns>
        public static bool DoesAnyExistById<TId>(
            this IStreamReadWithIdProtocols<TId> protocol,
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new DoesAnyExistByIdOp<TId>(id, objectType, versionMatchStrategy, deprecatedIdTypes, typeSelectionStrategy);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether or not any record by the provided identifier exists.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        /// <returns>true if any record exists, otherwise false.</returns>
        public static async Task<bool> DoesAnyExistByIdAsync<TId>(
            this IStreamReadWithIdProtocols<TId> protocol,
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new DoesAnyExistByIdOp<TId>(id, objectType, versionMatchStrategy, deprecatedIdTypes, typeSelectionStrategy);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether or not any record by the provided identifier exists.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        /// <returns>true if any record exists, otherwise false.</returns>
        public static bool DoesAnyExistById<TId>(
            this ISyncAndAsyncReturningProtocol<DoesAnyExistByIdOp<TId>, bool> protocol,
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new DoesAnyExistByIdOp<TId>(id, objectType, versionMatchStrategy, deprecatedIdTypes, typeSelectionStrategy);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether or not any record by the provided identifier exists.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        /// <returns>true if any record exists, otherwise false.</returns>
        public static async Task<bool> DoesAnyExistByIdAsync<TId>(
            this ISyncAndAsyncReturningProtocol<DoesAnyExistByIdOp<TId>, bool> protocol,
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new DoesAnyExistByIdOp<TId>(id, objectType, versionMatchStrategy, deprecatedIdTypes, typeSelectionStrategy);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether or not any record by the provided identifier exists.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        /// <returns>true if any record exists, otherwise false.</returns>
        public static bool DoesAnyExistById<TId>(
            this IDoesAnyExistById<TId> protocol,
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new DoesAnyExistByIdOp<TId>(id, objectType, versionMatchStrategy, deprecatedIdTypes, typeSelectionStrategy);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether or not any record by the provided identifier exists.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        /// <returns>true if any record exists, otherwise false.</returns>
        public static async Task<bool> DoesAnyExistByIdAsync<TId>(
            this IDoesAnyExistById<TId> protocol,
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new DoesAnyExistByIdOp<TId>(id, objectType, versionMatchStrategy, deprecatedIdTypes, typeSelectionStrategy);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }
    }
}
