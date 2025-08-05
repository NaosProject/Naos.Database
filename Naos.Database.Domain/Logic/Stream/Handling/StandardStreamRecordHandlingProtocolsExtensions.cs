// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamRecordHandlingProtocolsExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Linq;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Extension methods on <see cref="IStandardStreamRecordHandlingProtocols"/>.
    /// </summary>
    public static partial class StandardStreamRecordHandlingProtocolsExtensions
    {
        /// <summary>
        /// Gets a value that indicates whether or not record handling is disabled on the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// true if record handling is disabled on the stream; otherwise false.
        /// </returns>
        public static bool IsRecordHandlingDisabled(
            this IStandardStreamRecordHandlingProtocols stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new StandardGetHandlingStatusOp(
                Concerns.StreamHandlingDisabledConcern,
                new RecordFilter(internalRecordIds: new[] { Concerns.GlobalBlockingRecordId }),
                new HandlingFilter());

            var handlingStatus = stream.Execute(operation);

            var result = handlingStatus.Any() && (handlingStatus.Single().Value == HandlingStatus.DisabledForStream);

            return result;
        }
    }
}
