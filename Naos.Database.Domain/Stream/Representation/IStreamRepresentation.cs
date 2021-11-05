// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamRepresentation.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Stream description to allow the <see cref="GetStreamFromRepresentationByNameProtocolFactory"/> to produce the correct stream.
    /// </summary>
    public interface IStreamRepresentation
    {
        /// <summary>
        /// Gets the name of the stream.
        /// </summary>
        string Name { get; }
    }
}
