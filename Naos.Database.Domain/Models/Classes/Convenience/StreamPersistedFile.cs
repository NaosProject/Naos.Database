// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamPersistedFile.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Diagnostics.CodeAnalysis;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// A file that is stored in a stream.
    /// </summary>
    public partial class StreamPersistedFile : IHaveStringId, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamPersistedFile"/> class.
        /// </summary>
        /// <param name="id">The identifier of the file in the stream.</param>
        /// <param name="streamRepresentation">The representation of the stream.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="byteCount">The number of bytes in the file.</param>
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "byte", Justification = ObcSuppressBecause.CA1720_IdentifiersShouldNotContainTypeNames_TypeNameAddsClarityToIdentifierAndAlternativesDegradeClarity)]
        public StreamPersistedFile(
            string id,
            IStreamRepresentation streamRepresentation,
            string fileName,
            long byteCount)
        {
            // hashes
            new { id }.AsArg().Must().NotBeNullNorWhiteSpace();
            new { streamRepresentation }.AsArg().Must().NotBeNull();
            new { fileName }.AsArg().Must().NotBeNullNorWhiteSpace();
            new { byteCount }.AsArg().Must().BeGreaterThanOrEqualTo(0L);

            this.Id = id;
            this.StreamRepresentation = streamRepresentation;
            this.FileName = fileName;
            this.ByteCount = byteCount;
        }

        /// <summary>
        /// Gets the identifier of the file in the stream.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the representation of the stream.
        /// </summary>
        public IStreamRepresentation StreamRepresentation { get; private set; }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the number of bytes in the file.
        /// </summary>
        public long ByteCount { get; private set; }
    }
}