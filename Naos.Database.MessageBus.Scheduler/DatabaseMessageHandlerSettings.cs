﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseMessageHandlerSettings.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Scheduler
{
    using System;
    using System.Collections.Generic;

    using Naos.Database.Domain;

    /// <summary>
    /// Model object for Its.Configuration providing settings for the MessageHandlers.
    /// </summary>
    public class DatabaseMessageHandlerSettings
    {
        /// <summary>
        /// Gets or sets the default timeout for use on database operations.
        /// </summary>
        public TimeSpan DefaultTimeout { get; set; }

        /// <summary>
        /// Gets or sets a map of the <see cref="DatabaseKind" /> to a connection string to use for local host database operations.
        /// </summary>
        public IReadOnlyDictionary<DatabaseKind, string> DatabaseKindToLocalhostConnectionStringMap { get; set; }

        /// <summary>
        /// Gets or sets the location on disk for backups.
        /// </summary>
        public IReadOnlyDictionary<DatabaseKind, string> DatabaseKindToDataDirectoryMap { get; set; }

        /// <summary>
        /// Gets or sets the location on disk for backups.
        /// </summary>
        public IReadOnlyDictionary<DatabaseKind, string> DatabaseKindToBackupDirectoryMap { get; set; }
    }
}
