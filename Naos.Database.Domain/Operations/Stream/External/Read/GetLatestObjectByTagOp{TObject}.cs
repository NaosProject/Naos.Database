// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestObjectByTagOp{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the most recent object with the specified tag.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public partial class GetLatestObjectByTagOp<TObject> : ReturningOperationBase<TObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestObjectByTagOp{TObject}"/> class.
        /// </summary>
        /// <param name="tag">The tag to match.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the object type.  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public GetLatestObjectByTagOp(
            NamedValue<string> tag,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault)
        {
            tag.MustForArg().NotBeNull();
            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();

            this.Tag = tag;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.RecordNotFoundStrategy = recordNotFoundStrategy;
        }

        /// <summary>
        /// Gets the tag to match.
        /// </summary>
        public NamedValue<string> Tag { get; private set; }

        /// <summary>
        /// Gets the strategy to use to filter on the version of the object type.
        /// </summary>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the strategy to use when no record(s) are found.
        /// </summary>
        public RecordNotFoundStrategy RecordNotFoundStrategy { get; private set; }
    }
}
