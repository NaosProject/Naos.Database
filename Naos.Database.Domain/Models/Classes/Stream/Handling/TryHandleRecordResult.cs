// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryHandleRecordResult.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    using static System.FormattableString;

    /// <summary>
    /// The result of executing <see cref="StandardTryHandleRecordOp"/>.
    /// </summary>
    public partial class TryHandleRecordResult : IModelViaCodeGen, IForsakeDeepCloneWithVariantsViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TryHandleRecordResult"/> class.
        /// </summary>
        /// <param name="recordToHandle">The record to handle (must be null if <paramref name="isBlocked"/> is true).</param>
        /// <param name="isBlocked">OPTIONAL value indicating whether or not stream handling is blocked (not recording handling); DEFAULT is not blocked.</param>
        public TryHandleRecordResult(
            StreamRecord recordToHandle,
            bool isBlocked = false)
        {
            if (isBlocked)
            {
                recordToHandle.MustForArg(nameof(recordToHandle)).BeNull(Invariant($"{nameof(isBlocked)} is true"));
            }

            this.RecordToHandle = recordToHandle;
            this.IsBlocked = isBlocked;
        }

        /// <summary>
        /// Gets the record to handle.
        /// </summary>
        public StreamRecord RecordToHandle { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not handling is blocked.
        /// </summary>
        public bool IsBlocked { get; private set; }
    }
}
