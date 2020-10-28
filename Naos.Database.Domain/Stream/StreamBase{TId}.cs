// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamBase{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Stream interface, a stream is a list of objects ordered by timestamp.
    /// </summary>
    /// <typeparam name="TId">The type of ID of the stream.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public abstract class StreamBase<TId> : IStream<TId>
    {
        private readonly ISyncAndAsyncReturningProtocol<GetProtocolByTypeOp, IProtocol> protocolGetIdByTypeProtocol;
        private readonly ISyncAndAsyncReturningProtocol<GetProtocolByTypeOp, IProtocol> protocolGetTagsByTypeProtocol;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamBase{TId}"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="resourceLocatorProtocol">Protocol to get appropriate resource locator(s).</param>
        /// <param name="protocolGetIdByTypeProtocol">Optional, id extractor protocols by type; DEFAULT will look for implementation of <see cref="IIdentifiable"/> on the object written.</param>
        /// <param name="protocolGetTagsByTypeProtocol">Optional tag extractor protocols by type; DEFAULT will look for implementation of <see cref="IHaveTags"/> on the object written.</param>
        protected StreamBase(
            string name,
            IProtocolResourceLocator<TId> resourceLocatorProtocol,
            ISyncAndAsyncReturningProtocol<GetProtocolByTypeOp, IProtocol> protocolGetIdByTypeProtocol = null,
            ISyncAndAsyncReturningProtocol<GetProtocolByTypeOp, IProtocol> protocolGetTagsByTypeProtocol = null)
        {
            name.MustForArg(nameof(name)).NotBeNullNorWhiteSpace();
            resourceLocatorProtocol.MustForArg(nameof(resourceLocatorProtocol)).NotBeNull();

            this.Name = name;
            this.ResourceLocatorProtocol = resourceLocatorProtocol;
            this.protocolGetIdByTypeProtocol = protocolGetIdByTypeProtocol;
            this.protocolGetTagsByTypeProtocol = protocolGetTagsByTypeProtocol;
        }

        /// <inheritdoc />
        public ISyncAndAsyncReturningProtocol<GetIdFromObjectOp<TId, TObject>, TId> BuildGetIdFromObjectProtocol<TObject>()
        {
            ISyncAndAsyncReturningProtocol<GetIdFromObjectOp<TId, TObject>, TId> result = null;

            if (this.protocolGetIdByTypeProtocol != null)
            {
                var operation = new GetProtocolByTypeOp(
                    typeof(ISyncAndAsyncReturningProtocol<GetIdFromObjectOp<TId, TObject>, TId>).ToRepresentation(),
                    MissingProtocolStrategy.Null);
                var protocol = this.protocolGetIdByTypeProtocol.Execute(operation);
                result = (ISyncAndAsyncReturningProtocol<GetIdFromObjectOp<TId, TObject>, TId>)protocol;
            }

            if (result == null)
            {
                result = new GetIdFromObjectProtocol<TId, TObject>();
            }

            return result;
        }

        /// <inheritdoc />
        public ISyncAndAsyncReturningProtocol<GetTagsFromObjectOp<TObject>, IReadOnlyDictionary<string, string>> BuildGetTagsFromObjectProtocol<TObject>()
        {
            ISyncAndAsyncReturningProtocol<GetTagsFromObjectOp<TObject>, IReadOnlyDictionary<string, string>> result = null;
            if (this.protocolGetTagsByTypeProtocol != null)
            {
                var operation = new GetProtocolByTypeOp(
                    typeof(ISyncAndAsyncReturningProtocol<GetTagsFromObjectOp<TObject>, IReadOnlyDictionary<string, string>>).ToRepresentation(),
                    MissingProtocolStrategy.Null);
                var protocol = this.protocolGetTagsByTypeProtocol.Execute(operation);
                result = (ISyncAndAsyncReturningProtocol<GetTagsFromObjectOp<TObject>, IReadOnlyDictionary<string, string>>)protocol;
            }

            if (result == null)
            {
                result = new GetTagsFromObjectProtocol<TObject>();
            }

            return result;
        }

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public IProtocolResourceLocator<TId> ResourceLocatorProtocol { get; private set; }

        /// <inheritdoc />
        public abstract IStreamRepresentation<TId> StreamRepresentation { get; }

        /// <inheritdoc />
        public abstract void Execute(CreateStreamOp<TId> operation);

        /// <inheritdoc />
        public abstract Task ExecuteAsync(CreateStreamOp<TId> operation);

        /// <inheritdoc />
        public abstract void Execute(DeleteStreamOp<TId> operation);

        /// <inheritdoc />
        public abstract Task ExecuteAsync(DeleteStreamOp<TId> operation);

        /// <inheritdoc />
        public abstract ISyncAndAsyncReturningProtocol<GetLatestByIdAndTypeOp<TId, TObject>, TObject> BuildGetLatestByIdAndTypeProtocol<TObject>();

        /// <inheritdoc />
        public abstract ISyncAndAsyncVoidProtocol<PutOp<TObject>> BuildPutProtocol<TObject>();
    }
}
