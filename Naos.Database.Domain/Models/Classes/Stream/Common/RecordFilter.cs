// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordFilter.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Filter set to apply to a set of <see cref="StreamRecord"/>'s.
    /// </summary>
    public partial class RecordFilter : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordFilter"/> class.
        /// </summary>
        /// <param name="internalRecordIds">The internal record identifiers to match or null when not matching on internal record identifiers.</param>
        /// <param name="ids">The string serialized object identifiers to match on or null when not matching on object id.</param>
        /// <param name="idTypes">The identifier types to match on or null when not matching on object type.</param>
        /// <param name="objectTypes">The object types to match on or null when not matching on object type.</param>
        /// <param name="versionMatchStrategy">The strategy to use to filter on the version of the identifier and/or object type.</param>
        /// <param name="tags">The tags to match or null when not matching on tags.</param>
        /// <param name="tagMatchStrategy">The strategy to use for comparing tags when <see cref="Tags"/> is specified.</param>
        public RecordFilter(
            IReadOnlyCollection<long> internalRecordIds = null,
            IReadOnlyCollection<StringSerializedIdentifier> ids = null,
#pragma warning disable SA1305 // Field names should not use Hungarian notation
            IReadOnlyCollection<TypeRepresentation> idTypes = null,
#pragma warning restore SA1305 // Field names should not use Hungarian notation
            IReadOnlyCollection<TypeRepresentation> objectTypes = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tags = null,
            TagMatchStrategy tagMatchStrategy = TagMatchStrategy.RecordContainsAllQueryTags)
        {
            this.InternalRecordIds = internalRecordIds;
            this.Ids = ids;
            this.IdTypes = idTypes;
            this.ObjectTypes = objectTypes;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.Tags = tags;
            this.TagMatchStrategy = tagMatchStrategy;
        }

        /// <summary>
        /// Gets the internal record identifiers to match or null when not matching on internal record identifiers.
        /// </summary>
        public IReadOnlyCollection<long> InternalRecordIds { get; private set; }

        /// <summary>
        /// Gets the string serialized object identifiers to match on or null when not matching on object id.
        /// </summary>
        public IReadOnlyCollection<StringSerializedIdentifier> Ids { get; private set; }

        /// <summary>
        /// Gets the identifier types to match on or null when not matching on object type.
        /// </summary>
        public IReadOnlyCollection<TypeRepresentation> IdTypes { get; private set; }

        /// <summary>
        /// Gets the object types to match on or null when not matching on object type.
        /// </summary>
        public IReadOnlyCollection<TypeRepresentation> ObjectTypes { get; private set; }

        /// <summary>
        /// Gets the strategy to use to filter on the version of the identifier and/or object type.
        /// </summary>
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the tags to match or null when not matching on tags.
        /// </summary>
        public IReadOnlyCollection<NamedValue<string>> Tags { get; private set; }

        /// <summary>
        /// Gets the strategy to use for comparing tags when <see cref="Tags"/> is specified.
        /// </summary>
        public TagMatchStrategy TagMatchStrategy { get; private set; }
    }
}
