// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteDatabaseOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Delete a database using the specified configuration.
    /// </summary>
    public partial class DeleteDatabaseOp : VoidOperationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteDatabaseOp"/> class.
        /// </summary>
        /// <param name="databaseName">The database name.</param>
        public DeleteDatabaseOp(
            string databaseName)
        {
            databaseName.MustForArg(nameof(databaseName)).NotBeNullNorWhiteSpace();

            this.DatabaseName = databaseName;
        }

        /// <summary>
        /// Gets the database name.
        /// </summary>
        public string DatabaseName { get; private set; }
    }
}
