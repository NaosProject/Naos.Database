// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDatabaseDefinition.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Interface to declare top level contract of a database definition for use in operations like <see cref="CreateDatabaseOp"/>.
    /// </summary>
    public interface IDatabaseDefinition
    {
        /// <summary>
        /// Gets the name of the database.
        /// </summary>
        string DatabaseName { get; }
    }
}
