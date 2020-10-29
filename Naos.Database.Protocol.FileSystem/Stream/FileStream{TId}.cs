﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileStream{TId}.cs" company="Naos Project">
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
    /// File system implementation of <see cref="IStream{TId}"/>.
    /// Implements the <see cref="StreamBase{TId}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    /// <seealso cref="StreamBase{TId}" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public partial class FileStream<TId> : StreamBase<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileStream{TId}"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="defaultSerializerRepresentation">Default serializer description to use.</param>
        /// <param name="defaultSerializationFormat">Default serializer format.</param>
        /// <param name="serializerFactory">The factory to get a serializer to use for objects.</param>
        /// <param name="resourceLocatorProtocol">The protocols for getting locators.</param>
        /// <param name="protocolGetIdByTypeProtocol">Id extractor protocols by type.</param>
        /// <param name="protocolGetTagsByTypeProtocol">Tag extractor protocols by type.</param>
        public FileStream(
            string name,
            SerializerRepresentation defaultSerializerRepresentation,
            SerializationFormat defaultSerializationFormat,
            ISerializerFactory serializerFactory,
            IProtocolResourceLocator<TId> resourceLocatorProtocol,
            ISyncAndAsyncReturningProtocol<GetProtocolByTypeOp, IProtocol> protocolGetIdByTypeProtocol = null,
            ISyncAndAsyncReturningProtocol<GetProtocolByTypeOp, IProtocol> protocolGetTagsByTypeProtocol = null)
        : base(name, resourceLocatorProtocol, protocolGetIdByTypeProtocol, protocolGetTagsByTypeProtocol)
        {
            name.MustForArg(nameof(name)).NotBeNullNorWhiteSpace();
            defaultSerializerRepresentation.MustForArg(nameof(defaultSerializerRepresentation)).NotBeNull();
            serializerFactory.MustForArg(nameof(serializerFactory)).NotBeNull();
            resourceLocatorProtocol.MustForArg(nameof(resourceLocatorProtocol)).NotBeNull();

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
        public override IStreamRepresentation<TId> StreamRepresentation
        {
            get
            {
                var allFileSystemDatabaseLocators =
                    this.ResourceLocatorProtocol
                        .Execute(new GetAllResourceLocatorsOp())
                        .Cast<FileSystemDatabaseLocator>()
                        .ToList();

                var result = new FileStreamRepresentation<TId>(this.Name, allFileSystemDatabaseLocators);
                return result;
            }
        }
    }
}