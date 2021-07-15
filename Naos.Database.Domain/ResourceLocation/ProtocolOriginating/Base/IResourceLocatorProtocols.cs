// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResourceLocatorProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Type;

    /// <summary>
    /// Set of common protocols around <see cref="IResourceLocator" /> for use with protocols accessing communication or storage.
    /// </summary>
    public interface IResourceLocatorProtocols
        : ISyncAndAsyncReturningProtocol<GetAllResourceLocatorsOp, IReadOnlyCollection<IResourceLocator>>,
          ISyncAndAsyncReturningProtocol<GetResourceLocatorForUniqueIdentifierOp, IResourceLocator>
    {
        /// <summary>
        /// Gets the resource locator by identifier protocol.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <returns>Protocol for <see cref="GetResourceLocatorByIdProtocol{TId}"/>.</returns>
        ISyncAndAsyncReturningProtocol<GetResourceLocatorByIdOp<TId>, IResourceLocator> GetResourceLocatorByIdProtocol<TId>();
    }
}
