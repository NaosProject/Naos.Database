// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamFactory{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Protocol.Domain;

    /// <summary>
    /// Stream factory to get an <see cref="IStream{TId}"/> or <see cref="IReadOnlyStream{TId}"/> from a <see cref="StreamRepresentation{TId}"/>.
    /// </summary>
    /// <typeparam name="TId">The type of ID of the stream.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public class StreamFactory<TId> : IReturningProtocol<GetStreamFromRepresentationOp<TId>, IStream<TId>>
    {
        private readonly IReadOnlyDictionary<string, IStream<TId>> streamNameToStreamMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamFactory{TId}"/> class.
        /// </summary>
        /// <param name="streamNameToStreamMap">The stream name to stream map.</param>
        public StreamFactory(
            IReadOnlyDictionary<string, IStream<TId>> streamNameToStreamMap)
        {
            this.streamNameToStreamMap = streamNameToStreamMap ?? throw new ArgumentNullException(nameof(streamNameToStreamMap));
        }

        /// <inheritdoc />
        public IStream<TId> Execute(
            GetStreamFromRepresentationOp<TId> operation)
        {
            var exists = this.streamNameToStreamMap.TryGetValue(operation.StreamRepresentation.Name, out var result);
            if (exists)
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
