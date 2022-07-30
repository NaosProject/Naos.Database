// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NullDatabaseDefinition.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Null object implementation of <see cref="IDatabaseDefinition"/>.
    /// </summary>
    public partial class NullDatabaseDefinition : IDatabaseDefinition, IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullDatabaseDefinition"/> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        public NullDatabaseDefinition(
            string databaseName)
        {
            databaseName.MustForArg(nameof(databaseName)).NotBeNullNorWhiteSpace();

            this.DatabaseName = databaseName;
        }

        /// <inheritdoc />
        public string DatabaseName { get; private set; }
    }
}
