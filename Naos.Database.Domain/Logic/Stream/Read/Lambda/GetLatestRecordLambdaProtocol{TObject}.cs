// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestRecordLambdaProtocol{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Threading.Tasks;
    using OBeautifulCode.Type;

    /// <summary>
    /// Implements <see cref="IGetLatestRecord{TObject}"/> by protocolizing a lambda.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class GetLatestRecordLambdaProtocol<TObject> :
        LambdaReturningProtocol<GetLatestRecordOp<TObject>, StreamRecord<TObject>>,
        IGetLatestRecord<TObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestRecordLambdaProtocol{TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public GetLatestRecordLambdaProtocol(
            Func<GetLatestRecordOp<TObject>, StreamRecord<TObject>> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestRecordLambdaProtocol{TObject}"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public GetLatestRecordLambdaProtocol(
            Func<GetLatestRecordOp<TObject>, Task<StreamRecord<TObject>>> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestRecordLambdaProtocol{TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public GetLatestRecordLambdaProtocol(
            Func<GetLatestRecordOp<TObject>, StreamRecord<TObject>> synchronousLambda,
            Func<GetLatestRecordOp<TObject>, Task<StreamRecord<TObject>>> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
