// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseConfiguration.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools
{
    /// <summary>
    /// Detailed information about the database's configuration (file size and name type stuff).
    /// </summary>
    public class DatabaseConfiguration
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of database.
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the metadata name of the data file.
        /// </summary>
        public string DataFileLogicalName { get; set; }

        /// <summary>
        /// Gets or sets the full path to the data file.
        /// </summary>
        public string DataFilePath { get; set; }

        /// <summary>
        /// Gets or sets the full path to the log file.
        /// </summary>
        public string LogFilePath { get; set; }

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

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a copy of the current object.
        /// </summary>
        /// <returns>A copy of the current object.</returns>
        public DatabaseConfiguration Clone()
        {
            return new DatabaseConfiguration()
                       {
                           DatabaseName = this.DatabaseName,
                           DataFileLogicalName = this.DataFileLogicalName,
                           DataFilePath = this.DataFilePath,
                           DataFileCurrentSizeInKb = this.DataFileCurrentSizeInKb,
                           DataFileMaxSizeInKb = this.DataFileMaxSizeInKb,
                           DataFileGrowthSizeInKb = this.DataFileGrowthSizeInKb,
                           LogFileLogicalName = this.LogFileLogicalName,
                           LogFilePath = this.LogFilePath,
                           LogFileCurrentSizeInKb = this.LogFileCurrentSizeInKb,
                           LogFileMaxSizeInKb = this.LogFileMaxSizeInKb,
                           LogFileGrowthSizeInKb = this.LogFileGrowthSizeInKb,
                       };
        }

        /// <summary>
        /// Creates a copy of the current object while also updating the database name to the new value.
        /// </summary>
        /// <param name="newDatabaseName">New database name to be updated in the cloned object.</param>
        /// <returns>A copy of the current object with the new database name assigned.</returns>
        public DatabaseConfiguration CloneWithNewDatabaseName(string newDatabaseName)
        {
            var clone = this.Clone();
            clone.DatabaseName = newDatabaseName;
            return clone;
        }

        #endregion
    }
}
