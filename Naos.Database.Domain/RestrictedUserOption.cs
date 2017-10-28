// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestrictedUserOption.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
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
