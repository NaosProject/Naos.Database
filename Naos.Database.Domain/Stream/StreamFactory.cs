// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamFactory.cs" company="Naos Project">
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
    /// Stream factory to get an <see cref="IStream"/> or <see cref="IReadOnlyStream"/> from a <see cref="StreamRepresentation"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public class StreamFactory : IReturningProtocol<GetStreamFromRepresentationOp, IStream>
    {
        private readonly IReadOnlyDictionary<string, IStream> streamNameToStreamMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamFactory"/> class.
        /// </summary>
        /// <param name="streamNameToStreamMap">The stream name to stream map.</param>
        public StreamFactory(
            IReadOnlyDictionary<string, IStream> streamNameToStreamMap)
        {
            this.streamNameToStreamMap = streamNameToStreamMap ?? throw new ArgumentNullException(nameof(streamNameToStreamMap));
        }

        /// <inheritdoc />
        public IStream Execute(
            GetStreamFromRepresentationOp operation)
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
