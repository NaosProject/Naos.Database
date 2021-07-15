// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamWriteWithIdProtocols{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.CodeAnalysis.Recipes;


    /// <summary>
    /// Interface to protocol the basic stream data operations with a known identifier.
    /// </summary>
    /// <typeparam name="TId">Type of the identifier.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = NaosSuppressBecause.CA1040_AvoidEmptyInterfaces_NeedToIdentifyGroupOfTypesAndPreferInterfaceOverAttribute)]
    public interface IStreamWriteWithIdProtocols<TId>
    {
    }
}
