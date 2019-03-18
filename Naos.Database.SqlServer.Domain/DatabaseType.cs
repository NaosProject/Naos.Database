// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseType.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer.Domain
{
    /// <summary>
    /// Represents the type of database: system or user.
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// A user database.
        /// </summary>
        User = 0,

        /// <summary>
        /// A system database.
        /// </summary>
        System = 1,
    }
}
