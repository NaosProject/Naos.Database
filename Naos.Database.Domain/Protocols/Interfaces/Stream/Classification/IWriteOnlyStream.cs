// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWriteOnlyStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// A write-only stream.
    /// </summary>
    public interface IWriteOnlyStream
        : IStream,
          IStreamWriteProtocolFactory
    {
    }
}
