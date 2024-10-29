// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutWithIdAndReturnInternalRecordIdLambdaProtocol{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Threading.Tasks;
    using OBeautifulCode.Type;

    /// <summary>
    /// Implements <see cref="IPutWithIdAndReturnInternalRecordId{TId, TObject}"/> by protocolizing a lambda.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class PutWithIdAndReturnInternalRecordIdLambdaProtocol<TId, TObject> :
        LambdaReturningProtocol<PutWithIdAndReturnInternalRecordIdOp<TId, TObject>, long?>,
        IPutWithIdAndReturnInternalRecordId<TId, TObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutWithIdAndReturnInternalRecordIdLambdaProtocol{TId, TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public PutWithIdAndReturnInternalRecordIdLambdaProtocol(
            Func<PutWithIdAndReturnInternalRecordIdOp<TId, TObject>, long?> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PutWithIdAndReturnInternalRecordIdLambdaProtocol{TId, TObject}"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public PutWithIdAndReturnInternalRecordIdLambdaProtocol(
            Func<PutWithIdAndReturnInternalRecordIdOp<TId, TObject>, Task<long?>> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PutWithIdAndReturnInternalRecordIdLambdaProtocol{TId, TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public PutWithIdAndReturnInternalRecordIdLambdaProtocol(
            Func<PutWithIdAndReturnInternalRecordIdOp<TId, TObject>, long?> synchronousLambda,
            Func<PutWithIdAndReturnInternalRecordIdOp<TId, TObject>, Task<long?>> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
