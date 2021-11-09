// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullStreamRepresentation.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// A null object pattern implementation of an <see cref="IStreamRepresentation"/>.
    /// </summary>
    public partial class NullStreamRepresentation : IStreamRepresentation, IModelViaCodeGen
    {
        /// <inheritdoc />
        public string Name => nameof(NullStreamRepresentation);
    }
}
