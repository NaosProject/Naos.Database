// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestoreDatabaseMessage.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Contract
{
    using Naos.Database.Contract;
    using Naos.FileJanitor.MessageBus.Contract;
    using Naos.MessageBus.DataContract;

    /// <summary>
    /// Message to initiate a database restore on the server the handler is on.
    /// </summary>
    public class RestoreDatabaseMessage : IMessage, IShareFilePath
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
    }
}
