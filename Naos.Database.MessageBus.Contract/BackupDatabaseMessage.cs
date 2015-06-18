// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackupDatabaseMessage.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Contract
{
    using Naos.MessageBus.DataContract;

    /// <summary>
    /// Message to initiate a database backup on the server the handler is on.
    /// </summary>
    public class BackupDatabaseMessage : IMessage
    {
        /// <inheritdoc />
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the database to backup.
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the name to use on a backup.
        /// </summary>
        public string BackupName { get; set; }
    }
}
