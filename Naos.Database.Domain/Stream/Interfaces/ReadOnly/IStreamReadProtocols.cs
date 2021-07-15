// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamReadProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Type;


    /// <summary>
    /// Interface to protocol the basic stream data operations without a known identifier.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = NaosSuppressBecause.CA1040_AvoidEmptyInterfaces_NeedToIdentifyGroupOfTypesAndPreferInterfaceOverAttribute)]
    public interface IStreamReadProtocols
        : ISyncAndAsyncReturningProtocol<GetLatestRecordOp, StreamRecord>,
          ISyncAndAsyncReturningProtocol<GetLatestRecordByIdOp, StreamRecord>,
          ISyncAndAsyncReturningProtocol<GetAllRecordsByIdOp, IReadOnlyList<StreamRecord>>,
          ISyncAndAsyncReturningProtocol<GetLatestRecordMetadataByIdOp, StreamRecordMetadata>,
          ISyncAndAsyncReturningProtocol<GetAllRecordsMetadataByIdOp, IReadOnlyList<StreamRecordMetadata>>,
          ISyncAndAsyncReturningProtocol<DoesAnyExistByIdOp, bool>
    {
    }
}
