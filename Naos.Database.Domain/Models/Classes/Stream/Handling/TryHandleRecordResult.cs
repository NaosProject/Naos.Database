// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryHandleRecordResult.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// The result of executing <see cref="StandardTryHandleRecordOp"/>.
    /// </summary>
    public partial class TryHandleRecordResult : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TryHandleRecordResult"/> class.
        /// </summary>
        /// <param name="recordToHandle">The record to handle or null if <paramref name="isBlocked"/> is true.</param>
        /// <param name="isBlocked">OPTIONAL value indicating whether or not handling is blocked; DEFAULT is not blocked.</param>
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
        public StreamRecord RecordToHandle { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not handling is blocked.
        /// </summary>
        public bool IsBlocked { get; private set; }
    }
}
