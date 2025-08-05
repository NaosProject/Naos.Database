// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetStreamFromRepresentationByNameProtocol{TStreamRepresentation,TStream}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Executes a <see cref="GetStreamFromRepresentationOp{TStreamRepresentation, TStream}"/>,
    /// using a map of stream name to a stream builder.
    /// </summary>
    /// <typeparam name="TStreamRepresentation">Type of <see cref="IStreamRepresentation"/> to use.</typeparam>
    /// <typeparam name="TStream">Type of <see cref="IStream"/> to get.</typeparam>
    public class GetStreamFromRepresentationByNameProtocol<TStreamRepresentation, TStream> :
        SyncSpecificReturningProtocolBase<GetStreamFromRepresentationOp<TStreamRepresentation, TStream>, TStream>
        where TStreamRepresentation : IStreamRepresentation
        where TStream : IStream
    {
        private readonly GetStreamFromRepresentationByNameProtocol<TStream> backingProtocol;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetStreamFromRepresentationByNameProtocol{TStreamRepresentation, TStream}"/> class.
        /// </summary>
        /// <param name="streamNameToStreamMap">The stream name to stream map.</param>
        public GetStreamFromRepresentationByNameProtocol(
            IReadOnlyDictionary<string, Func<IStream>> streamNameToStreamMap)
        {
            streamNameToStreamMap.MustForArg(nameof(streamNameToStreamMap)).NotBeNull();

            this.backingProtocol = new GetStreamFromRepresentationByNameProtocol<TStream>(streamNameToStreamMap);
        }

        /// <inheritdoc />
        public override TStream Execute(
            GetStreamFromRepresentationOp<TStreamRepresentation, TStream> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var delegatedOperation = new GetStreamFromRepresentationOp<TStream>(operation.StreamRepresentation);

            var result = this.backingProtocol.Execute(delegatedOperation);

            return result;
        }
    }
}
