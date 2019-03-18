// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAllQuery.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using Dapper;
    using Newtonsoft.Json;
    using Spritely.Cqrs;

    /// <summary>
    ///     Query getting all results from a particular model type (aka. table name).
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class GetAllQuery<TResult> : IQuery<IReadOnlyCollection<TResult>>
    {
        /// <summary>
        ///     Gets or sets the type of the model (the table name).
        /// </summary>
        /// <value>
        ///     The type of the model.
        /// </value>
        public string ModelType { get; set; }
    }

    /// <summary>
    ///     Query handler for GetAllQuery{TResult}.
    /// </summary>
    /// <typeparam name="TDatabase">The type of the database.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public sealed class GetAllQueryHandler<TDatabase, TResult> : IQueryHandler<GetAllQuery<TResult>, IReadOnlyCollection<TResult>>
        where TDatabase : ReadModelDatabase<TDatabase>
    {
        private readonly JsonSerializerSettings jsonSerializerSettings;
        private readonly TDatabase readModelDatabase;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GetAllQueryHandler" /> class.
        /// </summary>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <param name="jsonSerializerSettings">The json serializer settings.</param>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
            Justification = "ArgumentExceptions do use argument names, but provide additional information.")]
        public GetAllQueryHandler(TDatabase readModelDatabase, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException("readModelDatabase");
            }

            if (readModelDatabase.ConnectionSettings == null)
            {
                throw new ArgumentNullException("readModelDatabase.ConnectionSettings");
            }

            this.readModelDatabase = readModelDatabase;
            this.jsonSerializerSettings = jsonSerializerSettings ?? Json.SerializerSettings;
        }

        /// <summary>
        ///     Handles the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The query result.</returns>
        public IReadOnlyCollection<TResult> Handle(GetAllQuery<TResult> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            IReadOnlyCollection<TResult> result;

            using (var connection = this.readModelDatabase.CreateConnection())
            {
                result = connection
                    .Query(string.Format(CultureInfo.InvariantCulture, "select * from [{0}]", query.ModelType))
                    .Select<dynamic, TResult>(
                        t =>
                            JsonConvert.DeserializeObject<TResult>(
                                this.readModelDatabase.ReadModel(t, query.ModelType),
                                this.jsonSerializerSettings))
                    .ToList()
                    .AsReadOnly();
            }

            return result;
        }
    }
}
