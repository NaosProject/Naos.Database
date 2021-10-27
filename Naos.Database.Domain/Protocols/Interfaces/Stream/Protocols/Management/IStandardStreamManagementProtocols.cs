// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStandardStreamManagementProtocols.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the standard stream management operations.
    /// </summary>
    public interface IStandardStreamManagementProtocols
        : ISyncReturningProtocol<StandardCreateStreamOp, StandardCreateStreamResult>,
          ISyncVoidProtocol<StandardDeleteStreamOp>,
          ISyncVoidProtocol<StandardPruneStreamOp>
    {
    }
}
