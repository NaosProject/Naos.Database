// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoConnectionDefinition.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Mongo.Domain
{
    using Naos.Database.Domain;

    /// <summary>
    /// Kinds of databases.
    /// </summary>
    public class MongoConnectionDefinition : IConnectionDefinition
    {
        /// <summary>
        /// Gets or sets the server name, IP, DNS, etc.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the authentication source (if applicable).
        /// </summary>
        public string AuthenticationDatabaseName { get; set; }
    }
}