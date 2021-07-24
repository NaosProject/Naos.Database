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
        :
          ISyncAndAsyncReturningProtocol<GetDistinctStringSerializedIdsOp, IReadOnlyCollection<string>>,
          ISyncAndAsyncReturningProtocol<DoesAnyExistByIdOp, bool>,
          ISyncAndAsyncReturningProtocol<GetRecordByInternalRecordIdOp, StreamRecord>,
          ISyncAndAsyncReturningProtocol<GetAllRecordsByIdOp, IReadOnlyList<StreamRecord>>,
          ISyncAndAsyncReturningProtocol<GetAllRecordsMetadataByIdOp, IReadOnlyList<StreamRecordMetadata>>,
          ISyncAndAsyncReturningProtocol<GetLatestRecordByTagOp, StreamRecord>,
          ISyncAndAsyncReturningProtocol<GetLatestRecordOp, StreamRecord>,
          ISyncAndAsyncReturningProtocol<GetLatestRecordMetadataByIdOp, StreamRecordMetadata>,
          ISyncAndAsyncReturningProtocol<GetLatestRecordByIdOp, StreamRecord>
    {
    }
}
