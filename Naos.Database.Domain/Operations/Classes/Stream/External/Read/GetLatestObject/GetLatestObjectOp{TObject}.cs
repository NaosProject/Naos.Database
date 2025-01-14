// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestObjectOp{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the most recent object.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public partial class GetLatestObjectOp<TObject> : ReturningOperationBase<TObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestObjectOp{TObject}"/> class.
        /// </summary>
        /// <param name="identifierType">OPTIONAL type of the identifier to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="recordNotFoundStrategy">OPTIONAL strategy to use when no record(s) are found.  DEFAULT is to return the default of object type.</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        public GetLatestObjectOp(
            TypeRepresentation identifierType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            RecordNotFoundStrategy recordNotFoundStrategy = RecordNotFoundStrategy.ReturnDefault,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null)
        {
            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();
            recordNotFoundStrategy.MustForArg(nameof(recordNotFoundStrategy)).NotBeEqualTo(RecordNotFoundStrategy.Unknown);
            deprecatedIdTypes.MustForArg(nameof(deprecatedIdTypes)).NotContainAnyNullElementsWhenNotNull();

            this.IdentifierType = identifierType;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.RecordNotFoundStrategy = recordNotFoundStrategy;
            this.DeprecatedIdTypes = deprecatedIdTypes;
        }

        /// <summary>
        /// Gets the type of identifier to filter on or null for no filter.
        /// </summary>
        public TypeRepresentation IdentifierType { get; private set; }

        /// <summary>
        /// Gets the strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).
        /// </summary>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the strategy to use when no record(s) are found.
        /// </summary>
        public RecordNotFoundStrategy RecordNotFoundStrategy { get; private set; }

        /// <summary>
        /// Gets the object types used in a record that indicates an identifier deprecation.
        /// </summary>
        public IReadOnlyCollection<TypeRepresentation> DeprecatedIdTypes { get; private set; }
    }
}
