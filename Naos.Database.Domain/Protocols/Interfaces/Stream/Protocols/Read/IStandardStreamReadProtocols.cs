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
          ISyncReturningProtocol<StandardGetInternalRecordIdsOp, IReadOnlyCollection<long>>,
          ISyncReturningProtocol<StandardGetLatestRecordOp, StreamRecord>,
          ISyncReturningProtocol<StandardGetLatestStringSerializedObjectOp, string>
    {
    }
}
