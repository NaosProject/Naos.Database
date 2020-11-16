// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullStreamRepresentation.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// A null object to be used as the id of an object in a <see cref="IReadWriteStream"/> that does not have an actual identifier.
    /// </summary>
    public partial class NullStreamRepresentation : IStreamRepresentation, IModelViaCodeGen
    {
        /// <inheritdoc />
        public string Name => nameof(NullStreamRepresentation);
    }
}
