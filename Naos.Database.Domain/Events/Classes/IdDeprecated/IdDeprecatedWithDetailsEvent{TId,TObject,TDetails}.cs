// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdDeprecatedWithDetailsEvent{TId,TObject,TDetails}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Event to indicate in a stream that an identifier is no longer expected to be used.
    /// This can be useful in marking the event and also to filter out identifiers which have been removed.
    /// </summary>
    /// <typeparam name="TId">Type of identifier being deprecated.</typeparam>
    /// <typeparam name="TObject">Type of object for the identifier being deprecated.</typeparam>
    /// <typeparam name="TDetails">The type of the details.</typeparam>
    /// <remarks>
    /// The consumer should consider the query pattern when putting this object into a stream, per
    /// <see cref="RecordFilter.DeprecatedIdTypes"/>.  It is easy to confuse the choice of this object versus
    /// <see cref="IdDeprecatedWithDetailsEvent{TDetails}"/> or <see cref="IdDeprecatedWithDetailsEvent{TObject, TDetails}"/>, how it is
    /// put into a stream, and how these objects ultimately aid in deprecating records.
    /// Please see notes in the constructor of <see cref="RecordFilter"/> for <see cref="RecordFilter.DeprecatedIdTypes"/>.
    /// </remarks>
    [SuppressMessage("Microsoft.Design", "CA1005:AvoidExcessiveParametersOnGenericTypes", Justification = NaosSuppressBecause.CA1005_AvoidExcessiveParametersOnGenericTypes_SpecifiedParametersRequiredForNeededFunctionality)]
    public partial class IdDeprecatedWithDetailsEvent<TId, TObject, TDetails> : EventBase<TId>
        where TDetails : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdDeprecatedWithDetailsEvent{TId, TObject, TDetails}"/> class.
        /// </summary>
        /// <param name="id">The identifier being deprecated.</param>
        /// <param name="timestampUtc">The timestamp UTC.</param>
        /// <param name="details">The details.</param>
        public IdDeprecatedWithDetailsEvent(
            TId id,
            DateTime timestampUtc,
            TDetails details)
            : base(id, timestampUtc)
        {
            details.MustForArg(nameof(details)).NotBeNull();

            this.Details = details;
        }

        /// <summary>
        /// Gets the details.
        /// </summary>
        public TDetails Details { get; private set; }
    }
}
