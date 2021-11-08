// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagMatchStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Specifies how to match on tags.
    /// </summary>
    public enum TagMatchStrategy
    {
        /// <summary>
        /// Unknown (default).
        /// </summary>
        Unknown,

        /// <summary>
        /// There is a match if the record contains any of the queried tags.
        /// </summary>
        /// <remarks>
        /// Example:
        /// query tags         : tag1, tag2, tag3
        /// matching record    : tag3, tag4, tag5
        /// non-matching record: tag4, tag5 (does not have any of the query tags)
        /// </remarks>
        RecordContainsAnyQueryTag,

        /// <summary>
        /// There is a match if the record contains all of the queried tags (with extra tags on the record ignored).
        /// </summary>
        /// <remarks>
        /// Example:
        /// query tags         : tag1, tag2, tag3
        /// matching record    : tag1, tag2, tag3, tag4
        /// non-matching record: tag1, tag3, tag4 (missing tag2)
        /// </remarks>
        RecordContainsAllQueryTags,

        /// <summary>
        /// There is a match if the record contains all of the queried tags and no other tags.
        /// </summary>
        /// <remarks>
        /// Example:
        /// query tags         : tag1, tag2, tag3
        /// matching record    : tag1, tag2, tag3
        /// non-matching record: tag1, tag2 (missing tag3)
        /// </remarks>
        RecordContainsAllQueryTagsAndNoneOther,
    }
}
