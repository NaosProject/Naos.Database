// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandleRecordOp{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Try to handle a record of type <typeparamref name="TObject"/> for a specified concern.
    /// </summary>
    /// <typeparam name="TObject">Type of the object in the record.</typeparam>
    public partial class HandleRecordOp<TObject> : VoidOperationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandleRecordOp{TObject}"/> class.
        /// </summary>
        /// <param name="recordToHandle">The record to handle.</param>
        public HandleRecordOp(
            StreamRecord<TObject> recordToHandle)
        {
            recordToHandle.MustForArg(nameof(recordToHandle)).NotBeNull();

            this.RecordToHandle = recordToHandle;
        }

        /// <summary>
        /// Gets the record to handle.
        /// </summary>
        /// <value>The record to handle.</value>
        public StreamRecord<TObject> RecordToHandle { get; private set; }
    }
}
