// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateDatabaseMessage.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Contract
{
    using Naos.Database.Contract;
    using Naos.MessageBus.DataContract;

    /// <summary>
    /// Message to create a database on the server the handler is on.
    /// </summary>
    public class CreateDatabaseMessage : IMessage, IShareDatabaseName
    {
        /// <inheritdoc />
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name of the database to create.
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the type of database.
        /// </summary>
        public DatabaseType DatabaseType { get; set; }

        /// <summary>
        /// Gets or sets the metadata name of the data file.
        /// </summary>
        public string DataFileLogicalName { get; set; }

        /// <summary>
        /// Gets or sets the name of the data file (combined with directory on server).
        /// </summary>
        public string DataFileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the log file (combined with directory on server).
        /// </summary>
        public string LogFileName { get; set; }

        /// <summary>
        /// Gets or sets the current size of data file in kilobytes.
        /// </summary>
        public long DataFileCurrentSizeInKb { get; set; }

        /// <summary>
        /// Gets or sets the max size of data file in kilobytes.
        /// </summary>
        public long DataFileMaxSizeInKb { get; set; }

        /// <summary>
        /// Gets or sets the size of growth interval of data file in kilobytes.
        /// </summary>
        public long DataFileGrowthSizeInKb { get; set; }

        /// <summary>
        /// Gets or sets the metadata name of the log file.
        /// </summary>
        public string LogFileLogicalName { get; set; }

        /// <summary>
        /// Gets or sets the current size of log file in kilobytes.
        /// </summary>
        public long LogFileCurrentSizeInKb { get; set; }

        /// <summary>
        /// Gets or sets the max size of data file in kilobytes.
        /// </summary>
        public long LogFileMaxSizeInKb { get; set; }

        /// <summary>
        /// Gets or sets the size of growth interval of log file in kilobytes.
        /// </summary>
        public long LogFileGrowthSizeInKb { get; set; }
    }
}
