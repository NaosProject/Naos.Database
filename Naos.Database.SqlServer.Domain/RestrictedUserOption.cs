// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestrictedUserOption.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer.Domain
{
    /// <summary>
    /// Determines whether to put the database into restricted user mode after a restore.
    /// </summary>
    public enum RestrictedUserOption
    {
        /// <summary>
        /// Put the database into normal mode.
        /// </summary>
        Normal,

        /// <summary>
        /// Put the database into restricted user mode.
        /// </summary>
        RestrictedUser,
    }
}
