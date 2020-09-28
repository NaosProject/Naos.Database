// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetStreamFromRepresentationOp{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Protocol.Domain;

    /// <summary>
    /// Operation to a <see cref="IResourceLocator"/> by a <see cref="Type"/>.
    /// </summary>
    /// <typeparam name="TId">The type of ID of the stream.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "AllStream", Justification = NaosSuppressBecause.CA1702_CompoundWordsShouldBeCasedCorrectly_AnalyzerIsIncorrectlyDetectingCompoundWords)]
    public partial class GetStreamFromRepresentationOp<TId> : ReturningOperationBase<IStream<TId>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetStreamFromRepresentationOp{TId}"/> class.
        /// </summary>
        /// <param name="streamRepresentation">The stream representation.</param>
        public GetStreamFromRepresentationOp(
            StreamRepresentation<TId> streamRepresentation)
        {
            this.StreamRepresentation = streamRepresentation ?? throw new ArgumentNullException(nameof(streamRepresentation));
        }

        /// <summary>
        /// Gets the stream representation.
        /// </summary>
        /// <value>The stream representation.</value>
        public StreamRepresentation<TId> StreamRepresentation { get; private set; }
    }
}
