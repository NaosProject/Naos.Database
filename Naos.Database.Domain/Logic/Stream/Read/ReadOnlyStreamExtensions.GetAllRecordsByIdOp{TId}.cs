﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyStreamExtensions.GetAllRecordsByIdOp{TId}.cs" company="Naos Project">
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
        /// Gets all records with the specified identifier.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <param name="orderRecordsBy">OPTIONAL value that specifies how to order the resulting records.  DEFAULT is ascending by internal record identifier.</param>
        /// <returns>The matching stream record(s).</returns>
        public static IReadOnlyList<StreamRecordWithId<TId>> GetAllRecordsById<TId>(
            this IReadOnlyStream stream,
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault,
            OrderRecordsBy orderRecordsBy = OrderRecordsBy.InternalRecordIdAscending)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetAllRecordsByIdOp<TId>(id, objectType, versionMatchStrategy, recordNotFoundStrategy, orderRecordsBy);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId>();
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets all records with the specified identifier.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <param name="orderRecordsBy">OPTIONAL value that specifies how to order the resulting records.  DEFAULT is ascending by internal record identifier.</param>
        /// <returns>The matching stream record(s).</returns>
        public static async Task<IReadOnlyList<StreamRecordWithId<TId>>> GetAllRecordsByIdAsync<TId>(
            this IReadOnlyStream stream,
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault,
            OrderRecordsBy orderRecordsBy = OrderRecordsBy.InternalRecordIdAscending)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetAllRecordsByIdOp<TId>(id, objectType, versionMatchStrategy, recordNotFoundStrategy, orderRecordsBy);
            var protocol = stream.GetStreamReadingWithIdProtocols<TId>();
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Gets all records with the specified identifier.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <param name="orderRecordsBy">OPTIONAL value that specifies how to order the resulting records.  DEFAULT is ascending by internal record identifier.</param>
        /// <returns>The matching stream record(s).</returns>
        public static IReadOnlyList<StreamRecordWithId<TId>> GetAllRecordsById<TId>(
            this IStreamReadWithIdProtocols<TId> protocol,
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault,
            OrderRecordsBy orderRecordsBy = OrderRecordsBy.InternalRecordIdAscending)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetAllRecordsByIdOp<TId>(id, objectType, versionMatchStrategy, recordNotFoundStrategy, orderRecordsBy);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets all records with the specified identifier.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <param name="orderRecordsBy">OPTIONAL value that specifies how to order the resulting records.  DEFAULT is ascending by internal record identifier.</param>
        /// <returns>The matching stream record(s).</returns>
        public static async Task<IReadOnlyList<StreamRecordWithId<TId>>> GetAllRecordsByIdAsync<TId>(
            this IStreamReadWithIdProtocols<TId> protocol,
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault,
            OrderRecordsBy orderRecordsBy = OrderRecordsBy.InternalRecordIdAscending)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetAllRecordsByIdOp<TId>(id, objectType, versionMatchStrategy, recordNotFoundStrategy, orderRecordsBy);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Gets all records with the specified identifier.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <param name="orderRecordsBy">OPTIONAL value that specifies how to order the resulting records.  DEFAULT is ascending by internal record identifier.</param>
        /// <returns>The matching stream record(s).</returns>
        public static IReadOnlyList<StreamRecordWithId<TId>> GetAllRecordsById<TId>(
            this ISyncAndAsyncReturningProtocol<GetAllRecordsByIdOp<TId>, IReadOnlyList<StreamRecordWithId<TId>>> protocol,
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault,
            OrderRecordsBy orderRecordsBy = OrderRecordsBy.InternalRecordIdAscending)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetAllRecordsByIdOp<TId>(id, objectType, versionMatchStrategy, recordNotFoundStrategy, orderRecordsBy);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets all records with the specified identifier.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <param name="orderRecordsBy">OPTIONAL value that specifies how to order the resulting records.  DEFAULT is ascending by internal record identifier.</param>
        /// <returns>The matching stream record(s).</returns>
        public static async Task<IReadOnlyList<StreamRecordWithId<TId>>> GetAllRecordsByIdAsync<TId>(
            this ISyncAndAsyncReturningProtocol<GetAllRecordsByIdOp<TId>, IReadOnlyList<StreamRecordWithId<TId>>> protocol,
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault,
            OrderRecordsBy orderRecordsBy = OrderRecordsBy.InternalRecordIdAscending)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetAllRecordsByIdOp<TId>(id, objectType, versionMatchStrategy, recordNotFoundStrategy, orderRecordsBy);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }

        /// <summary>
        /// Gets all records with the specified identifier.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <param name="orderRecordsBy">OPTIONAL value that specifies how to order the resulting records.  DEFAULT is ascending by internal record identifier.</param>
        /// <returns>The matching stream record(s).</returns>
        public static IReadOnlyList<StreamRecordWithId<TId>> GetAllRecordsById<TId>(
            this IGetAllRecordsById<TId> protocol,
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault,
            OrderRecordsBy orderRecordsBy = OrderRecordsBy.InternalRecordIdAscending)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetAllRecordsByIdOp<TId>(id, objectType, versionMatchStrategy, recordNotFoundStrategy, orderRecordsBy);
            var result = protocol.Execute(operation);
            return result;
        }

        /// <summary>
        /// Gets all records with the specified identifier.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
        /// <param name="protocol">The protocol.</param>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <param name="orderRecordsBy">OPTIONAL value that specifies how to order the resulting records.  DEFAULT is ascending by internal record identifier.</param>
        /// <returns>The matching stream record(s).</returns>
        public static async Task<IReadOnlyList<StreamRecordWithId<TId>>> GetAllRecordsByIdAsync<TId>(
            this IGetAllRecordsById<TId> protocol,
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault,
            OrderRecordsBy orderRecordsBy = OrderRecordsBy.InternalRecordIdAscending)
        {
            protocol.MustForArg(nameof(protocol)).NotBeNull();

            var operation = new GetAllRecordsByIdOp<TId>(id, objectType, versionMatchStrategy, recordNotFoundStrategy, orderRecordsBy);
            var result = await protocol.ExecuteAsync(operation);
            return result;
        }
    }
}
