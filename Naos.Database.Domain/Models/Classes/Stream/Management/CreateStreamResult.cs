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
    /// The result of executing a <see cref="StandardCreateStreamOp"/>.
    /// </summary>
    public partial class CreateStreamResult : IModelViaCodeGen, IForsakeDeepCloneWithVariantsViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateStreamResult"/> class.
        /// </summary>
        /// <param name="alreadyExisted">A value indicating whether the stream already exists.</param>
        /// <param name="wasCreated">A value indicating whether the stream was created.</param>
        public CreateStreamResult(
            bool alreadyExisted,
            bool wasCreated)
        {
            if ((!alreadyExisted) && (!wasCreated))
            {
                throw new ArgumentException(FormattableString.Invariant($"Cannot have both '{nameof(alreadyExisted)}' AND '{nameof(wasCreated)}' be false; the expectation is that the stream was created or there was an existing one."));
            }

            this.AlreadyExisted = alreadyExisted;
            this.WasCreated = wasCreated;
        }

        /// <summary>
        /// Gets a value indicating whether the stream already exists.
        /// </summary>
        public bool AlreadyExisted { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the stream was created.
        /// </summary>
        public bool WasCreated { get; private set; }
    }
}
