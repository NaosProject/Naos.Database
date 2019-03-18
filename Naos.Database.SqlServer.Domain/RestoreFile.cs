// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestoreFile.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer.Domain
{
    using System.Diagnostics.CodeAnalysis;

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
        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Matches sys schema in SQL Server.")]
        public string Type { get; set; }
    }
}
