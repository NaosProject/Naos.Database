// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardStreamExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;

    /// <summary>
    /// Extension methods on <see cref="IStandardStream"/>.
    /// </summary>
    public static class StandardStreamExtensions
    {
        /// <summary>
        /// Gets the <see cref="StringSerializedIdentifier"/> from the provided <paramref name="id"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier to convert.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="id">The identifier to convert.</param>
        /// <returns>The <see cref="StringSerializedIdentifier"/> representation of the <paramref name="id"/>.</returns>
        public static StringSerializedIdentifier GetStringSerializedIdentifier<TId>(
            this IStandardStream stream,
            TId id)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var serializedIdentifier = stream.IdSerializer.SerializeToString(id);

            var typeOfId = typeof(TId);

            var result = new StringSerializedIdentifier(serializedIdentifier, typeOfId.ToRepresentation());

            return result;
        }
    }
}
