// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HandlingStatusCompositionStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Type;

    /// <summary>
    /// Strategy on how to compose multiple strategies.
    /// </summary>
    public partial class HandlingStatusCompositionStrategy : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlingStatusCompositionStrategy"/> class.
        /// </summary>
        /// <param name="ignoreCancel">if set to <c>true</c> [ignore cancel].</param>
        public HandlingStatusCompositionStrategy(
            bool ignoreCancel = false)
        {
            this.IgnoreCancel = ignoreCancel;
        }

        /// <summary>
        /// Gets a value indicating whether [ignore cancel].
        /// </summary>
        /// <value><c>true</c> if [ignore cancel]; otherwise, <c>false</c>.</value>
        public bool IgnoreCancel { get; private set; }
    }
}
