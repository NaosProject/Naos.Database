// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryHandleRecordOp{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;

    /// <summary>
    /// Try to handle a record of type <typeparamref name="TObject"/> for a specified concern.
    /// </summary>
    /// <typeparam name="TObject">Type of the object in the record.</typeparam>
    public partial class TryHandleRecordOp<TObject> : ReturningOperationBase<StreamRecord<TObject>>, ISpecifyResourceLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TryHandleRecordOp{TObject}"/> class.
        /// </summary>
        /// <param name="concern">The concern.</param>
        /// <param name="identifierType">The optional type of the identifier; default is no filter.</param>
        /// <param name="typeVersionMatchStrategy">The optional type version match strategy; DEFAULT is Any.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        public TryHandleRecordOp(
            string concern,
            TypeRepresentationWithAndWithoutVersion identifierType = null,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any,
            IResourceLocator specifiedResourceLocator = null)
        {
            this.Concern = concern;
            this.IdentifierType = identifierType;
            this.TypeVersionMatchStrategy = typeVersionMatchStrategy;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
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
        /// Gets the type version match strategy.
        /// </summary>
        /// <value>The type version match strategy.</value>
        public TypeVersionMatchStrategy TypeVersionMatchStrategy { get; private set; }

        /// <inheritdoc />
        public IResourceLocator SpecifiedResourceLocator { get; private set; }
    }
}
