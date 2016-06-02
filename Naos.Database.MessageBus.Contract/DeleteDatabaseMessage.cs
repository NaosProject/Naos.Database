// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteDatabaseMessage.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Contract
{
    using Naos.MessageBus.Domain;

    /// <summary>
    /// Message to delete a database on the server the handler is on.
    /// </summary>
    public class DeleteDatabaseMessage : IMessage, IShareDatabaseName
    {
        /// <inheritdoc />
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the database to delete.
        /// </summary>
        public string DatabaseName { get; set; }
    }
}
