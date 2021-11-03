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
    /// <typeparam name="TId">Type of ID being used.</typeparam>
    public partial class GetResourceLocatorByIdOp<TId> : ReturningOperationBase<IResourceLocator>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetResourceLocatorByIdOp{TId}"/> class.
        /// </summary>
        /// <param name="id">The ID.</param>
        public GetResourceLocatorByIdOp(
            TId id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public TId Id { get; private set; }
    }
}
