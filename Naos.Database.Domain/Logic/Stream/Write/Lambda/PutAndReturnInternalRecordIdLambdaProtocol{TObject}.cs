// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PutAndReturnInternalRecordIdLambdaProtocol{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Threading.Tasks;
    using OBeautifulCode.Type;

    /// <summary>
    /// Implements <see cref="IPutAndReturnInternalRecordId{TObject}"/> by protocolizing a lambda.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class PutAndReturnInternalRecordIdLambdaProtocol<TObject> :
        LambdaReturningProtocol<PutAndReturnInternalRecordIdOp<TObject>, long?>,
        IPutAndReturnInternalRecordId<TObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutAndReturnInternalRecordIdLambdaProtocol{TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public PutAndReturnInternalRecordIdLambdaProtocol(
            Func<PutAndReturnInternalRecordIdOp<TObject>, long?> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PutAndReturnInternalRecordIdLambdaProtocol{TObject}"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public PutAndReturnInternalRecordIdLambdaProtocol(
            Func<PutAndReturnInternalRecordIdOp<TObject>, Task<long?>> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PutAndReturnInternalRecordIdLambdaProtocol{TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public PutAndReturnInternalRecordIdLambdaProtocol(
            Func<PutAndReturnInternalRecordIdOp<TObject>, long?> synchronousLambda,
            Func<PutAndReturnInternalRecordIdOp<TObject>, Task<long?>> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
