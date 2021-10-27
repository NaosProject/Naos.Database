// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStandardReadWriteStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// A stream supporting a standard set of read/write operations.
    /// </summary>
    public interface IStandardReadWriteStream
        :
          IReadWriteStream,
          IStreamManagementProtocolFactory,
          IStreamRecordHandlingProtocolFactory,
          IStandardStreamManagementProtocols,
          IStandardStreamRecordHandlingProtocols,
          IStandardStreamReadProtocols,
          IStandardStreamWriteProtocols
    {
    }
}
