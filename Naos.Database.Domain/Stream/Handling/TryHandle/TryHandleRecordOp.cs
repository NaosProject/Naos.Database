// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryHandleRecordOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Type;

    /// <summary>
    /// Handles a record.
    /// </summary>
    public partial class TryHandleRecordOp : ReturningOperationBase<StreamRecord>, ISpecifyResourceLocator, IHaveTags
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TryHandleRecordOp"/> class.
        /// </summary>
        /// <param name="concern">The concern.</param>
        /// <param name="identifierType">The optional type of the identifier; default is no filter.</param>
        /// <param name="objectType">The optional type of the object; default is no filter.</param>
        /// <param name="typeVersionMatchStrategy">The optional type version match strategy; DEFAULT is Any.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        /// <param name="tags">The optional tags to write with produced events.</param>
        public TryHandleRecordOp(
            string concern,
            TypeRepresentationWithAndWithoutVersion identifierType = null,
            TypeRepresentationWithAndWithoutVersion objectType = null,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any,
            IResourceLocator specifiedResourceLocator = null,
            IReadOnlyDictionary<string, string> tags = null)
        {
            this.Concern = concern;
            this.IdentifierType = identifierType;
            this.ObjectType = objectType;
            this.TypeVersionMatchStrategy = typeVersionMatchStrategy;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
            this.Tags = tags;
        }

        /// <summary>
        /// Gets the concern.
        /// </summary>
        /// <value>The concern.</value>
        public string Concern { get; private set; }

        /// <summary>
        /// Gets the type of the identifier.
        /// </summary>
        /// <value>The type of the identifier.</value>
        public TypeRepresentationWithAndWithoutVersion IdentifierType { get; private set; }

        /// <summary>
        /// Gets the type of the object.
        /// </summary>
        /// <value>The type of the object.</value>
        public TypeRepresentationWithAndWithoutVersion ObjectType { get; private set; }

        /// <summary>
        /// Gets the type version match strategy.
        /// </summary>
        /// <value>The type version match strategy.</value>
        public TypeVersionMatchStrategy TypeVersionMatchStrategy { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Tags { get; private set; }
    }
}
