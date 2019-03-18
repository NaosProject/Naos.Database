// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConnectionDefinition.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Base interface for specific connection definitions.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = "Interface is a home for any future functionality like a summary string or something.")]
    public interface IConnectionDefinition
    {
    }
}
