// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReplaceOption.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Determines what to do if restoring a database that already exists.
    /// </summary>
    public enum ReplaceOption
    {
        /// <summary>
        /// Existing database is not replaced and the system should throw.
        /// </summary>
        DoNotReplaceExistingDatabaseAndThrow,

        /// <summary>
        /// Replace the existing database.
        /// </summary>
        ReplaceExistingDatabase,
    }
}
