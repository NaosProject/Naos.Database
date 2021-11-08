// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrderRecordsBy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Determine how to order records on a multiple record query.
    /// </summary>
    public enum OrderRecordsBy
    {
        /// <summary>
        /// Unknown (default).
        /// </summary>
        Unknown,

        /// <summary>
        /// Orders the records by the internal record identifier from lowest to highest.
        /// </summary>
        InternalRecordIdAscending,

        /// <summary>
        /// Orders the records by the internal record identifier from highest to lowest.
        /// </summary>
        InternalRecordIdDescending,

        /// <summary>
        /// Orders the records randomly each time.
        /// </summary>
        Random,
    }
}
