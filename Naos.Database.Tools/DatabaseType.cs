// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseType.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools
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
