// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStandardStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// A stream supporting a standard set of read, write, recording handling, and management operations.
    /// </summary>
    public interface IStandardStream
        :
          IReadWriteStream,
          IRecordHandlingOnlyStream,
          IManagementOnlyStream,
          IStandardStreamReadProtocols,
          IStandardStreamWriteProtocols,
          IStandardStreamManagementProtocols,
          IStandardStreamRecordHandlingProtocols
    {
    }
}
