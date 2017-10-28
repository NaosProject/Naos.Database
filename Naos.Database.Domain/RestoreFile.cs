// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestoreFile.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Represents a file to restore.
    /// </summary>
    public class RestoreFile
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
        public string FileType { get; set; }
    }
}
