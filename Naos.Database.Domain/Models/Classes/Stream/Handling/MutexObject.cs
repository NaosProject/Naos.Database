// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MutexObject.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// The result of executing a <see cref="StandardCreateStreamOp"/>.
    /// </summary>
    public partial class MutexObject : IModelViaCodeGen, IHaveStringId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MutexObject"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public MutexObject(
            string id)
        {
            id.MustForArg(nameof(id)).NotBeNullNorWhiteSpace();

            this.Id = id;
        }

        /// <inheritdoc />
        public string Id { get; private set; }
    }
}
