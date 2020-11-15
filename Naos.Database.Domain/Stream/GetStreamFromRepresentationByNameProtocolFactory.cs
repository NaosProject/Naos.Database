// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetStreamFromRepresentationByNameProtocolFactory.cs" company="Naos Project">
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
    /// Stream factory to get an <see cref="IReadWriteStream"/> or <see cref="IReadOnlyStream"/> from a <see cref="StreamRepresentation"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public class GetStreamFromRepresentationByNameProtocolFactory
    {
        private readonly IReadOnlyDictionary<string, Func<IReadWriteStream>> streamNameToStreamMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetStreamFromRepresentationByNameProtocolFactory"/> class.
        /// </summary>
        /// <param name="streamNameToStreamMap">The stream name to stream map.</param>
        public GetStreamFromRepresentationByNameProtocolFactory(
            IReadOnlyDictionary<string, Func<IReadWriteStream>> streamNameToStreamMap)
        {
            this.streamNameToStreamMap = streamNameToStreamMap ?? throw new ArgumentNullException(nameof(streamNameToStreamMap));
        }

        /// <summary>
        /// Gets the stream from representation protocol.
        /// </summary>
        /// <typeparam name="TStreamRepresentation">The type of the <see cref="IStreamRepresentation"/>.</typeparam>
        /// <typeparam name="TStream">The type of the <see cref="IStream"/>.</typeparam>
        /// <returns>Protocol to get an <see cref="IStream"/> from a <see cref="IStreamRepresentation"/>.</returns>
        public IReturningProtocol<GetStreamFromRepresentationOp<TStreamRepresentation, TStream>, TStream> GetStreamFromRepresentationProtocol<TStreamRepresentation, TStream>()
            where TStreamRepresentation : IStreamRepresentation
            where TStream : IStream
        {
            return new LambdaReturningProtocol<GetStreamFromRepresentationOp<TStreamRepresentation, TStream>, TStream>(synchronousLambda:
                operation =>
                {
                    var exists = this.streamNameToStreamMap.TryGetValue(operation.StreamRepresentation.Name, out var result);
                    if (exists)
                    {
                        return (TStream)result();
                    }
                    else
                    {
                        return default;
                    }
                });
        }
    }
}
