// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamRecordHandlingProtocols{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the stream record handling operations without a typed identifier and with a typed record payload.
    /// </summary>
    /// <typeparam name="TObject">The type of object in the record.</typeparam>
    public interface IStreamRecordHandlingProtocols<TObject>
        : ISyncAndAsyncReturningProtocol<TryHandleRecordOp<TObject>, StreamRecord<TObject>>
    {
    }
}
