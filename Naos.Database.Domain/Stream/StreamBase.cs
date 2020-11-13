// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamBase.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

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
        /// <param name="resourceLocatorProtocol">Protocol to get appropriate resource locator(s).</param>
        protected StreamBase(
            string name,
            IProtocolResourceLocator resourceLocatorProtocol)
        {
            name.MustForArg(nameof(name)).NotBeNullNorWhiteSpace();
            resourceLocatorProtocol.MustForArg(nameof(resourceLocatorProtocol)).NotBeNull();

            this.Name = name;
            this.ResourceLocatorProtocol = resourceLocatorProtocol;
        }

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public IProtocolResourceLocator ResourceLocatorProtocol { get; private set; }

        /// <inheritdoc />
        public abstract IStreamRepresentation StreamRepresentation { get; }

        /// <inheritdoc />
        public abstract void Execute(CreateStreamOp operation);

        /// <inheritdoc />
        public abstract Task ExecuteAsync(CreateStreamOp operation);

        /// <inheritdoc />
        public abstract void Execute(DeleteStreamOp operation);

        /// <inheritdoc />
        public abstract Task ExecuteAsync(DeleteStreamOp operation);

        /// <inheritdoc />
        public abstract IProtocolStreamObjectReadOperations<TId, TObject> GetObjectReadOperationsProtocol<TId, TObject>();

        /// <inheritdoc />
        public abstract IProtocolStreamObjectReadOperations<TObject> GetObjectReadOperationsProtocol<TObject>();

        /// <inheritdoc />
        public abstract IProtocolStreamObjectWriteOperations<TId, TObject> GetObjectWriteOperationsProtocol<TId, TObject>();

        /// <inheritdoc />
        public abstract IProtocolStreamObjectWriteOperations<TObject> GetObjectWriteOperationsProtocol<TObject>();

        /// <inheritdoc />
        public abstract long Execute(GetNextUniqueLongOp operation);

        /// <inheritdoc />
        public abstract Task<long> ExecuteAsync(GetNextUniqueLongOp operation);
    }
}
