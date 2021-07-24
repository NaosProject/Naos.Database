// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamReadProtocols{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the basic stream data operations without a known identifier.
    /// </summary>
    /// <typeparam name="TObject">Type of object used.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = NaosSuppressBecause.CA1040_AvoidEmptyInterfaces_NeedToIdentifyGroupOfTypesAndPreferInterfaceOverAttribute)]
    public interface IStreamReadProtocols<TObject>
        : ISyncAndAsyncReturningProtocol<GetLatestObjectOp<TObject>, TObject>,
          ISyncAndAsyncReturningProtocol<GetLatestObjectByTagOp<TObject>, TObject>,
          ISyncAndAsyncReturningProtocol<GetLatestRecordOp<TObject>, StreamRecord<TObject>>
    {
    }
}
