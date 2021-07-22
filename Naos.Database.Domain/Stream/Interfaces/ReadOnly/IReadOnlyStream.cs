// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReadOnlyStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Stream interface, a stream is a list of objects ordered by timestamp, only read operations are supported.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public interface IReadOnlyStream
        :
            IStream,
            IStreamReadProtocolFactory,
            ISyncReturningProtocol<GetDistinctStringSerializedIdsOp, IReadOnlyCollection<string>>,
            ISyncReturningProtocol<GetRecordByInternalRecordIdOp, StreamRecord>,
            ISyncReturningProtocol<GetLatestRecordOp, StreamRecord>,
            ISyncReturningProtocol<GetLatestRecordMetadataByIdOp, StreamRecordMetadata>,
            ISyncReturningProtocol<GetAllRecordsByIdOp, IReadOnlyList<StreamRecord>>,
            ISyncReturningProtocol<GetAllRecordsMetadataByIdOp, IReadOnlyList<StreamRecordMetadata>>,
            ISyncReturningProtocol<DoesAnyExistByIdOp, bool>,
            ISyncReturningProtocol<GetLatestRecordByIdOp, StreamRecord>
    {
    }
}
