// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReadWriteStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// A read/write stream.
    /// </summary>
    public interface IReadWriteStream
        : IReadOnlyStream,
          IWriteOnlyStream
    {
    }
}
