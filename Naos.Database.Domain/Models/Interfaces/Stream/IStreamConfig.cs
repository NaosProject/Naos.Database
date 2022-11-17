// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamConfig.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// Interface to expose necessary general items to create an implementation of <see cref="IStandardStream"/>.
    /// </summary>
    public interface IStreamConfig
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the access the stream has.
        /// </summary>
        StreamAccessKinds AccessKinds { get; }

        /// <summary>
        /// Gets the default <see cref="SerializerRepresentation"/> (used for identifier serialization).
        /// </summary>
        SerializerRepresentation DefaultSerializerRepresentation { get; }

        /// <summary>
        /// Gets the default <see cref="SerializationFormat"/>.
        /// </summary>
        SerializationFormat DefaultSerializationFormat { get; }

        /// <summary>
        /// Gets all <see cref="IResourceLocator"/>'s.
        /// </summary>
        IReadOnlyCollection<IResourceLocator> AllLocators { get; }
    }
}
