// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandlingOnlyStreamExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Extension methods on <see cref="IRecordHandlingOnlyStream"/>.
    /// </summary>
    public static partial class HandlingOnlyStreamExtensions
    {
        /// <summary>
        /// Gets a value that indicates whether or not record handling is disabled on the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// true if record handling is disabled on the stream; otherwise false.
        /// </returns>
        public static bool IsRecordHandlingDisabled(
            this IRecordHandlingOnlyStream stream)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var operation = new GetHandlingStatusOp(Concerns.GlobalBlockingRecordId, Concerns.StreamHandlingDisabledConcern);

            var protocol = stream.GetStreamRecordHandlingProtocols();

            var handlingStatus = protocol.Execute(operation);

            var result = handlingStatus == HandlingStatus.DisabledForStream;

            return result;
        }
    }
}
