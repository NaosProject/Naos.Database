// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UniqueLongIssuedEvent.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Event to record the execution of <see cref="GetNextUniqueLongOp"/>.
    /// </summary>
    public partial class UniqueLongIssuedEvent : EventBase, IHaveDetails
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueLongIssuedEvent"/> class.
        /// </summary>
        /// <param name="uniqueLong">The unique long that was issued.</param>
        /// <param name="timestampUtc">The timestamp in UTC.</param>
        /// <param name="details">OPTIONAL details about the event.  DEFAULT is no details.</param>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "long", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public UniqueLongIssuedEvent(
            long uniqueLong,
            DateTime timestampUtc,
            string details = null)
            : base(timestampUtc)
        {
            this.UniqueLong = uniqueLong;
            this.Details = details;
        }

        /// <summary>
        /// Gets the unique long that was issued.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "long", Justification = NaosSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public long UniqueLong { get; private set; }

        /// <inheritdoc />
        public string Details { get; private set; }
    }
}
