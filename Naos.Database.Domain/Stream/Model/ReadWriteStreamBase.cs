// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadWriteStreamBase.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.CodeAnalysis.Recipes;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Stream interface, a stream is a list of objects ordered by timestamp.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public abstract class ReadWriteStreamBase : IReadWriteStream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadWriteStreamBase"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="resourceLocatorProtocols">Protocol to get appropriate resource locator(s).</param>
        protected ReadWriteStreamBase(
            string name,
            IResourceLocatorProtocols resourceLocatorProtocols)
        {
            name.MustForArg(nameof(name)).NotBeNullNorWhiteSpace();
            resourceLocatorProtocols.MustForArg(nameof(resourceLocatorProtocols)).NotBeNull();

            this.Name = name;
            this.ResourceLocatorProtocols = resourceLocatorProtocols;
        }

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public IResourceLocatorProtocols ResourceLocatorProtocols { get; private set; }

        /// <inheritdoc />
        public abstract IStreamRepresentation StreamRepresentation { get; }

        /// <inheritdoc />
        public abstract IStreamReadProtocols GetStreamReadingProtocols();

        /// <inheritdoc />
        public abstract IStreamReadProtocols<TObject> GetStreamReadingProtocols<TObject>();

        /// <inheritdoc />
        public abstract IStreamReadWithIdProtocols<TId> GetStreamReadingWithIdProtocols<TId>();

        /// <inheritdoc />
        public abstract IStreamReadWithIdProtocols<TId, TObject> GetStreamReadingWithIdProtocols<TId, TObject>();

        /// <inheritdoc />
        public abstract IStreamWriteProtocols GetStreamWritingProtocols();

        /// <inheritdoc />
        public abstract IStreamWriteProtocols<TObject> GetStreamWritingProtocols<TObject>();

        /// <inheritdoc />
        public abstract IStreamWriteWithIdProtocols<TId> GetStreamWritingWithIdProtocols<TId>();

        /// <inheritdoc />
        public abstract IStreamWriteWithIdProtocols<TId, TObject> GetStreamWritingWithIdProtocols<TId, TObject>();
    }
}
