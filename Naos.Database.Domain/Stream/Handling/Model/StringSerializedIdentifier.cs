// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringSerializedIdentifier.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Container of a string serialized identifier with it's type.
    /// </summary>
    public partial class StringSerializedIdentifier : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringSerializedIdentifier"/> class.
        /// </summary>
        /// <param name="stringSerializedId">The string serialized identifier.</param>
        /// <param name="identifierType">Type of the identifier.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public StringSerializedIdentifier(
            string stringSerializedId,
            TypeRepresentationWithAndWithoutVersion identifierType)
        {
            this.StringSerializedId = stringSerializedId;
            this.IdentifierType = identifierType;
        }

        /// <summary>
        /// Gets the string serialized identifier.
        /// </summary>
        /// <value>The string serialized identifier.</value>
        public string StringSerializedId { get; private set; }

        /// <summary>
        /// Gets the type of the identifier.
        /// </summary>
        /// <value>The type of the identifier.</value>
        public TypeRepresentationWithAndWithoutVersion IdentifierType { get; private set; }
    }
}
