// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStandardStreamConfig.cs" company="Naos Project">
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
    public interface IStandardStreamConfig
    {
        /// <summary>
        /// Gets the name of the stream.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the kind of access that the stream has.
        /// </summary>
        StreamAccessKinds AccessKinds { get; }

        /// <summary>
        /// Gets the serializer representation to use to get a serializer to use
        /// when serializing objects (not identifiers) into record payloads to put.
        /// </summary>
        SerializerRepresentation DefaultSerializerRepresentation { get; }

        /// <summary>
        /// Gets the serialization format to use
        /// when serializing objects (not identifiers) into record payloads to put.
        /// </summary>
        SerializationFormat DefaultSerializationFormat { get; }

        /// <summary>
        /// Gets all <see cref="IResourceLocator"/>'s.
        /// </summary>
        IReadOnlyCollection<IResourceLocator> AllLocators { get; }
    }
}
