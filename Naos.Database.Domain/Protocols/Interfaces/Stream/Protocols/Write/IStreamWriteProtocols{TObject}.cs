﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStreamWriteProtocols{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Interface to protocol the basic stream writing operations without a typed identifier and with a typed record payload.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IStreamWriteProtocols<TObject> :
        IPutAndReturnInternalRecordId<TObject>,
        IPut<TObject>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="PutAndReturnInternalRecordIdOp{TObject}" />.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IPutAndReturnInternalRecordId<TObject> :
        ISyncAndAsyncReturningProtocol<PutAndReturnInternalRecordIdOp<TObject>, long?>
    {
    }

    /// <summary>
    /// Convenience interface for protocol that executes a <see cref="PutOp{TObject}" />.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IPut<TObject> : ISyncAndAsyncVoidProtocol<PutOp<TObject>>
    {
    }
}
