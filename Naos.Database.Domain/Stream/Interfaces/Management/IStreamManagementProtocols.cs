// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamManagementProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using Naos.CodeAnalysis.Recipes;
    using Naos.Protocol.Domain;

    /// <summary>
    /// Management interface if an <see cref="IStream"/> supports.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Naming",
        "CA1711:IdentifiersShouldNotHaveIncorrectSuffix",
        Justification = NaosSuppressBecause.CA1711_IdentifiersShouldNotHaveIncorrectSuffix_TypeNameAddedAsSuffixForTestsWhereTypeIsPrimaryConcern)]
    public interface IStreamManagementProtocols
        : ISyncAndAsyncReturningProtocol<CreateStreamOp, CreateStreamResult>,
          ISyncAndAsyncVoidProtocol<DeleteStreamOp>,
          ISyncAndAsyncVoidProtocol<PruneBeforeInternalRecordDateOp>,
          ISyncAndAsyncVoidProtocol<PruneBeforeInternalRecordIdOp>
    {
    }
}
