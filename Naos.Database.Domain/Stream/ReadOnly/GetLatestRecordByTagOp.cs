// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestRecordByTagOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Gets the latest record with provided tag.
    /// </summary>
    public partial class GetLatestRecordByTagOp : ReturningOperationBase<StreamRecord>, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestRecordByTagOp"/> class.
        /// </summary>
        /// <param name="tag">The tag to match.</param>
        /// <param name="objectType">The optional type of the object; default is no filter.</param>
        /// <param name="versionMatchStrategy">The type version match strategy.</param>
        /// <param name="existingRecordNotEncounteredStrategy">The optional strategy on how to deal with no matching record; DEFAULT is the default of the requested type or null.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public GetLatestRecordByTagOp(
            NamedValue<string> tag,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            ExistingRecordNotEncounteredStrategy existingRecordNotEncounteredStrategy = ExistingRecordNotEncounteredStrategy.ReturnDefault,
            IResourceLocator specifiedResourceLocator = null)
        {
            tag.MustForArg().NotBeNull();
            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();

            this.Tag = tag;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.ObjectType = objectType;
            this.ExistingRecordNotEncounteredStrategy = existingRecordNotEncounteredStrategy;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <summary>
        /// Gets the tag to match.
        /// </summary>
        /// <value>The tag name to match.</value>
        public NamedValue<string> Tag { get; private set; }

        /// <summary>
        /// Gets the type version match strategy.
        /// </summary>
        /// <value>The type version match strategy.</value>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        /// <value>The type of the object.</value>
        public TypeRepresentation ObjectType { get; private set; }

        /// <summary>
        /// Gets the existing record not encountered strategy.
        /// </summary>
        /// <value>The existing record not encountered strategy.</value>
        public ExistingRecordNotEncounteredStrategy ExistingRecordNotEncounteredStrategy { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
