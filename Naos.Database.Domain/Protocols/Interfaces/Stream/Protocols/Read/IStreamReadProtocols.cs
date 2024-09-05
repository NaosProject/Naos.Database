// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamReadProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the basic stream reading operations without a typed identifier and without a typed record payload.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = NaosSuppressBecause.CA1040_AvoidEmptyInterfaces_NeedToIdentifyGroupOfTypesAndPreferInterfaceOverAttribute)]
    public interface IStreamReadProtocols
        : IGetAllRecordsMetadata
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetAllRecordsMetadataOp" />.
    /// </summary>
    public interface IGetAllRecordsMetadata :
        ISyncAndAsyncReturningProtocol<GetAllRecordsMetadataOp, IReadOnlyList<StreamRecordMetadata>>
    {
    }
}
