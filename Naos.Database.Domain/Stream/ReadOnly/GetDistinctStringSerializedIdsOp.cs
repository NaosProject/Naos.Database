// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetDistinctStringSerializedIdsOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;

    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Gets the latest record.
    /// </summary>
    public partial class GetDistinctStringSerializedIdsOp : ReturningOperationBase<IReadOnlyCollection<string>>, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetDistinctStringSerializedIdsOp"/> class.
        /// </summary>
        /// <param name="identifierType">The optional type of the identifier; default is no filter.</param>
        /// <param name="objectType">The optional type of the object; default is no filter.</param>
        /// <param name="versionMatchStrategy">The type version match strategy.</param>
        /// <param name="tagsToMatch">The optional tags to match; default is no filter.</param>
        /// <param name="tagMatchStrategy">The optional strategy for comparing tags; DEFAULT is <see cref="TagMatchStrategy"/>.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        public GetDistinctStringSerializedIdsOp(
            TypeRepresentation identifierType = null,
            TypeRepresentation objectType = null,
            VersionMatchStrategy versionMatchStrategy = VersionMatchStrategy.Any,
            IReadOnlyCollection<NamedValue<string>> tagsToMatch = null,
            TagMatchStrategy tagMatchStrategy = null,
            IResourceLocator specifiedResourceLocator = null)
        {
            this.IdentifierType = identifierType;
            this.ObjectType = objectType;
            this.VersionMatchStrategy = versionMatchStrategy;
            this.TagsToMatch = tagsToMatch;
            this.TagMatchStrategy = tagMatchStrategy;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

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
        public VersionMatchStrategy VersionMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the tag match strategy.
        /// </summary>
        /// <value>The tag match strategy.</value>
        public TagMatchStrategy TagMatchStrategy { get; private set; }

        /// <summary>
        /// Gets the tags to match.
        /// </summary>
        /// <value>The tags to match.</value>
        public IReadOnlyCollection<NamedValue<string>> TagsToMatch { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
