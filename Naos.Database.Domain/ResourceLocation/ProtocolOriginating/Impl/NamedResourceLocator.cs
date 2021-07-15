// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NamedResourceLocator.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// A general purpose resource locator operating on a name.
    /// </summary>
    public partial class NamedResourceLocator : ResourceLocatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedResourceLocator"/> class.
        /// </summary>
        /// <param name="name">The name of the resource locator.</param>
        public NamedResourceLocator(
            string name)
        {
            new { name }.AsArg().Must().NotBeNullNorWhiteSpace();

            this.Name = name;
        }

        /// <summary>
        /// Gets the name of the resource locator.
        /// </summary>
        public string Name { get; private set; }
    }
}
