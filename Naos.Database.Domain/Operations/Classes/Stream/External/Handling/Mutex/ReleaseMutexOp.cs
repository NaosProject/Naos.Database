// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReleaseMutexOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// An operation to release a mutex by self-cancelling the handling of a <see cref="MutexObject"/> in a stream,
    /// scoped by identifier and handling concern.
    /// </summary>
    public partial class ReleaseMutexOp : VoidOperationBase, IHaveInternalRecordId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReleaseMutexOp"/> class.
        /// </summary>
        /// <param name="waitOneOp">The operation that acquired a mutex by exclusively handling a <see cref="MutexObject"/> in a stream.</param>
        /// <param name="internalRecordId">The internal record id of the <see cref="MutexObject"/> that was successfully handled by the execution of the specified <paramref name="waitOneOp"/>.</param>
        public ReleaseMutexOp(
            WaitOneOp waitOneOp,
            long internalRecordId)
        {
            waitOneOp.MustForArg(nameof(waitOneOp)).NotBeNull();

            this.WaitOneOp = waitOneOp;
            this.InternalRecordId = internalRecordId;
        }

        /// <summary>
        /// Gets the operation that acquired a mutex by exclusively handling a <see cref="MutexObject"/> in a stream.
        /// </summary>
        public WaitOneOp WaitOneOp { get; private set; }

        /// <inheritdoc />
        public long InternalRecordId { get; private set; }
    }
}
