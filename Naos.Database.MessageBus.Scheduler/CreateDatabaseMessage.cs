// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateDatabaseMessage.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MessageBus.Scheduler
{
    using Naos.Database.Domain;
    using Naos.MessageBus.Domain;

    /// <summary>
    /// Message to create a database on the server the handler is on.
    /// </summary>
    public class CreateDatabaseMessage : IMessage, IShareDatabaseName
    {
        /// <inheritdoc />
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the kind of database.
        /// </summary>
        public DatabaseKind DatabaseKind { get; set; }

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Kb", Justification = "Spelling/name is correct.")]
        public long DataFileCurrentSizeInKb { get; set; }

        /// <summary>
        /// Gets or sets the max size of data file in kilobytes.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Kb", Justification = "Spelling/name is correct.")]
        public long DataFileMaxSizeInKb { get; set; }

        /// <summary>
        /// Gets or sets the size of growth interval of data file in kilobytes.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Kb", Justification = "Spelling/name is correct.")]
        public long DataFileGrowthSizeInKb { get; set; }

        /// <summary>
        /// Gets or sets the metadata name of the log file.
        /// </summary>
        public string LogFileLogicalName { get; set; }

        /// <summary>
        /// Gets or sets the current size of log file in kilobytes.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Kb", Justification = "Spelling/name is correct.")]
        public long LogFileCurrentSizeInKb { get; set; }

        /// <summary>
        /// Gets or sets the max size of data file in kilobytes.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Kb", Justification = "Spelling/name is correct.")]
        public long LogFileMaxSizeInKb { get; set; }

        /// <summary>
        /// Gets or sets the size of growth interval of log file in kilobytes.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Kb", Justification = "Spelling/name is correct.")]
        public long LogFileGrowthSizeInKb { get; set; }
    }
}
