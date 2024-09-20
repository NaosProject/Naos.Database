// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PassThroughResourceLocatorProtocols{TSupportedId}.cs" company="Naos Project">
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
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Generic <see cref="IResourceLocatorProtocols"/> implementation to easily contain sharding logic.
    /// </summary>
    /// <typeparam name="TSupportedId">Supported identifier type.</typeparam>
    /// <remarks>
    /// Implementation of <see cref="IResourceLocator"/> protocols to be provided to a stream that satisfies the contract
    /// while allowing multiple locators of any type to be supplied and a selection function for choosing said locator by id.
    /// todo: seems like getResourceLocatorByIdOpFunc should just be a protocol, not a func.
    /// </remarks>
    public class PassThroughResourceLocatorProtocols<TSupportedId>
        : IResourceLocatorProtocols
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PassThroughResourceLocatorProtocols{TSupportedId}"/> class.
        /// </summary>
        /// <param name="allLocators">All locators.</param>
        /// <param name="resourceLocatorForUniqueIdentifier">The resource locator for unique identifiers.</param>
        /// <param name="getResourceLocatorByIdOpFunc">The resource locator by identifier protocol.</param>
        public PassThroughResourceLocatorProtocols(
            IReadOnlyCollection<IResourceLocator> allLocators,
            IResourceLocator resourceLocatorForUniqueIdentifier,
            Func<GetResourceLocatorByIdOp<TSupportedId>, IResourceLocator> getResourceLocatorByIdOpFunc)
        {
            allLocators.MustForArg(nameof(allLocators)).NotBeNullNorEmptyEnumerableNorContainAnyNulls();

            this.AllLocators = allLocators;
            this.ResourceLocatorForUniqueIdentifier = resourceLocatorForUniqueIdentifier ?? throw new ArgumentNullException(nameof(resourceLocatorForUniqueIdentifier));
            this.GetResourceLocatorByIdOpFunc = getResourceLocatorByIdOpFunc ?? throw new ArgumentNullException(nameof(getResourceLocatorByIdOpFunc));
        }

        /// <summary>
        /// Gets all locators.
        /// </summary>
        public IReadOnlyCollection<IResourceLocator> AllLocators { get; private set; }

        /// <summary>
        /// Gets the resource locator for unique identifiers.
        /// </summary>
        public IResourceLocator ResourceLocatorForUniqueIdentifier { get; private set; }

        /// <summary>
        /// Gets the resource locator by identifier protocol.
        /// </summary>
        public Func<GetResourceLocatorByIdOp<TSupportedId>, IResourceLocator> GetResourceLocatorByIdOpFunc { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<IResourceLocator> Execute(
            GetAllResourceLocatorsOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result =  this.AllLocators;

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
        public IResourceLocator Execute(
            GetResourceLocatorForUniqueIdentifierOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var result = this.ResourceLocatorForUniqueIdentifier;

            return result;
        }

        /// <inheritdoc />
        public async Task<IResourceLocator> ExecuteAsync(
            GetResourceLocatorForUniqueIdentifierOp operation)
        {
            var result = await Task.FromResult(this.Execute(operation));

            return result;
        }

        /// <inheritdoc />
        public ISyncAndAsyncReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator> GetResourceLocatorByIdProtocol<TId>()
        {
            if (typeof(TId) != typeof(TSupportedId))
            {
                throw new ArgumentException(Invariant($"Only {typeof(TSupportedId).ToStringReadable()} is supported (not provided {typeof(TId).ToStringReadable()})."));
            }

            // ReSharper disable once SuspiciousTypeConversion.Global - this has been confirmed to be the same type and thus will cast fine...
            var result = (ISyncAndAsyncReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator>)new LambdaReturningProtocol<GetResourceLocatorByIdOp<TSupportedId>, IResourceLocator>(this.GetResourceLocatorByIdOpFunc);

            return result;
        }
    }
}
