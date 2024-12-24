// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordedStreamOpExecutionBase.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Base class for an object that records the execution of a stream operation.
    /// </summary>
    public abstract class RecordedStreamOpExecutionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordedStreamOpExecutionBase"/> class.
        /// </summary>
        /// <param name="preExecutionTimestampUtc">The timestamp, in UTC, prior to the execution of the operation.</param>
        protected RecordedStreamOpExecutionBase(
            DateTime preExecutionTimestampUtc)
        {
            preExecutionTimestampUtc.MustForArg(nameof(preExecutionTimestampUtc)).BeUtcDateTime();

            this.PreExecutionTimestampUtc = preExecutionTimestampUtc;
        }

        /// <summary>
        /// Gets the timestamp, in UTC, prior to the execution of the operation.
        /// </summary>
        public DateTime PreExecutionTimestampUtc { get; }

        /// <summary>
        /// Gets the timestamp, in UTC, after the operation was executed, or
        /// null if executed the operation threw an exception.
        /// </summary>
        public DateTime? PostExecutionTimestampUtc { get; private set; }

        /// <summary>
        /// Gets the executed operation interface.
        /// </summary>
        public abstract IOperation OperationInterface { get; }

        /// <summary>
        /// Records the post-execution timestamp using the time right now.
        /// </summary>
        public void RecordTimestampPostExecution()
        {
            this.RecordTimestampPostExecution(DateTime.UtcNow);
        }

        /// <summary>
        /// Records the post-execution timestamp.
        /// </summary>
        /// <param name="postExecutionTimestampUtc">The UTC timestamp.</param>
        public void RecordTimestampPostExecution(
            DateTime postExecutionTimestampUtc)
        {
            postExecutionTimestampUtc.MustForArg(nameof(postExecutionTimestampUtc)).BeUtcDateTime();
            postExecutionTimestampUtc.MustForArg(nameof(postExecutionTimestampUtc)).BeGreaterThanOrEqualTo(this.PreExecutionTimestampUtc);

            this.PostExecutionTimestampUtc.MustForOp(nameof(this.PostExecutionTimestampUtc)).BeNull();

            this.PostExecutionTimestampUtc = postExecutionTimestampUtc;
        }
    }
}