// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRecordHandlingOnlyStream.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// A stream that can only perform recording handling tasks.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "'Stream' is the best term we could come up with; it's potential confusion with System.IO.Stream was debated.")]
    public interface IRecordHandlingOnlyStream
        : IStream,
          IStreamRecordHandlingProtocolFactory,
          IStreamDistributedMutexProtocolFactory
    {
    }
}
