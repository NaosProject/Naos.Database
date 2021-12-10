// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleResourceLocatorProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Implements <see cref="IResourceLocatorProtocols"/> using a single provided <see cref="ResourceLocatorBase"/>.
    /// </summary>
    public sealed class SingleResourceLocatorProtocols
        : IResourceLocatorProtocols
    {
        private readonly IResourceLocator resourceLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleResourceLocatorProtocols"/> class.
        /// </summary>
        /// <param name="resourceLocator">The SQL stream locator.</param>
        public SingleResourceLocatorProtocols(
            IResourceLocator resourceLocator)
        {
            this.resourceLocator = resourceLocator ?? throw new ArgumentNullException(nameof(resourceLocator));
        }

        /// <inheritdoc />
        public IReadOnlyCollection<IResourceLocator> Execute(
            GetAllResourceLocatorsOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new[]
            {
                this.resourceLocator,
            };

            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<IResourceLocator>> ExecuteAsync(
            GetAllResourceLocatorsOp operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public ISyncAndAsyncReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator> GetResourceLocatorByIdProtocol<TId>()
        {
            var result = new LambdaReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator>(_ => this.resourceLocator);

            return result;
        }

        /// <inheritdoc />
        public IResourceLocator Execute(
            GetResourceLocatorForUniqueIdentifierOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = this.resourceLocator;

            return result;
        }

        /// <inheritdoc />
        public async Task<IResourceLocator> ExecuteAsync(
            GetResourceLocatorForUniqueIdentifierOp operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }
    }
}
