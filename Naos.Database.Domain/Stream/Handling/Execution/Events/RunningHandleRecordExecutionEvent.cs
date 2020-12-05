// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RunningHandleRecordExecutionEvent.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using Naos.Protocol.Domain;

    /// <summary>
    /// Event indicating that a <see cref="HandleRecordOp"/> is executing.
    /// </summary>
    public partial class RunningHandleRecordExecutionEvent : EventBase<long>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RunningHandleRecordExecutionEvent"/> class.
        /// </summary>
        /// <param name="id">The internal record identifier concerned with this handling sequence (the effective aggregate identifier of a record handling scenario).</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        public RunningHandleRecordExecutionEvent(
            long id,
            DateTime timestampUtc)
        : base(id, timestampUtc)
        {
        }
    }
}
