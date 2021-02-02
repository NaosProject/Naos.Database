// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateStreamResult.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Result of <see cref="CreateStreamOp"/>.
    /// </summary>
    public partial class CreateStreamResult : IModelViaCodeGen, IForsakeDeepCloneWithVariantsViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateStreamResult"/> class.
        /// </summary>
        /// <param name="alreadyExisted">if set to <c>true</c> [already existed].</param>
        /// <param name="wasCreated">if set to <c>true</c> [was created].</param>
        public CreateStreamResult(
            bool alreadyExisted,
            bool wasCreated)
        {
            if (!alreadyExisted && !wasCreated)
            {
                throw new ArgumentNullException(FormattableString.Invariant($"Cannot have both '{nameof(alreadyExisted)}' AND '{nameof(wasCreated)}' be false; the expectation is that the stream was created or there was an existing one."));
            }

            this.AlreadyExisted = alreadyExisted;
            this.WasCreated = wasCreated;
        }

        /// <summary>
        /// Gets a value indicating whether [already existed].
        /// </summary>
        /// <value><c>true</c> if [already existed]; otherwise, <c>false</c>.</value>
        public bool AlreadyExisted { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [was created].
        /// </summary>
        /// <value><c>true</c> if [was created]; otherwise, <c>false</c>.</value>
        public bool WasCreated { get; private set; }
    }
}
