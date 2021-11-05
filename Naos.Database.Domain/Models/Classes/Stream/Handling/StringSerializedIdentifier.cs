// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringSerializedIdentifier.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
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
        /// <param name="stringSerializedId">The object's identifier serialized as a string.</param>
        /// <param name="identifierType">Type of the identifier.</param>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public StringSerializedIdentifier(
            string stringSerializedId,
            TypeRepresentation identifierType)
        {
            identifierType.MustForArg(nameof(identifierType)).NotBeNull();

            this.StringSerializedId = stringSerializedId;
            this.IdentifierType = identifierType;
        }

        /// <summary>
        /// Gets the object's identifier serialized as a string.
        /// </summary>
        public string StringSerializedId { get; private set; }

        /// <summary>
        /// Gets the type of the identifier.
        /// </summary>
        public TypeRepresentation IdentifierType { get; private set; }
    }
}
