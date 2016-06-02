// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IShareDatabaseName.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Contract
{
    using Naos.MessageBus.Domain;

    /// <summary>
    /// Interface to share database name with other messages in a sequence.
    /// </summary>
    public interface IShareDatabaseName : IShare
    {
        /// <summary>
        /// Gets or sets the name of the database to delete.
        /// </summary>
        string DatabaseName { get; set; }
    }
}
