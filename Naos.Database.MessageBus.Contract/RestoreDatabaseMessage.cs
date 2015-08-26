// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestoreDatabaseMessage.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Contract
{
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

        /// <summary>
        /// Gets or sets a value indicating whether or not to run a checksum on the restore.
        /// </summary>
        public bool RunChecksum { get; set; }

        /// <inheritdoc />
        public string FilePath { get; set; }
    }
}
