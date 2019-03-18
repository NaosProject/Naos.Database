// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Credentials.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer.Client
{
    using System.Security;

    /// <summary>
    ///     A set of credentials.
    /// </summary>
    public sealed class Credentials
    {
        /// <summary>
        ///     Gets or sets the password.
        /// </summary>
        /// <value>
        ///     The password.
        /// </value>
        public SecureString Password { get; set; }

        /// <summary>
        ///     Gets or sets the user.
        /// </summary>
        /// <value>
        ///     The user.
        /// </value>
        public string User { get; set; }
    }
}
