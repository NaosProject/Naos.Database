// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamConfigBase.cs" company="Naos Project">
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
    /// Base class for <see cref="IStandardStreamConfig"/>.
    /// </summary>
    public abstract partial class StandardStreamConfigBase : IStandardStreamConfig, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardStreamConfigBase"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="accessKinds">The kind of access that the stream has.</param>
        /// <param name="defaultSerializerRepresentation">The serializer representation to use to get a serializer to use when serializing objects (not identifiers) into record payloads to put.</param>
        /// <param name="defaultSerializationFormat">The serialization format to use when serializing objects (not identifiers) into record payloads to put.</param>
        /// <param name="allLocators">All <see cref="IResourceLocator"/>'s.</param>
        protected StandardStreamConfigBase(
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

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public StreamAccessKinds AccessKinds { get; private set; }

        /// <inheritdoc />
        public SerializerRepresentation DefaultSerializerRepresentation { get; private set; }

        /// <inheritdoc />
        public SerializationFormat DefaultSerializationFormat { get; private set; }

        /// <inheritdoc />
        public IReadOnlyCollection<IResourceLocator> AllLocators { get; private set; }
    }
}
