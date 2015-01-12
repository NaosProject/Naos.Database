// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableDescription.cs" company="Naos">
//   Copyright 2014 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Utils.Database.Tools
{
    using System.Globalization;

    /// <summary>
    /// Detailed information about the table.
    /// </summary>
    public class TableDescription
    {
        #region Properties

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
        public ColumnDescription[] Columns { get; set; }

        #endregion
    }
}
