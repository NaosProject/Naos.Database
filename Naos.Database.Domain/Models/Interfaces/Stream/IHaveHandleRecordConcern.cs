// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHaveHandleRecordConcern.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Interface to expose a record handling concern.
    /// </summary>
    /// <remarks>
    /// A concern enables multiple, unrelated and/or uncoordinated consumers to handle the same record.
    /// </remarks>
    public interface IHaveHandleRecordConcern
    {
        /// <summary>
        /// Gets the record handling concern.
        /// </summary>
        string Concern { get; }
    }
}