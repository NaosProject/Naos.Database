// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoesAnyExistByIdOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.CodeAnalysis.Recipes;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using static System.FormattableString;

    /// <summary>
    /// Gets a value indicating whether or not any record by the provided identifier exists.
    /// </summary>
    public partial class DoesAnyExistByIdOp : ReturningOperationBase<bool>, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoesAnyExistByIdOp"/> class.
        /// </summary>
        /// <param name="stringSerializedId">The string serialized identifier.</param>
        /// <param name="identifierType">The type of the identifier; default is no filter.</param>
        /// <param name="objectType">The optional type of the object; default is no filter.</param>
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public DoesAnyExistByIdOp(
            string stringSerializedId,
            TypeRepresentation identifierType,
            TypeRepresentation objectType = null,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any,
            IResourceLocator specifiedResourceLocator = null)
        {
            identifierType.MustForArg(nameof(identifierType)).NotBeNull();

            this.StringSerializedId = stringSerializedId;
            this.IdentifierType = identifierType;
            this.ObjectType = objectType;
            this.TypeVersionMatchStrategy = typeVersionMatchStrategy;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
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
        public TypeRepresentation IdentifierType { get; private set; }

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        /// <value>The type of the object.</value>
        public TypeRepresentation ObjectType { get; private set; }

        /// <summary>
        /// Gets the type version match strategy.
        /// </summary>
        /// <value>The type version match strategy.</value>
        public TypeVersionMatchStrategy TypeVersionMatchStrategy { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
