// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamConfigBase.cs" company="Naos Project">
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
    public abstract partial class StreamConfigBase : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamConfigBase"/> class.
        /// </summary>
        /// <param name="name">Name of the stream.</param>
        /// <param name="accessKinds">Access the stream has.</param>
        /// <param name="defaultSerializerRepresentation">Default <see cref="SerializerRepresentation"/> to use (used for identifier serialization).</param>
        /// <param name="defaultSerializationFormat">Default <see cref="SerializationFormat"/> to use.</param>
        /// <param name="allLocators">All <see cref="IResourceLocator"/>'s.</param>
        protected StreamConfigBase(
            string name,
            StreamAccessKinds accessKinds,
            SerializerRepresentation defaultSerializerRepresentation,
            SerializationFormat defaultSerializationFormat,
            IReadOnlyCollection<IResourceLocator> allLocators)
        {
            name.MustForArg(nameof(name)).NotBeNullNorWhiteSpace();
            accessKinds.MustForArg(nameof(accessKinds)).NotBeEqualTo(StreamAccessKinds.None);
            defaultSerializerRepresentation.MustForArg(nameof(defaultSerializerRepresentation)).NotBeNull();
            defaultSerializationFormat.MustForArg(nameof(defaultSerializationFormat)).NotBeEqualTo(SerializationFormat.Invalid);
            allLocators.MustForArg(nameof(allLocators)).NotBeNullNorEmptyEnumerableNorContainAnyNulls();

            this.Name = name;
            this.AccessKinds = accessKinds;
            this.DefaultSerializerRepresentation = defaultSerializerRepresentation;
            this.DefaultSerializationFormat = defaultSerializationFormat;
            this.AllLocators = allLocators;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the access the stream has.
        /// </summary>
        public StreamAccessKinds AccessKinds { get; private set; }

        /// <summary>
        /// Gets the default <see cref="SerializerRepresentation"/> (used for identifier serialization).
        /// </summary>
        public SerializerRepresentation DefaultSerializerRepresentation { get; private set; }

        /// <summary>
        /// Gets the default <see cref="SerializationFormat"/>.
        /// </summary>
        public SerializationFormat DefaultSerializationFormat { get; private set; }

        /// <summary>
        /// Gets all <see cref="IResourceLocator"/>'s.
        /// </summary>
        public IReadOnlyCollection<IResourceLocator> AllLocators { get; private set; }
    }
}
