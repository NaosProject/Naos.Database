// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITryHandleRecordOpBase.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using Naos.Protocol.Domain;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to manage common aspects of the Try Handle operation family which cannot have a base class because they are all based on different operation types.
    /// </summary>
    public interface ITryHandleRecordOpBase : ISpecifyResourceLocator, IHaveDetails, IHaveTags
    {
        /// <summary>
        /// Gets the concern.
        /// </summary>
        /// <value>The concern.</value>
        string Concern { get; }

        /// <summary>
        /// Gets the type version match strategy.
        /// </summary>
        /// <value>The type version match strategy.</value>
        TypeVersionMatchStrategy TypeVersionMatchStrategy { get; }

        /// <summary>
        /// Gets the order records strategy.
        /// </summary>
        /// <value>The order records strategy.</value>
        OrderRecordsStrategy OrderRecordsStrategy { get; }

        /// <summary>
        /// Gets the optional minimum record identifier to consider for handling (this will allow for ordinal traversal and handle each record once before starting over which can be desired behavior on things that self-cancel and are long running).
        /// </summary>
        /// <value>The minimum internal record identifier.</value>
        long? MinimumInternalRecordId { get; }

        /// <summary>
        /// Gets a value indicating whether handling entries should include any tags on the record being handled.
        /// </summary>
        /// <value><c>true</c> if [inherit record tags]; otherwise, <c>false</c>.</value>
        bool InheritRecordTags { get; }
    }
}
