// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamConfig.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;
    using OBeautifulCode.Type;

    /// <summary>
    /// Base class for <see cref="IStreamConfig"/>.
    /// </summary>
    public partial class StreamConfig : StreamConfigBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamConfig"/> class.
        /// </summary>
        /// <param name="name">Name of the stream.</param>
        /// <param name="accessKinds">Access the stream has.</param>
        /// <param name="defaultSerializerRepresentation">Default <see cref="SerializerRepresentation"/> to use (used for identifier serialization).</param>
        /// <param name="defaultSerializationFormat">Default <see cref="SerializationFormat"/> to use.</param>
        /// <param name="allLocators">All <see cref="IResourceLocator"/>'s.</param>
        public StreamConfig(
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
