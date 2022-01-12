// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRecordLocator.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Interface to expose necessary items to locate a specific record in a stream.
    /// </summary>
    public interface IRecordLocator
    {
        /// <summary>
        /// Gets the resource locator that the <see cref="InternalRecordId"/> is located at.
        /// </summary>
        IResourceLocator ResourceLocator { get; }

        /// <summary>
        /// Gets the internal record identifier in the stream specified by <see cref="ResourceLocator"/>.
        /// </summary>
        long InternalRecordId { get; }
    }
}
