// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderRecordsStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Strategy on how to order records on a multiple record query.
    /// </summary>
    public enum OrderRecordsStrategy
    {
        /// <summary>
        /// Orders the records by the internal record identifier from lowest to highest.
        /// </summary>
        ByInternalRecordIdAscending,
    }
}
