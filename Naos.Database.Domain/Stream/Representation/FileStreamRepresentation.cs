// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStreamRepresentation.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Stream description to allow the <see cref="GetStreamFromRepresentationByNameProtocolFactory"/> to produce the correct stream.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public partial class FileStreamRepresentation : StreamRepresentationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileStreamRepresentation"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
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
        /// <value>The <see cref="FileSystemDatabaseLocator"/>'s associated with the stream.</value>
        public IReadOnlyList<FileSystemDatabaseLocator> FileSystemDatabaseLocators { get; private set; }
    }
}
