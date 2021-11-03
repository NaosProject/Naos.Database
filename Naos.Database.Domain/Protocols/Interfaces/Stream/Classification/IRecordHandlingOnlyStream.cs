// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRecordHandlingOnlyStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// A stream that can only perform recording handling tasks.
    /// </summary>
    public interface IRecordHandlingOnlyStream
        : IStream,
          IStreamRecordHandlingProtocolFactory
    {
    }
}
