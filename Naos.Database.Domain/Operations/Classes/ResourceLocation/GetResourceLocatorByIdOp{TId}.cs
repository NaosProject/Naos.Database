// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetResourceLocatorByIdOp{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Get the <see cref="IResourceLocator"/> by the ID.
    /// </summary>
    /// <remarks>
    /// This enables ID based sharding.
    /// </remarks>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    public partial class GetResourceLocatorByIdOp<TId> : ReturningOperationBase<IResourceLocator>, IHaveId<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetResourceLocatorByIdOp{TId}"/> class.
        /// </summary>
        /// <param name="id">The identifier of the object.</param>
        public GetResourceLocatorByIdOp(
            TId id)
        {
            this.Id = id;
        }

        /// <inheritdoc />
        public TId Id { get; private set; }
    }
}
