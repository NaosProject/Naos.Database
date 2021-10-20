// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StreamRepresentation.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Stream description to allow the <see cref="GetStreamFromRepresentationByNameProtocolFactory"/> to produce the correct stream.
    /// </summary>
    public partial class StreamRepresentation : StreamRepresentationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamRepresentation"/> class.
        /// </summary>
        /// <param name="name">The name of the stream.</param>
        public StreamRepresentation(
            string name)
            : base(name)
        {
            /* no-op */
        }
    }
}
