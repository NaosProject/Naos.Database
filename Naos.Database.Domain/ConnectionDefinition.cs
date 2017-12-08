// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionDefinition.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Linq;

    using Spritely.Recipes;

    /// <summary>
    /// Kinds of databases.
    /// </summary>
    public class ConnectionDefinition
    {
        /// <summary>
        /// Gets or sets the server name, IP, DNS, etc.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the instance name (if applicable).
        /// </summary>
        public string InstanceName { get; set; }

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
        public string AuthenticationSource { get; set; }
    }
}
