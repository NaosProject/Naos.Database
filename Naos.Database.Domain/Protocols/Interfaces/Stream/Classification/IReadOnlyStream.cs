// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReadOnlyStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// A read-only stream.
    /// </summary>
    public interface IReadOnlyStream
        : IStream,
          IStreamReadProtocolFactory
    {
    }
}
