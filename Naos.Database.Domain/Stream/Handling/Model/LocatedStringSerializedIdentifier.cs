// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocatedStringSerializedIdentifier.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;
    using OBeautifulCode.Type;

    /// <summary>
    /// Container of a string serialized identifier with it's type and it's correct locator.
    /// </summary>
    public class LocatedStringSerializedIdentifier : ISpecifyResourceLocator, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocatedStringSerializedIdentifier"/> class.
        /// </summary>
        /// <param name="stringSerializedId">The string serialized identifier.</param>
        /// <param name="specifiedResourceLocator">Locator for the identifier.</param>
        public LocatedStringSerializedIdentifier(
            StringSerializedIdentifier stringSerializedId,
            IResourceLocator specifiedResourceLocator)
        {
            this.StringSerializedId = stringSerializedId;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <summary>
        /// Gets the string serialized identifier.
        /// </summary>
        /// <value>The string serialized identifier.</value>
        public StringSerializedIdentifier StringSerializedId { get; private set; }

        /// <summary>
        /// Gets the type of the identifier.
        /// </summary>
        /// <value>The type of the identifier.</value>
        public TypeRepresentationWithAndWithoutVersion IdentifierType { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
