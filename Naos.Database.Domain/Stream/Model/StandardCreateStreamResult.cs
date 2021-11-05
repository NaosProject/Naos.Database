// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardCreateStreamResult.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Result of <see cref="StandardCreateStreamOp"/>.
    /// </summary>
    public partial class StandardCreateStreamResult : IModelViaCodeGen, IForsakeDeepCloneWithVariantsViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardCreateStreamResult"/> class.
        /// </summary>
        /// <param name="alreadyExisted">if set to <c>true</c> [already existed].</param>
        /// <param name="wasCreated">if set to <c>true</c> [was created].</param>
        public StandardCreateStreamResult(
            bool alreadyExisted,
            bool wasCreated)
        {
            if (!alreadyExisted && !wasCreated)
            {
                throw new ArgumentException(FormattableString.Invariant($"Cannot have both '{nameof(alreadyExisted)}' AND '{nameof(wasCreated)}' be false; the expectation is that the stream was created or there was an existing one."));
            }

            this.AlreadyExisted = alreadyExisted;
            this.WasCreated = wasCreated;
        }

        /// <summary>
        /// Gets a value indicating whether [already existed].
        /// </summary>
        public bool AlreadyExisted { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [was created].
        /// </summary>
        public bool WasCreated { get; private set; }
    }
}
