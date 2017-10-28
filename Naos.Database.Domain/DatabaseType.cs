// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseType.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
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
