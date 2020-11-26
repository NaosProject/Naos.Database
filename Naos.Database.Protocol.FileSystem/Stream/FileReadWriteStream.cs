// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileReadWriteStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Protocol.FileSystem
{
    using System;
    using System.IO;
    using System.Linq;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Database.Domain;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization;

    /// <summary>
    /// File system implementation of <see cref="IReadWriteStream"/>, it is thread resilient but not necessarily thread safe.
    /// Implements the <see cref="ReadWriteStreamBase" />.
    /// </summary>
    /// <seealso cref="ReadWriteStreamBase" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public partial class FileReadWriteStream : ReadWriteStreamBase, IStreamManagementProtocolFactory, IStreamEventHandlingProtocolFactory
    {
        private readonly object fileLock = new object();

        /// <summary>
        /// Gets the locking object for the specific <see cref="IStream"/>.
        /// </summary>
        /// <value>The file lock.</value>
        // ReSharper disable once ConvertToAutoProperty - prefer proper backing field for safety given it's used for locking
        public object FileLock => this.fileLock;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileReadWriteStream"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="defaultSerializerRepresentation">Default serializer description to use.</param>
        /// <param name="defaultSerializationFormat">Default serializer format.</param>
        /// <param name="serializerFactory">The factory to get a serializer to use for objects.</param>
        /// <param name="resourceLocatorProtocols">The protocols for getting locators.</param>
        public FileReadWriteStream(
            string name,
            SerializerRepresentation defaultSerializerRepresentation,
            SerializationFormat defaultSerializationFormat,
            ISerializerFactory serializerFactory,
            IResourceLocatorProtocols resourceLocatorProtocols)
        : base(name, resourceLocatorProtocols)
        {
            name.MustForArg(nameof(name)).NotBeNullNorWhiteSpace();
            defaultSerializerRepresentation.MustForArg(nameof(defaultSerializerRepresentation)).NotBeNull();
            serializerFactory.MustForArg(nameof(serializerFactory)).NotBeNull();
            resourceLocatorProtocols.MustForArg(nameof(resourceLocatorProtocols)).NotBeNull();

            this.DefaultSerializerRepresentation = defaultSerializerRepresentation;
            this.SerializerFactory = serializerFactory;
            this.DefaultSerializationFormat = defaultSerializationFormat;
        }

        /// <summary>
        /// Gets the default serializer description.
        /// </summary>
        /// <value>The default serializer description.</value>
        public SerializerRepresentation DefaultSerializerRepresentation { get; private set; }

        /// <summary>
        /// Gets the default serialization format.
        /// </summary>
        /// <value>The default serialization format.</value>
        public SerializationFormat DefaultSerializationFormat { get; private set; }

        /// <summary>
        /// Gets the serializer factory.
        /// </summary>
        /// <value>The serializer factory.</value>
        public ISerializerFactory SerializerFactory { get; private set; }

        /// <inheritdoc />
        public override IStreamRepresentation StreamRepresentation
        {
            get
            {
                var allFileSystemDatabaseLocators =
                    this.ResourceLocatorProtocols
                        .Execute(new GetAllResourceLocatorsOp())
                        .Cast<FileSystemDatabaseLocator>()
                        .ToList();

                var result = new FileStreamRepresentation(this.Name, allFileSystemDatabaseLocators);
                return result;
            }
        }

        /// <inheritdoc />
        public override IStreamReadProtocols GetStreamReadingProtocols() => new FileStreamReadWriteProtocols(this);

        /// <inheritdoc />
        public override IStreamReadProtocols<TObject> GetStreamReadingProtocols<TObject>() => new FileStreamReadWriteProtocols<TObject>(this);

        /// <inheritdoc />
        public override IStreamReadProtocols<TId, TObject> GetStreamReadingProtocols<TId, TObject>() => new FileStreamReadWriteProtocols<TId, TObject>(this);

        /// <inheritdoc />
        public override IStreamWriteProtocols GetStreamWritingProtocols() => new FileStreamReadWriteProtocols(this);

        /// <inheritdoc />
        public override IStreamWriteProtocols<TObject> GetStreamWritingProtocols<TObject>() => new FileStreamReadWriteProtocols<TObject>(this);

        /// <inheritdoc />
        public override IStreamWriteProtocols<TId, TObject> GetStreamWritingProtocols<TId, TObject>() => new FileStreamReadWriteProtocols<TId, TObject>(this);

        /// <inheritdoc />
        public IStreamManagementProtocols GetStreamManagementProtocols() => new FileStreamManagementProtocols(this);

        /// <inheritdoc />
        public IStreamEventHandlingProtocols<TEvent> GetStreamEventHandlingProtocols<TEvent>()
            where TEvent : IEvent
            => new FileStreamEventHandlingProtocols<TEvent>(this);
    }
}
