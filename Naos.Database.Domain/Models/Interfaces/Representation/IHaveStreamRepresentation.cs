// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHaveStreamRepresentation.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Interface to expose a <see cref="IStreamRepresentation"/>.
    /// </summary>
    public interface IHaveStreamRepresentation
    {
        /// <summary>
        /// Gets the stream representation.
        /// </summary>
        IStreamRepresentation StreamRepresentation { get; }
    }
}
