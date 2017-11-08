// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseKind.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
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
