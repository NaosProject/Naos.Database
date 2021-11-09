// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetStreamFromRepresentationByNameProtocolFactory.cs" company="Naos Project">
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
    /// Stream factory to get an <see cref="IReadWriteStream"/> or <see cref="IReadOnlyStream"/> from a <see cref="StreamRepresentation"/>.
    /// </summary>
    public class GetStreamFromRepresentationByNameProtocolFactory :
        SyncSpecificReturningProtocolBase<GetStreamFromRepresentationOp, IStream>
    {
        private readonly IReadOnlyDictionary<string, Func<IStream>> streamNameToStreamMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetStreamFromRepresentationByNameProtocolFactory"/> class.
        /// </summary>
        /// <param name="streamNameToStreamMap">The stream name to stream map.</param>
        public GetStreamFromRepresentationByNameProtocolFactory(
            IReadOnlyDictionary<string, Func<IStream>> streamNameToStreamMap)
        {
            streamNameToStreamMap.MustForArg(nameof(streamNameToStreamMap)).NotBeNull();

            this.streamNameToStreamMap = streamNameToStreamMap;
        }

        /// <inheritdoc />
        public override IStream Execute(
            GetStreamFromRepresentationOp operation)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            var exists = this.streamNameToStreamMap.TryGetValue(operation.StreamRepresentation.Name, out var result);

            if (exists)
            {
                return result();
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Gets the stream from representation protocol.
        /// </summary>
        /// <typeparam name="TStreamRepresentation">The type of the <see cref="IStreamRepresentation"/>.</typeparam>
        /// <typeparam name="TStream">The type of the <see cref="IStream"/>.</typeparam>
        /// <returns>Protocol to get an <see cref="IStream"/> from a <see cref="IStreamRepresentation"/>.</returns>
        public ISyncReturningProtocol<GetStreamFromRepresentationOp<TStreamRepresentation, TStream>, TStream> GetStreamFromRepresentationProtocol<TStreamRepresentation, TStream>()
            where TStreamRepresentation : IStreamRepresentation
            where TStream : IStream
        {
            return new LambdaReturningProtocol<GetStreamFromRepresentationOp<TStreamRepresentation, TStream>, TStream>(
                operation =>
                {
                    operation.MustForArg(nameof(operation)).NotBeNull();

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
