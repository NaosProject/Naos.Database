// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReadOnlyStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Protocol.Domain;

    /// <summary>
    /// Stream interface, a stream is a list of objects ordered by timestamp, only read operations are supported.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public interface IReadOnlyStream
        :
            IStream,
            IStreamReadProtocolFactory,
            IReturningProtocol<GetDistinctStringSerializedIdsOp, IReadOnlyCollection<string>>,
            IReturningProtocol<GetRecordByInternalRecordIdOp, StreamRecord>,
            IReturningProtocol<GetLatestRecordOp, StreamRecord>,
            IReturningProtocol<GetLatestRecordMetadataByIdOp, StreamRecordMetadata>,
            IReturningProtocol<GetAllRecordsByIdOp, IReadOnlyList<StreamRecord>>,
            IReturningProtocol<GetAllRecordsMetadataByIdOp, IReadOnlyList<StreamRecordMetadata>>,
            IReturningProtocol<DoesAnyExistByIdOp, bool>,
            IReturningProtocol<GetLatestRecordByIdOp, StreamRecord>
    {
    }
}
