// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseMessageHandlerSettings.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Contract
{
    using System;

    /// <summary>
    /// Model object for Its.Configuration providing settings for the MessageHandlers.
    /// </summary>
    public class DatabaseMessageHandlerSettings
    {
        /// <summary>
        /// Gets or sets the connection string to use for local host database operations.
        /// </summary>
        public string LocalhostConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the default timeout for use on database operations.
        /// </summary>
        public TimeSpan DefaultTimeout { get; set; }

        /// <summary>
        /// Gets or sets the location on disk for backups.
        /// </summary>
        public string DataDirectory { get; set; }

        /// <summary>
        /// Gets or sets the location on disk for backups.
        /// </summary>
        public string BackupDirectory { get; set; }
    }
}
