// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestoreFile.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools
{
    /// <summary>
    /// Represents a file to restore.
    /// </summary>
    internal class RestoreFile
    {
        /// <summary>
        /// Gets or sets the metadata name of the file.
        /// </summary>
        public string LogicalName { get; set; }

        /// <summary>
        /// Gets or sets the path to the file.
        /// </summary>
        public string PhysicalName { get; set; }

        /// <summary>
        /// Gets or sets the type of file.
        /// </summary>
        public string Type { get; set; }
    }
}
