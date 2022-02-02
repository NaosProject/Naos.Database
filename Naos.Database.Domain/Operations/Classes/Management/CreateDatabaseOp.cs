// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateDatabaseOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Create a database using the specified configuration.
    /// </summary>
    public partial class CreateDatabaseOp : VoidOperationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateDatabaseOp"/> class.
        /// </summary>
        /// <param name="definition">The database definition.</param>
        /// <param name="existingDatabaseStrategy">The strategy to use when the database already exists.</param>
        public CreateDatabaseOp(
            IDatabaseDefinition definition,
            ExistingDatabaseStrategy existingDatabaseStrategy)
        {
            definition.MustForArg(nameof(definition)).NotBeNull();
            existingDatabaseStrategy.MustForArg(nameof(existingDatabaseStrategy)).NotBeEqualTo(ExistingDatabaseStrategy.Unknown);

            this.Definition = definition;
            this.ExistingDatabaseStrategy = existingDatabaseStrategy;
        }

        /// <summary>
        /// Gets the definition to create the database.
        /// </summary>
        public IDatabaseDefinition Definition { get; private set; }

        /// <summary>
        /// Gets the strategy to use when the database already exists.
        /// </summary>
        public ExistingDatabaseStrategy ExistingDatabaseStrategy { get; private set; }
    }
}
