// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyStreamExtensions.GetLatestRecordByIdOp{TId,TObject}.cs" company="Naos Project">
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
        /// Gets the most recent record with the specified identifier.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.</param>
        /// <returns>The record.</returns>
        public static StreamRecordWithId<TId, TObject> GetLatestRecordById<TId, TObject>(
            this IReadOnlyStream stream,
            TId id,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetLatestRecordByIdOp<TId, TObject>(id, versionMatchStrategy, recordNotFoundStrategy, deprecatedIdTypes);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId, TObject>();
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets the most recent record with the specified identifier.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.</param>
        /// <returns>The record.</returns>
        public static async Task<StreamRecordWithId<TId, TObject>> GetLatestRecordByIdAsync<TId, TObject>(
            this IReadOnlyStream stream,
            TId id,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetLatestRecordByIdOp<TId, TObject>(id, versionMatchStrategy, recordNotFoundStrategy, deprecatedIdTypes);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId, TObject>();
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }
    }
}
