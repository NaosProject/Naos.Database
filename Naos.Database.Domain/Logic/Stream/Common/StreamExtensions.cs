// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;

    /// <summary>
    /// Extension methods on <see cref="IStream"/>.
    /// </summary>
    public static partial class StreamExtensions
    {
        /// <summary>
        /// Gets the <see cref="StringSerializedIdentifier"/> from the provided <paramref name="id"/>.
        /// </summary>
        /// <typeparam name="TId">The type of the identifier to convert.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="id">The identifier to convert.</param>
        /// <returns>The <see cref="StringSerializedIdentifier"/> representation of the <paramref name="id"/>.</returns>
        public static StringSerializedIdentifier GetStringSerializedIdentifier<TId>(
            this IStream stream,
            TId id)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var identifierSerializer = stream.SerializerFactory.BuildSerializer(stream.DefaultSerializerRepresentation);
            var serializedIdentifier = identifierSerializer.SerializeToString(id);
            var result = new StringSerializedIdentifier(serializedIdentifier, typeof(TId).ToRepresentation());

            return result;
        }
    }
}
