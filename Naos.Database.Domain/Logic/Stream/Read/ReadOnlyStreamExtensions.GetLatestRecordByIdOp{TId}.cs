﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyStreamExtensions.GetLatestRecordByIdOp{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    public static partial class ReadOnlyStreamExtensions
    {
        /// <summary>
        /// Gets the most recent record with the specified identifier.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the object type.  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <returns>The record.</returns>
        public static StreamRecordWithId<TId> GetLatestRecordById<TId>(
            this IReadOnlyStream stream,
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetLatestRecordByIdOp<TId>(id, objectType, versionMatchStrategy, recordNotFoundStrategy);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId>();
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets the most recent record with the specified identifier.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the object type.  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <returns>The record.</returns>
        public static async Task<StreamRecordWithId<TId>> GetLatestRecordByIdAsync<TId>(
            this IReadOnlyStream stream,
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetLatestRecordByIdOp<TId>(id, objectType, versionMatchStrategy, recordNotFoundStrategy);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId>();
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }
    }
}