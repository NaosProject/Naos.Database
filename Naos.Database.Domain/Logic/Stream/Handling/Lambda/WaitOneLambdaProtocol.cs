// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WaitOneLambdaProtocol.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Threading.Tasks;
    using OBeautifulCode.Type;

    /// <summary>
    /// Implements <see cref="IWaitOne"/> by protocolizing a lambda.
    /// </summary>
    public class WaitOneLambdaProtocol :
        LambdaReturningProtocol<WaitOneOp, ReleaseMutexOp>,
        IWaitOne
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WaitOneLambdaProtocol"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The lambda to protocol the operation.</param>
        public WaitOneLambdaProtocol(
            Func<WaitOneOp, ReleaseMutexOp> synchronousLambda)
            : base(synchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitOneLambdaProtocol"/> class.
        /// </summary>
        /// <param name="asynchronousLambda">The lambda to protocol the operation.</param>
        public WaitOneLambdaProtocol(
            Func<WaitOneOp, Task<ReleaseMutexOp>> asynchronousLambda)
            : base(asynchronousLambda)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitOneLambdaProtocol"/> class.
        /// </summary>
        /// <param name="synchronousLambda">The synchronous lambda to protocol the operation.</param>
        /// <param name="asynchronousLambda">The asynchronous lambda to protocol the operation.</param>
        public WaitOneLambdaProtocol(
            Func<WaitOneOp, ReleaseMutexOp> synchronousLambda,
            Func<WaitOneOp, Task<ReleaseMutexOp>> asynchronousLambda)
            : base(synchronousLambda, asynchronousLambda)
        {
        }
    }
}
