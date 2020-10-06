// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryDatabaseLocator.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// In memory implementation of <see cref="DatabaseLocatorBase"/>.
    /// </summary>
    public partial class MemoryDatabaseLocator : DatabaseLocatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryDatabaseLocator"/> class.
        /// </summary>
        /// <param name="name">The root folder path.</param>
        public MemoryDatabaseLocator(
            string name)
        {
            name.MustForArg(nameof(name)).NotBeNullNorWhiteSpace();

            this.Name = name;
        }

        /// <summary>
        /// Gets the root folder path.
        /// </summary>
        /// <value>The root folder path.</value>
        public string Name { get; private set; }
    }
}
