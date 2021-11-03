// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHaveInternalRecordId.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Interface to expose a <see cref="StreamRecord"/> internal record identifier.
    /// </summary>
    public interface IHaveInternalRecordId
    {
        /// <summary>
        /// Gets the internal record id.
        /// </summary>
        long InternalRecordId { get; }
    }
}