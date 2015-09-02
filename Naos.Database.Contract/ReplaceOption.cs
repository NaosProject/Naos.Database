// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReplaceOption.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Contract
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
