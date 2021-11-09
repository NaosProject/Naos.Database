// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamRepresentation.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// A representation of a <see cref="IStream"/> that can be serialized without knowledge of the stream.
    /// </summary>
    public interface IStreamRepresentation
    {
        /// <summary>
        /// Gets the name of the stream.
        /// </summary>
        string Name { get; }
    }
}
