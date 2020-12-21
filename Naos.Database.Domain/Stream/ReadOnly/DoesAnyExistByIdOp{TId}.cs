// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoesAnyExistByIdOp{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.Protocol.Domain;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;
    using static System.FormattableString;

    /// <summary>
    /// Gets a value indicating whether or not any record by the provided identifier exists.
    /// </summary>
    /// <typeparam name="TId">Type of the identifier.</typeparam>
    public partial class DoesAnyExistByIdOp<TId> : ReturningOperationBase<bool>, ISpecifyResourceLocator, IIdentifiableBy<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoesAnyExistByIdOp{TId}"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="objectType">The optional type of the object; default is no filter.</param>
        /// <param name="typeVersionMatchStrategy">The type version match strategy.</param>
        /// <param name="specifiedResourceLocator">The optional locator to use; DEFAULT will assume single locator on stream or throw.</param>
        public DoesAnyExistByIdOp(
            TId id,
            TypeRepresentation objectType = null,
            TypeVersionMatchStrategy typeVersionMatchStrategy = TypeVersionMatchStrategy.Any,
            IResourceLocator specifiedResourceLocator = null)
        {
            this.Id = id;
            this.ObjectType = objectType;
            this.TypeVersionMatchStrategy = typeVersionMatchStrategy;
            this.SpecifiedResourceLocator = specifiedResourceLocator;
        }

        /// <inheritdoc />
        public TId Id { get; private set; }

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
