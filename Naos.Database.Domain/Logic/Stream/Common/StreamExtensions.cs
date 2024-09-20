// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

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
        /// <param name="typeSelectionStrategy">OPTIONAL strategy to use to select the types that are applicable to this operation (e.g. object type, object's identifier type).  DEFAULT is to use the runtime types and throw if any of them are null.</param>
        /// <returns>The <see cref="StringSerializedIdentifier"/> representation of the <paramref name="id"/>.</returns>
        public static StringSerializedIdentifier GetStringSerializedIdentifier<TId>(
            this IStream stream,
            TId id,
            TypeSelectionStrategy typeSelectionStrategy = TypeSelectionStrategy.UseRuntimeType)
        {
            stream.MustForArg(nameof(stream)).NotBeNull();

            var identifierSerializer = stream.SerializerFactory.BuildSerializer(stream.DefaultSerializerRepresentation);
            var serializedIdentifier = identifierSerializer.SerializeToString(id);
            var typeOfId = typeSelectionStrategy.Apply(id);
            var result = new StringSerializedIdentifier(serializedIdentifier, typeOfId.ToRepresentation());

            return result;
        }
    }
}
