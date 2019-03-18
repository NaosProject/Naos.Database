// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableDescription.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer.Domain
{
    using System.Collections.Generic;

    /// <summary>
    /// Detailed information about the table.
    /// </summary>
    public class TableDescription
    {
        /// <summary>
        /// Gets or sets the name of the database the table is from.
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the schema of the table.
        /// </summary>
        public string TableSchema { get; set; }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the columns descriptions of the table.
        /// </summary>
        public IReadOnlyCollection<ColumnDescription> Columns { get; set; }
    }
}
