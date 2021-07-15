// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandleRecordOp.cs" company="Naos Project">
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
    public partial class HandleRecordOp : VoidOperationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandleRecordOp"/> class.
        /// </summary>
        /// <param name="recordToHandle">The record to handle.</param>
        public HandleRecordOp(
            StreamRecord recordToHandle)
        {
            recordToHandle.MustForArg(nameof(recordToHandle)).NotBeNull();

            this.RecordToHandle = recordToHandle;
        }

        /// <summary>
        /// Gets the record to handle.
        /// </summary>
        /// <value>The record to handle.</value>
        public StreamRecord RecordToHandle { get; private set; }
    }
}
