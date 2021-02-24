// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryHandleRecordResult.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Result of a <see cref="TryHandleRecordOp"/>.
    /// </summary>
    public partial class TryHandleRecordResult : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TryHandleRecordResult"/> class.
        /// </summary>
        /// <param name="recordToHandle">The record to handle.</param>
        /// <param name="isBlocked">An optional value indicating whether or not handling is blocked; DEFAULT is <c>false</c>.</param>
        public TryHandleRecordResult(
            StreamRecord recordToHandle,
            bool isBlocked = false)
        {
            this.RecordToHandle = recordToHandle;
            this.IsBlocked = isBlocked;
        }

        /// <summary>
        /// Gets the record to handle.
        /// </summary>
        /// <value>The record to handle.</value>
        public StreamRecord RecordToHandle { get; private set; }

        /// <summary>
        /// Gets a value indicating whether handling is blocked.
        /// </summary>
        /// <value><c>true</c> if handling is blocked; otherwise, <c>false</c>.</value>
        public bool IsBlocked { get; private set; }
    }
}
