// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestoreDatabaseMessage.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Scheduler
{
    using System;

    using Naos.Database.Domain;
    using Naos.FileJanitor.MessageBus.Scheduler;
    using Naos.MessageBus.Domain;

    /// <summary>
    /// Message to initiate a database restore on the server the handler is on.
    /// </summary>
    public class RestoreDatabaseMessage : IMessage, IShareFilePath, IShareDatabaseName
    {
        /// <inheritdoc />
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the database to restore to.
        /// </summary>
        public string DatabaseName { get; set; }

        /// <inheritdoc />
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the checksum option to use.
        /// </summary>
        public ChecksumOption ChecksumOption { get; set; }

        /// <summary>
        /// Gets or sets the error handling to use.
        /// </summary>
        public ErrorHandling ErrorHandling { get; set; }

        /// <summary>
        /// Gets or sets the recovery option to use.
        /// </summary>
        public RecoveryOption RecoveryOption { get; set; }

        /// <summary>
        /// Gets or sets the replace to use.
        /// </summary>
        public ReplaceOption ReplaceOption { get; set; }

        /// <summary>
        /// Gets or sets the restricted user option to use.
        /// </summary>
        public RestrictedUserOption RestrictedUserOption { get; set; }

        /// <summary>
        /// Gets or sets an optional timeout; if not specified then the <see cref="DatabaseMessageHandlerSettings.DefaultTimeout" /> will be used.
        /// </summary>
        public TimeSpan Timeout { get; set; }
    }
}
