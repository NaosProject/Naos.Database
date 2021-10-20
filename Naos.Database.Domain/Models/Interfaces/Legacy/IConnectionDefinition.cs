// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConnectionDefinition.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain.Legacy
{
    using Naos.CodeAnalysis.Recipes;

    /// <summary>
    /// This is only for legacy SQL connectivity and will be removed in the future.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = NaosSuppressBecause.CA1040_AvoidEmptyInterfaces_NeedToIdentifyGroupOfTypesAndPreferInterfaceOverAttribute)]
    public interface IConnectionDefinition
    {
    }
}
