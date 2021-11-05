// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISpecifyResourceLocator.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Interface to expose a specified <see cref="IResourceLocator"/> for the operation.
    /// </summary>
    public interface ISpecifyResourceLocator
    {
        /// <summary>
        /// Gets the specified resource locator.
        /// </summary>
        IResourceLocator SpecifiedResourceLocator { get; }
    }
}
