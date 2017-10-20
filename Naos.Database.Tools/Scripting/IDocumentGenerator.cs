// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDocumentGenerator.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools
{
    using System;

    /// <summary>
    /// Interface for documenting database schema.
    /// </summary>
    public interface IDocumentGenerator : IDisposable
    {
        /// <summary>
        /// Add a table to document.
        /// </summary>
        /// <param name="name">Name of the table.</param>
        /// <param name="values">Multi-dimensional array describing the table.</param>
        /// <param name="merges">Multi-dimensional array of cells to merge.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "1#", Justification = "Specifically using a multi-dimensional array.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "2#", Justification = "Specifically using a multi-dimensional array.")]
        void AddTable(string name, string[,] values, int[,] merges);

        /// <summary>
        /// Add an entry to the document.
        /// </summary>
        /// <param name="entry">Entry to add.</param>
        /// <param name="size">Font size.</param>
        /// <param name="bold">Value indicating whether or not to bold the value.</param>
        void AddEntry(string entry, int size, bool bold);

        /// <summary>
        /// Add an entry to the document.
        /// </summary>
        /// <param name="entry">Entry to add.</param>
        /// <param name="size">Font size.</param>
        /// <param name="bold">Value indicating whether or not to bold the value.</param>
        /// <param name="alignment">Specific alignment.</param>
        void AddEntry(string entry, int size, bool bold, Alignment alignment);

        /// <summary>
        /// Indent in on the document.
        /// </summary>
        void Indent();

        /// <summary>
        /// Undent out on the document.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Undent", Justification = "Spelling/name is correct.")]
        void Undent();

        /// <summary>
        /// Close the document.
        /// </summary>
        void Close();
    }

    /// <summary>
    /// Enumeration of alignment types.
    /// </summary>
    public enum Alignment
    {
        /// <summary>
        /// Left alignment.
        /// </summary>
        Left,

        /// <summary>
        /// Right alignment.
        /// </summary>
        Right,

        /// <summary>
        /// Center alignment.
        /// </summary>
        Center,

        /// <summary>
        /// Justify alignment.
        /// </summary>
        Justify,
    }

    /// <summary>
    /// Null implementation of <see cref="IDocumentGenerator" />.
    /// </summary>
    public sealed class NullDocumentGenerator : IDocumentGenerator
    {
        /// <inheritdoc cref="IDocumentGenerator" />
        public void AddTable(string name, string[,] values, int[,] merges)
        {
            /* no-op */
        }

        /// <inheritdoc cref="IDocumentGenerator" />
        public void AddEntry(string entry, int size, bool bold)
        {
            /* no-op */
        }

        /// <inheritdoc cref="IDocumentGenerator" />
        public void AddEntry(string entry, int size, bool bold, Alignment alignment)
        {
            /* no-op */
        }

        /// <inheritdoc cref="IDocumentGenerator" />
        public void Indent()
        {
            /* no-op */
        }

        /// <inheritdoc cref="IDocumentGenerator" />
        public void Undent()
        {
            /* no-op */
        }

        /// <inheritdoc cref="IDocumentGenerator" />
        public void Close()
        {
            /* no-op */
        }

        /// <inheritdoc cref="IDocumentGenerator" />
        public void Dispose()
        {
            /* no-op */
        }
    }
}
