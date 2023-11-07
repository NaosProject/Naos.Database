// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordExpectedToBeHandledReport.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using Naos.Diagnostics.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Report returned from <see cref="Domain.RecordExpectedToBeHandled" /> evaluation.
    /// </summary>
    public partial class RecordExpectedToBeHandledReport : IHaveCheckStatus, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordExpectedToBeHandledReport"/> class.
        /// </summary>
        /// <param name="status">Evaluated check status.</param>
        /// <param name="recordExpectedToBeHandled">Instructions used to evaluate.</param>
        /// <param name="internalRecordIdToHandlingStatusMap">The map of violating internal record identifiers to handling status.</param>
        public RecordExpectedToBeHandledReport(
            CheckStatus status,
            RecordExpectedToBeHandled recordExpectedToBeHandled,
            IReadOnlyDictionary<long, HandlingStatus> internalRecordIdToHandlingStatusMap)
        {
            status.MustForArg(nameof(status)).NotBeEqualTo(CheckStatus.Invalid);
            recordExpectedToBeHandled.MustForArg(nameof(recordExpectedToBeHandled)).NotBeNull();
            internalRecordIdToHandlingStatusMap.MustForArg(nameof(internalRecordIdToHandlingStatusMap)).NotBeNull();

            this.Status = status;
            this.RecordExpectedToBeHandled = recordExpectedToBeHandled;
            this.InternalRecordIdToHandlingStatusMap = internalRecordIdToHandlingStatusMap;
        }

        /// <inheritdoc />
        public CheckStatus Status { get; private set; }

        /// <summary>
        /// Gets the instructions used to evaluate.
        /// </summary>
        public RecordExpectedToBeHandled RecordExpectedToBeHandled { get; private set; }

        /// <summary>
        /// Gets the map of violating internal record identifiers to handling status.
        /// </summary>
        public IReadOnlyDictionary<long, HandlingStatus> InternalRecordIdToHandlingStatusMap { get; private set; }
    }
}
