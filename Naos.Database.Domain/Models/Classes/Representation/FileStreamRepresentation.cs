// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamRepresentation.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// A representation of a file-based stream.
    /// </summary>
    public partial class FileStreamRepresentation : StreamRepresentationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileStreamRepresentation"/> class.
        /// </summary>
        /// <param name="name">The <see cref="FileSystemDatabaseLocator"/>'s associated with the stream.</param>
        /// <param name="fileSystemDatabaseLocators">The root directory file path.</param>
        public FileStreamRepresentation(
            string name,
            IReadOnlyList<FileSystemDatabaseLocator> fileSystemDatabaseLocators)
            : base(name)
        {
            fileSystemDatabaseLocators.MustForArg(nameof(fileSystemDatabaseLocators)).NotBeNullNorEmptyEnumerableNorContainAnyNulls();

            this.FileSystemDatabaseLocators = fileSystemDatabaseLocators;
        }

        /// <summary>
        /// Gets the <see cref="FileSystemDatabaseLocator"/>'s associated with the stream.
        /// </summary>
        public IReadOnlyList<FileSystemDatabaseLocator> FileSystemDatabaseLocators { get; private set; }
    }
}
