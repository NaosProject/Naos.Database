// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAllObjectsLambdaProtocol{TObject}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OBeautifulCode.Type;

    /// <summary>
    /// Implements <see cref="IGetAllObjects{TObject}"/> by protocolizing a lambda.
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public class GetAllObjectsLambdaProtocol<TObject> :
        LambdaReturningProtocol<GetAllObjectsOp<TObject>, IReadOnlyList<TObject>>,
        IGetAllObjects<TObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllObjectsLambdaProtocol{TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public GetAllObjectsLambdaProtocol(
            Func<GetAllObjectsOp<TObject>, IReadOnlyList<TObject>> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllObjectsLambdaProtocol{TObject}"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public GetAllObjectsLambdaProtocol(
            Func<GetAllObjectsOp<TObject>, Task<IReadOnlyList<TObject>>> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllObjectsLambdaProtocol{TObject}"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public GetAllObjectsLambdaProtocol(
            Func<GetAllObjectsOp<TObject>, IReadOnlyList<TObject>> synchronousLambda,
            Func<GetAllObjectsOp<TObject>, Task<IReadOnlyList<TObject>>> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
