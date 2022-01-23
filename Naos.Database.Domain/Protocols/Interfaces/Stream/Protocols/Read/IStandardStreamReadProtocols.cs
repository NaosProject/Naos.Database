// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStandardStreamReadProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the standard stream reading operations.
    /// </summary>
    public interface IStandardStreamReadProtocols
        :
          ISyncReturningProtocol<StandardGetDistinctStringSerializedIdsOp, IReadOnlyCollection<StringSerializedIdentifier>>,
          ISyncReturningProtocol<StandardDoesAnyExistByIdOp, bool>,
          ISyncReturningProtocol<StandardGetRecordByInternalRecordIdOp, StreamRecord>,
          ISyncReturningProtocol<StandardGetRecordIdsOp, IReadOnlyCollection<long>>,
          ISyncReturningProtocol<StandardGetLatestRecordByTagsOp, StreamRecord>,
          ISyncReturningProtocol<StandardGetLatestRecordOp, StreamRecord>,
          ISyncReturningProtocol<StandardGetLatestRecordMetadataByIdOp, StreamRecordMetadata>,
          ISyncReturningProtocol<StandardGetLatestRecordByIdOp, StreamRecord>,
          ISyncReturningProtocol<StandardGetLatestStringSerializedObjectByIdOp, string>
    {
    }
}
