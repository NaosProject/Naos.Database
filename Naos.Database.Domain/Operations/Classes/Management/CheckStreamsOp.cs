// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckStreamsOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Operation to check streams.
    /// </summary>
    public partial class CheckStreamsOp : ReturningOperationBase<CheckStreamsReport>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckStreamsOp"/> class.
        /// </summary>
        /// <param name="streamNameToCheckStreamInstructionsMap">The <see cref="IStreamRepresentation" /> to <see cref="CheckStreamInstruction" /> map.</param>
        public CheckStreamsOp(
            IReadOnlyDictionary<string, CheckStreamInstruction> streamNameToCheckStreamInstructionsMap)
        {
            streamNameToCheckStreamInstructionsMap.MustForArg(nameof(streamNameToCheckStreamInstructionsMap)).NotBeNull().And().NotContainAnyKeyValuePairsWithNullValue();

            this.StreamNameToCheckStreamInstructionsMap = streamNameToCheckStreamInstructionsMap;
        }

        /// <summary>
        /// Gets the stream name to <see cref="CheckStreamInstruction" /> map.
        /// </summary>
        public IReadOnlyDictionary<string, CheckStreamInstruction> StreamNameToCheckStreamInstructionsMap { get; private set; }
    }
}
