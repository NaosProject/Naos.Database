﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoesAnyExistByIdOp{TId}.cs" company="Naos Project">
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
    /// Gets a value indicating whether or not any record by the provided identifier exists.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public partial class DoesAnyExistByIdOp<TId> : ReturningOperationBase<bool>, IHaveId<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoesAnyExistByIdOp{TId}"/> class.
        /// </summary>
        /// <param name="id">The identifier of the object.</param>
        /// <param name="objectType">OPTIONAL type of the object to filter on.  DEFAULT is no filter.</param>
        /// <param name="versionMatchStrategy">OPTIONAL strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is no filter (any version is acceptable).</param>
        /// <param name="deprecatedIdTypes">OPTIONAL object types used in a record that indicates an identifier deprecation.  DEFAULT is no deprecated types specified.  Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/> for how deprecation works.</param>
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        public DoesAnyExistByIdOp(
            TId id,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes = null,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType)
        {
            versionMatchStrategy.ThrowOnUnsupportedVersionMatchStrategyForType();
            deprecatedIdTypes.MustForArg(nameof(deprecatedIdTypes)).NotContainAnyNullElementsWhenNotNull();
            typeSelectionStrategy.MustForArg(nameof(typeSelectionStrategy)).NotBeEqualTo(TypeSelectionStrategy.Unknown);

            this.Id = id;
            this.ObjectType = objectType;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.DeprecatedIdTypes = deprecatedIdTypes;
            this.TypeSelectionStrategy = typeSelectionStrategy;
        }

        /// <inheritdoc />
        public TId Id { get; private set; }

        /// <summary>
        /// Gets the type object to filter on or null for no filter.
        /// </summary>
        public TypeRepresentation ObjectType { get; private set; }

        /// <summary>
        /// Gets the strategy to use to filter on the version of the queried types that are applicable to this operation (e.g. object type, object's identifier type).
        /// </summary>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the object types used in a record that indicates an identifier deprecation.
        /// </summary>
        public IReadOnlyCollection<TypeRepresentation> DeprecatedIdTypes { get; private set; }

        /// <summary>
        /// Gets the strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).
        /// </summary>
        public TypeSelectionStrategy TypeSelectionStrategy { get; private set; }
    }
}
