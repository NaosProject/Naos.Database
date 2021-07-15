// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleResourceLocatorProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OBeautifulCode.Type;

    /// <summary>
    /// Implements <see cref="IResourceLocatorProtocols"/> using a single provided <see cref="ResourceLocatorBase"/>.
    /// </summary>
    public sealed partial class SingleResourceLocatorProtocol
        : IResourceLocatorProtocols
    {
        private readonly IResourceLocator resourceLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleResourceLocatorProtocol"/> class.
        /// </summary>
        /// <param name="resourceLocator">The SQL stream locator.</param>
        public SingleResourceLocatorProtocol(IResourceLocator resourceLocator)
        {
            this.resourceLocator = resourceLocator ?? throw new ArgumentNullException(nameof(resourceLocator));
        }

        /// <inheritdoc />
        public IReadOnlyCollection<IResourceLocator> Execute(
            GetAllResourceLocatorsOp operation)
        {
            return new[]
                   {
                       this.resourceLocator,
                   };
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<IResourceLocator>> ExecuteAsync(
            GetAllResourceLocatorsOp operation)
        {
            return await Task.FromResult(this.Execute(operation));
        }

        /// <inheritdoc />
        public ISyncAndAsyncReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator> GetResourceLocatorByIdProtocol<TId>()
        {
            return new LambdaReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator>(_ => this.resourceLocator);
        }

        /// <inheritdoc />
        public IResourceLocator Execute(
            GetResourceLocatorForUniqueIdentifierOp operation)
        {
            return this.resourceLocator;
        }

        /// <inheritdoc />
        public async Task<IResourceLocator> ExecuteAsync(
            GetResourceLocatorForUniqueIdentifierOp operation)
        {
            var syncResult = this.Execute(operation);
            return await Task.FromResult(syncResult);
        }
    }
}
