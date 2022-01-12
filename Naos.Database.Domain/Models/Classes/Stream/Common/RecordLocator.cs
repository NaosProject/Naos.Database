// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordLocator.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Basic concrete implementation of <see cref="IRecordLocator" />.
    /// </summary>
    public partial class RecordLocator : IRecordLocator, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordLocator"/> class.
        /// </summary>
        /// <param name="resourceLocator">The resource locator that the <see cref="InternalRecordId"/> is located at.</param>
        /// <param name="internalRecordId">The internal record identifier in the stream specified by <see cref="ResourceLocator"/>.</param>
        public RecordLocator(
            IResourceLocator resourceLocator,
            long internalRecordId)
        {
            resourceLocator.MustForArg(nameof(resourceLocator)).NotBeNull();

            this.ResourceLocator = resourceLocator;
            this.InternalRecordId = internalRecordId;
        }

        /// <inheritdoc />
        public IResourceLocator ResourceLocator { get; private set; }

        /// <inheritdoc />
        public long InternalRecordId { get; private set; }
    }
}
