// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardGetLatestRecordByIdOp.cs" company="Naos Project">
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
    /// Gets the most recent record with provided identifier.
    /// </summary>
    /// <remarks>
    /// This is an internal operation; it is designed to honor the contract of an <see cref="IStandardStream"/>.
    /// While technically there are no limitations on who may execute this operation on such a stream,
    /// these are "bare metal" operations and can be misused without a deeper understanding of what will happen.
    /// Most typically, you will use the operations that are exposed via these extension methods
    /// <see cref="ReadOnlyStreamExtensions"/> and <see cref="WriteOnlyStreamExtensions"/>.
    /// </remarks>
    public partial class StandardGetLatestRecordByIdOp : ReturningOperationBase<StreamRecord>, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardGetLatestRecordByIdOp"/> class.
        /// </summary>
        /// <param name="stringSerializedId">The object's identifier serialized as a string.</param>
        /// <param name="identifierType">OPTIONAL type of the identifier to filter on.  DEFAULT is no filter.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <param name="specifiedResourceLocator">OPTIONAL locator to use. DEFAULT will assume single locator on stream or throw.</param>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "string", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public StandardGetLatestRecordByIdOp(
            string stringSerializedId,
            TypeRepresentation identifierType = null,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault,
            IResourceLocator specifiedResourceLocator = null)
        {
            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();
            recordNotFoundStrategy.MustForArg(nameof(recordNotFoundStrategy)).NotBeEqualTo(RecordNotFoundStrategy.Unknown);

            this.StringSerializedId = stringSerializedId;
            this.IdentifierType = identifierType;
            this.ObjectType = objectType;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.RecordNotFoundStrategy = recordNotFoundStrategy;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <summary>
        /// Gets the object's identifier serialized as a string.
        /// </summary>
        public string StringSerializedId { get; private set; }

        /// <summary>
        /// Gets the type of identifier to filter on or null for no filter.
        /// </summary>
        public TypeRepresentation IdentifierType { get; private set; }

        /// <summary>
        /// Gets the type object to filter on or null for no filter.
        /// </summary>
        public TypeRepresentation ObjectType { get; private set; }

        /// <summary>
        /// Gets the strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).
        /// </summary>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the strategy to use when no record(s) are found.
        /// </summary>
        public RecordNotFoundStrategy RecordNotFoundStrategy { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
