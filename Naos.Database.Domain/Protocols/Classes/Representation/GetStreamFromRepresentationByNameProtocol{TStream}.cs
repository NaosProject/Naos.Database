// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetStreamFromRepresentationByNameProtocol{TStream}.cs" company="Naos Project">
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
    /// <typeparam name="TStream">Type of <see cref="IStream"/> to get.</typeparam>
    public class GetStreamFromRepresentationByNameProtocol<TStream> :
        SyncSpecificReturningProtocolBase<GetStreamFromRepresentationOp<TStream>, TStream>
        where TStream : IStream
    {
        private readonly GetStreamFromRepresentationByNameProtocol backingProtocol;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetStreamFromRepresentationByNameProtocol{TStream}"/> class.
        /// </summary>
        /// <param name="streamNameToStreamMap">The stream name to stream map.</param>
        public GetStreamFromRepresentationByNameProtocol(
            IReadOnlyDictionary<string, Func<IStream>> streamNameToStreamMap)
        {
            streamNameToStreamMap.MustForArg(nameof(streamNameToStreamMap)).NotBeNull();

            this.backingProtocol = new GetStreamFromRepresentationByNameProtocol(streamNameToStreamMap);
        }

        /// <inheritdoc />
        public override TStream Execute(
            GetStreamFromRepresentationOp<TStream> operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var delegatedOperation = new GetStreamFromRepresentationOp(operation.StreamRepresentation);

            var result = (TStream)this.backingProtocol.Execute(delegatedOperation);

            return result;
        }
    }
}
