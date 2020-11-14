// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamBase.cs" company="Naos Project">
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
    public abstract class StreamBase : IStream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamBase"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="resourceLocatorProtocols">Protocol to get appropriate resource locator(s).</param>
        protected StreamBase(
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
        public abstract IStreamReadingProtocols GetStreamReadingProtocols();

        /// <inheritdoc />
        public abstract IStreamReadingProtocols<TObject> GetStreamReadingProtocols<TObject>();

        /// <inheritdoc />
        public abstract IStreamReadingProtocols<TId, TObject> GetStreamReadingProtocols<TId, TObject>();

        /// <inheritdoc />
        public abstract IStreamWritingProtocols GetStreamWritingProtocols();

        /// <inheritdoc />
        public abstract IStreamWritingProtocols<TObject> GetStreamWritingProtocols<TObject>();

        /// <inheritdoc />
        public abstract IStreamWritingProtocols<TId, TObject> GetStreamWritingProtocols<TId, TObject>();
    }
}
