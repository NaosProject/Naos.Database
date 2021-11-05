// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringSerializedIdentifier.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Representation.System;
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
            TypeRepresentation identifierType)
        {
            this.StringSerializedId = stringSerializedId;
            this.IdentifierType = identifierType;
        }

        /// <summary>
        /// Gets the string serialized identifier.
        /// </summary>
        public string StringSerializedId { get; private set; }

        /// <summary>
        /// Gets the type of the identifier.
        /// </summary>
        public TypeRepresentation IdentifierType { get; private set; }
    }
}
