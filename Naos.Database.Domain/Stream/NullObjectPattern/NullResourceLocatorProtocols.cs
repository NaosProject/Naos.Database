// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullResourceLocatorProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;

    using OBeautifulCode.Type;

    /// <summary>
    /// A null object to be used as the id of an object in a <see cref="IReadWriteStream"/> that does not have an actual identifier.
    /// </summary>
    public partial class NullResourceLocatorProtocols : IResourceLocatorProtocols
    {
        /// <inheritdoc />
        public IReadOnlyCollection<IResourceLocator> Execute(
            GetAllResourceLocatorsOp operation)
        {
            return new[]
                   {
                       new NullDatabaseLocator(),
                   };
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<IResourceLocator>> ExecuteAsync(
            GetAllResourceLocatorsOp operation)
        {
            return await Task.FromResult(
                new[]
                {
                    new NullDatabaseLocator(),
                });
        }

        /// <inheritdoc />
        public IResourceLocator Execute(
            GetResourceLocatorForUniqueIdentifierOp operation)
        {
            return new NullDatabaseLocator();
        }

        /// <inheritdoc />
        public async Task<IResourceLocator> ExecuteAsync(
            GetResourceLocatorForUniqueIdentifierOp operation)
        {
            return await Task.FromResult(new NullDatabaseLocator());
        }

        /// <inheritdoc />
        public ISyncAndAsyncReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator> GetResourceLocatorByIdProtocol<TId>()
        {
            return new LambdaReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator>(_ => new NullDatabaseLocator());
        }
    }
}
