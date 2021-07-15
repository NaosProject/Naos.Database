// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandleRecordWithIdOp{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Handles a record.
    /// </summary>
    /// <typeparam name="TId">Type of the identifier of the record.</typeparam>
    public partial class HandleRecordWithIdOp<TId> : VoidOperationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandleRecordWithIdOp{TId}"/> class.
        /// </summary>
        /// <param name="recordToHandle">The record to handle.</param>
        public HandleRecordWithIdOp(
            StreamRecordWithId<TId> recordToHandle)
        {
            recordToHandle.MustForArg(nameof(recordToHandle)).NotBeNull();

            this.RecordToHandle = recordToHandle;
        }

        /// <summary>
        /// Gets the record to handle.
        /// </summary>
        /// <value>The record to handle.</value>
        public StreamRecordWithId<TId> RecordToHandle { get; private set; }
    }
}
