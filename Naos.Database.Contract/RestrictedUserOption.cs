// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestrictedUserOption.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Contract
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
        RestrictedUser
    }
}
