// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullResourceLocatorProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// A null object pattern implementation of <see cref="IResourceLocatorProtocols"/>.
    /// </summary>
    public class NullResourceLocatorProtocols : IResourceLocatorProtocols
    {
        /// <inheritdoc />
        public IReadOnlyCollection<IResourceLocator> Execute(
            GetAllResourceLocatorsOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new[]
            {
                new NullResourceLocator(),
            };

            return result;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<IResourceLocator>> ExecuteAsync(
            GetAllResourceLocatorsOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = await Task.FromResult(
                new[]
                {
                    new NullResourceLocator(),
                });

            return result;
        }

        /// <inheritdoc />
        public IResourceLocator Execute(
            GetResourceLocatorForUniqueIdentifierOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = new NullResourceLocator();

            return result;
        }

        /// <inheritdoc />
        public async Task<IResourceLocator> ExecuteAsync(
            GetResourceLocatorForUniqueIdentifierOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = await Task.FromResult(new NullResourceLocator());

            return result;
        }

        /// <inheritdoc />
        public ISyncAndAsyncReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator> GetResourceLocatorByIdProtocol<TId>()
        {
            var result = new LambdaReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator>(_ => new NullResourceLocator());

            return result;
        }
    }
}
