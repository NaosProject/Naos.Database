// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutWithIdLambdaProtocol{TId,TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Threading.Tasks;
    using OBeautifulCode.Type;

    /// <summary>
    /// Implements <see cref="IPutWithId{TId, TObject}"/> by protocolizing a lambda.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier of the object.</typeparam>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class PutWithIdLambdaProtocol<TId, TObject> :
        LambdaVoidProtocol<PutWithIdOp<TId, TObject>>,
        IPutWithId<TId, TObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutWithIdLambdaProtocol{TId, TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public PutWithIdLambdaProtocol(
            Action<PutWithIdOp<TId, TObject>> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PutWithIdLambdaProtocol{TId, TObject}"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public PutWithIdLambdaProtocol(
            Func<PutWithIdOp<TId, TObject>, Task> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PutWithIdLambdaProtocol{TId, TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public PutWithIdLambdaProtocol(
            Action<PutWithIdOp<TId, TObject>> synchronousLambda,
            Func<PutWithIdOp<TId, TObject>, Task> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
