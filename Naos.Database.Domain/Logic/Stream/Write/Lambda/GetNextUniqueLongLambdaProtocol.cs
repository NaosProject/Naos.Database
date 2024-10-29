// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetNextUniqueLongLambdaProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Threading.Tasks;
    using OBeautifulCode.Type;

    /// <summary>
    /// Implements <see cref="IGetNextUniqueLong"/> by protocolizing a lambda.
    /// </summary>
    public class GetNextUniqueLongLambdaProtocol :
        LambdaReturningProtocol<GetNextUniqueLongOp, long>,
        IGetNextUniqueLong
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetNextUniqueLongLambdaProtocol"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public GetNextUniqueLongLambdaProtocol(
            Func<GetNextUniqueLongOp, long> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNextUniqueLongLambdaProtocol"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public GetNextUniqueLongLambdaProtocol(
            Func<GetNextUniqueLongOp, Task<long>> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNextUniqueLongLambdaProtocol"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public GetNextUniqueLongLambdaProtocol(
            Func<GetNextUniqueLongOp, long> synchronousLambda,
            Func<GetNextUniqueLongOp, Task<long>> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
