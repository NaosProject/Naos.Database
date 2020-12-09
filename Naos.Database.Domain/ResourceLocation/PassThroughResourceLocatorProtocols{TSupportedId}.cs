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
    using Naos.Protocol.Domain;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Generic <see cref="IResourceLocatorProtocols"/> implementation to easily contain sharding logic.
    /// </summary>
    /// <typeparam name="TSupportedId">Supported identifier type.</typeparam>
    public class PassThroughResourceLocatorProtocols<TSupportedId>
        : IResourceLocatorProtocols
    {
        private readonly VersionlessTypeEqualityComparer versionlessComparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PassThroughResourceLocatorProtocols{TSupportedId}"/> class.
        /// </summary>
        /// <param name="allLocators">All locators.</param>
        /// <param name="resourceLocatorForUniqueIdentifier">The resource locator for unique identifiers.</param>
        /// <param name="resourceLocatorByIdProtocol">The resource locator by identifier protocol.</param>
        public PassThroughResourceLocatorProtocols(
            IReadOnlyCollection<IResourceLocator> allLocators,
            IResourceLocator resourceLocatorForUniqueIdentifier,
            Func<GetResourceLocatorByIdOp<TSupportedId>, IResourceLocator> resourceLocatorByIdProtocol)
        {
            this.versionlessComparer = new VersionlessTypeEqualityComparer();
            this.AllLocators = allLocators;
            this.ResourceLocatorForUniqueIdentifier = resourceLocatorForUniqueIdentifier;
            this.ResourceLocatorByIdProtocol = resourceLocatorByIdProtocol;
        }

        /// <summary>
        /// Gets all locators.
        /// </summary>
        /// <value>All locators.</value>
        public IReadOnlyCollection<IResourceLocator> AllLocators { get; private set; }

        /// <summary>
        /// Gets the resource locator for unique identifiers.
        /// </summary>
        /// <value>The resource locator for unique identifiers.</value>
        public IResourceLocator ResourceLocatorForUniqueIdentifier { get; private set; }

        /// <summary>
        /// Gets the resource locator by identifier protocol.
        /// </summary>
        /// <value>The resource locator by identifier protocol.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "It is in fact the backing logic of the method.")]
        public Func<GetResourceLocatorByIdOp<TSupportedId>, IResourceLocator> ResourceLocatorByIdProtocol { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<IResourceLocator> Execute(GetAllResourceLocatorsOp operation) => this.AllLocators;

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<IResourceLocator>> ExecuteAsync(
            GetAllResourceLocatorsOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public IResourceLocator Execute(GetResourceLocatorForUniqueIdentifierOp operation) => this.ResourceLocatorForUniqueIdentifier;

        /// <inheritdoc />
        public async Task<IResourceLocator> ExecuteAsync(
            GetResourceLocatorForUniqueIdentifierOp operation)
        {
            var syncResult = this.Execute(operation);
            var result = await Task.FromResult(syncResult);
            return result;
        }

        /// <inheritdoc />
        public ISyncAndAsyncReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator> GetResourceLocatorByIdProtocol<TId>()
        {
            if (!this.versionlessComparer.Equals(typeof(TId), typeof(TSupportedId)))
            {
                throw new ArgumentException(Invariant($"Only {typeof(TSupportedId).ToStringReadable()} is supported (not provided {typeof(TId).ToStringReadable()})."));
            }

            // ReSharper disable once SuspiciousTypeConversion.Global - this has been confirmed to be the same type and thus will cast fine...
            return (ISyncAndAsyncReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator>)new LambdaReturningProtocol<GetResourceLocatorByIdOp<TSupportedId>, IResourceLocator>(this.ResourceLocatorByIdProtocol);
        }
    }
}
