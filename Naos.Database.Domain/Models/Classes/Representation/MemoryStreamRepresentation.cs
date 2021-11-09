// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemoryStreamRepresentation.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// A representation of an in-memory stream.
    /// </summary>
    public partial class MemoryStreamRepresentation : StreamRepresentationBase, IHaveStringId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryStreamRepresentation"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        /// <param name="id">A unique identifier for the stream.</param>
        public MemoryStreamRepresentation(
            string name,
            string id)
            : base(name)
        {
            id.MustForArg(nameof(id)).NotBeNullNorWhiteSpace();

            this.Id = id;
        }

        /// <inheritdoc />
        public string Id { get; private set; }
    }
}
