﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLatestObjectLambdaProtocol{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Threading.Tasks;
    using OBeautifulCode.Type;

    /// <summary>
    /// Implements <see cref="IGetLatestObject{TObject}"/> by protocolizing a lambda.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class GetLatestObjectLambdaProtocol<TObject> :
        LambdaReturningProtocol<GetLatestObjectOp<TObject>, TObject>,
        IGetLatestObject<TObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestObjectLambdaProtocol{TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public GetLatestObjectLambdaProtocol(
            Func<GetLatestObjectOp<TObject>, TObject> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestObjectLambdaProtocol{TObject}"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public GetLatestObjectLambdaProtocol(
            Func<GetLatestObjectOp<TObject>, Task<TObject>> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLatestObjectLambdaProtocol{TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public GetLatestObjectLambdaProtocol(
            Func<GetLatestObjectOp<TObject>, TObject> synchronousLambda,
            Func<GetLatestObjectOp<TObject>, Task<TObject>> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
