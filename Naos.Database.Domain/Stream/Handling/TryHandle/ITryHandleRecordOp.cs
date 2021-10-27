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
    public interface ITryHandleRecordOp : IHaveDetails, IHaveTags
    {
        /// <summary>
        /// Gets the concern.
        /// </summary>
        string Concern { get; }

        /// <summary>
        /// Gets the type version match strategy.
        /// </summary>
        VersionMatchStrategy VersionMatchStrategy { get; }

        /// <summary>
        /// Gets the order records strategy.
        /// </summary>
        OrderRecordsBy OrderRecordsBy { get; }

        /// <summary>
        /// Gets the optional minimum record identifier to consider for handling (this will allow for ordinal traversal and handle each record once before starting over which can be desired behavior on things that self-cancel and are long running).
        /// </summary>
        long? MinimumInternalRecordId { get; }

        /// <summary>
        /// Gets a value indicating whether handling entries should include any tags on the record being handled.
        /// </summary>
        bool InheritRecordTags { get; }
    }
}
