// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordedStreamOpExecution{TStreamOp}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// An object that records the execution of a stream operation.
    /// </summary>
    /// <typeparam name="TStreamOp">The type of stream operation.</typeparam>
    public class RecordedStreamOpExecution<TStreamOp> : RecordedStreamOpExecutionBase
        where TStreamOp : IOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordedStreamOpExecution{TStreamOp}"/> class.
        /// </summary>
        /// <param name="operation">The executed operation.</param>
        public RecordedStreamOpExecution(
            TStreamOp operation)
            : this(operation, DateTime.UtcNow)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordedStreamOpExecution{TStreamOp}"/> class.
        /// </summary>
        /// <param name="operation">The executed operation.</param>
        /// <param name="preExecutionTimestampUtc">The timestamp, in UTC, prior to the execution of the operation.</param>
        public RecordedStreamOpExecution(
            TStreamOp operation,
            DateTime preExecutionTimestampUtc)
            : base(preExecutionTimestampUtc)
        {
            operation.MustForArg(nameof(operation)).NotBeNull();

            this.Operation = operation;
        }

        /// <summary>
        /// Gets the executed operation.
        /// </summary>
        public TStreamOp Operation { get; }

        /// <inheritdoc />
        public override IOperation OperationInterface => this.Operation;
    }
}
