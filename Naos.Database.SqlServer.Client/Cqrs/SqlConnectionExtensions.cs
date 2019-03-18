// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlConnectionExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer.Client
{
    using System;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using Spritely.Recipes;

    /// <summary>
    /// Extensions to <see cref="DatabaseConnectionSettings" />.
    /// </summary>
    public static class SqlConnectionExtensions
    {
        /// <summary>
        /// Make a standard SQL connection string with password as clear text.
        /// </summary>
        /// <param name="settings">Input settings.</param>
        /// <returns>Standard connection string with password as clear text.</returns>
        public static string ToInsecureConnectionString(this DatabaseConnectionSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var connectionString = settings.CreateCredentialLessConnectionString();

            if (settings.Credentials != null)
            {
                if (!string.IsNullOrWhiteSpace(settings.MoreOptions))
                {
                    connectionString.Append(settings.MoreOptions);
                }

                connectionString.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "User Id={0};",
                    settings.Credentials.User);

                connectionString.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "Password={0};",
                    settings.Credentials.Password?.ToInsecureString() ?? string.Empty);

                return connectionString.ToString();
            }

            connectionString.Append("Integrated Security=True;");
            if (!string.IsNullOrWhiteSpace(settings.MoreOptions))
            {
                connectionString.Append(settings.MoreOptions);
            }

            return connectionString.ToString();
        }

        /// <summary>
        ///     Creates a SQLConnection instance from database connection settings. Caller should threat
        ///     this method like a call to new SqlConnection() by disposing of the instance appropriately.
        /// </summary>
        /// <param name="settings">The database connection settings.</param>
        /// <returns>A SQLConnection instance configured with the given database connection settings.</returns>
        public static SqlConnection CreateSqlConnection(this DatabaseConnectionSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var connectionString = settings.CreateCredentialLessConnectionString();

            if (settings.Credentials != null)
            {
                if (!string.IsNullOrWhiteSpace(settings.MoreOptions))
                {
                    connectionString.Append(settings.MoreOptions);
                }

                var credentials = new SqlCredential(settings.Credentials.User, settings.Credentials.Password);

                return new SqlConnection(connectionString.ToString(), credentials);
            }

            connectionString.Append("Integrated Security=True;");
            if (!string.IsNullOrWhiteSpace(settings.MoreOptions))
            {
                connectionString.Append(settings.MoreOptions);
            }

            return new SqlConnection(connectionString.ToString());
        }

        private static StringBuilder CreateCredentialLessConnectionString(this DatabaseConnectionSettings settings)
        {
            var connectionString = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(settings.Server))
            {
                connectionString.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "Server={0};",
                    settings.Server);
            }

            if (!string.IsNullOrWhiteSpace(settings.Database))
            {
                connectionString.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "Database={0};",
                    settings.Database);
            }

            connectionString.AppendFormat(
                CultureInfo.InvariantCulture,
                "Async={0};",
                settings.Async);

            connectionString.AppendFormat(
                CultureInfo.InvariantCulture,
                "Encrypt={0};",
                settings.Encrypt);

            if (settings.ConnectionTimeoutInSeconds.HasValue)
            {
                connectionString.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "Connection Timeout={0};",
                    settings.ConnectionTimeoutInSeconds.Value);
            }

            return connectionString;
        }
    }
}
