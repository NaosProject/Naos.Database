// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITryHandleRecordOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to manage common aspects of the Try Handle operation family which cannot have a base class because they are all based on different operation types.
    /// </summary>
    public interface ITryHandleRecordOp : IHaveDetails, IHaveTags, IHaveHandleRecordConcern
    {
        /// <summary>
        /// Gets the strategy to use to filter on the version of the object type.
        /// </summary>
        VersionMatchStrategy VersionMatchStrategy { get; }

        /// <summary>
        /// Gets a value that specifies how to order the resulting records.
        /// </summary>
        OrderRecordsBy OrderRecordsBy { get; }

        /// <summary>
        /// Gets the minimum internal record identifier to consider for handling or null for no minimum identifier.
        /// </summary>
        /// <remarks>
        /// This will allow for ordinal traversal and handle each record once before starting over which can be a desired behavior on protocols that self-cancel and are long running.
        /// </remarks>
        long? MinimumInternalRecordId { get; }

        /// <summary>
        /// Gets a value indicating whether the resulting <see cref="IHandlingEvent"/> should inherit tags from the record being handled.
        /// </summary>
        bool InheritRecordTags { get; }
    }
}
