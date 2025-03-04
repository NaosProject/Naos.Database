// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamReadWithIdProtocols{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System.Collections.Generic;
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the basic stream reading operations with a typed identifier and with a typed record payload.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IStreamReadWithIdProtocols<TId, TObject> :
        IDoesAnyExistById<TId, TObject>,
        IGetAllObjectsById<TId, TObject>,
        IGetLatestObjectById<TId, TObject>,
        IGetLatestObjectsById<TId, TObject>,
        IGetLatestRecordById<TId, TObject>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="DoesAnyExistByIdOp{TId, TObject}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IDoesAnyExistById<TId, TObject> :
        ISyncAndAsyncReturningProtocol<DoesAnyExistByIdOp<TId, TObject>, bool>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetAllObjectsByIdOp{TId, TObject}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IGetAllObjectsById<TId, TObject> :
        ISyncAndAsyncReturningProtocol<GetAllObjectsByIdOp<TId, TObject>, IReadOnlyList<TObject>>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetLatestObjectByIdOp{TId, TObject}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IGetLatestObjectById<TId, TObject> :
        ISyncAndAsyncReturningProtocol<GetLatestObjectByIdOp<TId, TObject>, TObject>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetLatestObjectsByIdOp{TId, TObject}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IGetLatestObjectsById<TId, TObject> :
        ISyncAndAsyncReturningProtocol<GetLatestObjectsByIdOp<TId, TObject>, IReadOnlyList<TObject>>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="GetLatestRecordByIdOp{TId, TObject}" />.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IGetLatestRecordById<TId, TObject> :
        ISyncAndAsyncReturningProtocol<GetLatestRecordByIdOp<TId, TObject>, StreamRecordWithId<TId, TObject>>
    {
    }
}
