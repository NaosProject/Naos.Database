// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystemDatabaseLocator.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// File system implementation of <see cref="DatabaseLocatorBase"/>.
    /// </summary>
    public partial class FileSystemDatabaseLocator : DatabaseLocatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemDatabaseLocator"/> class.
        /// </summary>
        /// <param name="rootFolderPath">The root folder path.</param>
        public FileSystemDatabaseLocator(
            string rootFolderPath)
        {
            rootFolderPath.MustForArg(nameof(rootFolderPath)).NotBeNullNorWhiteSpace();

            this.RootFolderPath = rootFolderPath;
        }

        /// <summary>
        /// Gets the root folder path.
        /// </summary>
        public string RootFolderPath { get; private set; }
    }
}
