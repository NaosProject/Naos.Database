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
        /// <remarks>
        /// This enables the stream to be passed thru process boundaries.
        /// The stream provides a representation of itself, which can be serialized
        /// and then later deserialized and available to execute operations against.
        /// </remarks>
        IStreamRepresentation StreamRepresentation { get; }
    }
}
