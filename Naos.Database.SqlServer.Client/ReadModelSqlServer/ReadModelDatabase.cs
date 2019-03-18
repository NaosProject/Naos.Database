// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadModelDatabase.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql
{
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Spritely.Cqrs;

    /// <summary>
    ///     Class representing a read model database. Commands and queries accessing the read model require this.
    /// </summary>
    public class ReadModelDatabase<T> : IDatabase where T : ReadModelDatabase<T>
    {
        /// <summary>
        ///     Gets or sets the connection settings.
        /// </summary>
        /// <value>
        ///     The connection settings.
        /// </value>
        public DatabaseConnectionSettings ConnectionSettings { get; set; }

        /// <summary>
        ///     Creates a database connection (user is expected to properly dispose of instance).
        /// </summary>
        /// <returns>A new database connection (user is expected to properly dispose of instance)</returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
            Justification = "This method does not dispose by design. User is expected to dispose of method result.")]
        public virtual IDbConnection CreateConnection()
        {
            var connection = this.ConnectionSettings.CreateSqlConnection();

            connection.Open();

            return connection;
        }

        /// <summary>
        ///     Reads the model property from the instance for the given model type.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>The model.</returns>
        public virtual string ReadModel(dynamic instance, string modelType)
        {
            return instance.Model;
        }

        /// <summary>
        ///     Reads the identifier from the given model instance.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>The model identifier.</returns>
        public virtual string ReadId<TModel>(TModel instance)
        {
            return (instance as dynamic).Id.ToString();
        }

        /// <summary>
        ///     Gets the name of the identifier column for the given model type.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>The name of the identifier column.</returns>
        public virtual string GetIdColumnName(string modelType)
        {
            return "Id";
        }

        /// <summary>
        ///     Gets the name of the model column for the given model type.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>The name of the model column.</returns>
        public virtual string GetModelColumnName(string modelType)
        {
            return "Model";
        }

        /// <summary>
        ///     Gets the name of the updated column for the given model type.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>The name of the updated column.</returns>
        public virtual string GetUpdatedColumnName(string modelType)
        {
            return "UpdatedUtc";
        }
    }
}
