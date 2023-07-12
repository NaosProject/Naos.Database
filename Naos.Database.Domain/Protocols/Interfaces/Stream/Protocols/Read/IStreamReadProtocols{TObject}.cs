// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamReadProtocols{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the basic stream reading operations without a typed identifier and with a typed record payload.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IStreamReadProtocols<TObject>
        : IGetLatestObjectOp<TObject>,
          IGetLatestObjectByTags<TObject>,
          IGetLatestRecord<TObject>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetLatestObjectByTagsOp{TObject}" />.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IGetLatestObjectByTags<TObject> :
        ISyncAndAsyncReturningProtocol<GetLatestObjectByTagsOp<TObject>, TObject>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetLatestObjectOp{TObject}" />.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IGetLatestObjectOp<TObject> :
        ISyncAndAsyncReturningProtocol<GetLatestObjectOp<TObject>, TObject>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetLatestRecordOp{TObject}" />.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IGetLatestRecord<TObject> :
        ISyncAndAsyncReturningProtocol<GetLatestRecordOp<TObject>, StreamRecord<TObject>>
    {
    }
}
