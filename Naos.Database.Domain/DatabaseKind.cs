// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseKind.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Kinds of databases.
    /// </summary>
    public enum DatabaseKind
    {
        /// <summary>
        /// Invalid default.
        /// </summary>
        Invalid,

        /// <summary>
        /// Microsoft SQL Server.
        /// </summary>
        SqlServer,

        /// <summary>
        /// Mongo DB.
        /// </summary>
        Mongo,
    }
}
