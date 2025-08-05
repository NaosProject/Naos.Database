// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamConfig.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// Generic implementation of <see cref="StandardStreamConfigBase"/>.
    /// </summary>
    public partial class StandardStreamConfig : StandardStreamConfigBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardStreamConfig"/> class.
        /// </summary>
        /// <param name="name">Name of the stream.</param>
        /// <param name="accessKinds">The kind of access that the stream has..</param>
        /// <param name="defaultSerializerRepresentation">The serializer representation to use to get a serializer to use when serializing objects (not identifiers) into record payloads to put.</param>
        /// <param name="defaultSerializationFormat">The serialization format to use when serializing objects (not identifiers) into record payloads to put.</param>
        /// <param name="allLocators">All <see cref="IResourceLocator"/>'s.</param>
        public StandardStreamConfig(
            string name,
            StreamAccessKinds accessKinds,
            SerializerRepresentation defaultSerializerRepresentation,
            SerializationFormat defaultSerializationFormat,
            IReadOnlyCollection<IResourceLocator> allLocators)
        : base(name, accessKinds, defaultSerializerRepresentation, defaultSerializationFormat, allLocators)
        {
        }
    }
}
