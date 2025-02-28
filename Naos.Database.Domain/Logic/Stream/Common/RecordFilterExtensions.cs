// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordFilterExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;

    /// <summary>
    /// Extensions used to construct a <see cref="RecordFilter"/>.
    /// </summary>
    public static class RecordFilterExtensions
    {
        /// <summary>
        /// Constructs a <see cref="RecordFilter"/> that filters on the specified object type.
        /// </summary>
        /// <param name="objectType">The object type.</param>
        /// <returns>
        /// The <see cref="RecordFilter"/>.
        /// </returns>
        public static RecordFilter ToRecordFilter(
            this Type objectType)
        {
            objectType.MustForArg(nameof(objectType)).NotBeNull();

            var result = new RecordFilter(objectTypes: new[] { objectType.ToRepresentation() });

            return result;
        }

        /// <summary>
        /// Constructs a <see cref="RecordFilter"/> that filters on the specified object types.
        /// </summary>
        /// <param name="objectTypes">The object types.</param>
        /// <returns>
        /// The <see cref="RecordFilter"/>.
        /// </returns>
        public static RecordFilter ToRecordFilter(
            this IReadOnlyCollection<Type> objectTypes)
        {
            objectTypes.MustForArg(nameof(objectTypes)).NotBeNullNorEmptyEnumerableNorContainAnyNulls();

            var result = new RecordFilter(objectTypes: objectTypes.Select(_ => _.ToRepresentation()).ToList());

            return result;
        }
    }
}