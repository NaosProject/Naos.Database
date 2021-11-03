// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IManagementOnlyStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// A stream that can only perform management tasks.
    /// </summary>
    public interface IManagementOnlyStream
        : IStream,
          IStreamManagementProtocolFactory
    {
    }
}
