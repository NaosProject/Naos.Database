// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetStreamFromRepresentationByNameProtocol.cs" company="Naos Project">
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
    /// Executes a <see cref="GetStreamFromRepresentationOp"/>, using a map of stream name to a stream builder.
    /// </summary>
    public class GetStreamFromRepresentationByNameProtocol :
        SyncSpecificReturningProtocolBase<GetStreamFromRepresentationOp, IStream>
    {
        private readonly IReadOnlyDictionary<string, Func<IStream>> streamNameToStreamMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetStreamFromRepresentationByNameProtocol"/> class.
        /// </summary>
        /// <param name="streamNameToStreamMap">The stream name to stream map.</param>
        public GetStreamFromRepresentationByNameProtocol(
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
    }
}
