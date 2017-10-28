// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnDescription.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Detailed information about the column.
    /// </summary>
    public class ColumnDescription
    {
        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Gets or sets the ordinal position of the column.
        /// </summary>
        public int OrdinalPosition { get; set; }

        /// <summary>
        /// Gets or sets the default value of the column.
        /// </summary>
        public string ColumnDefault { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the column is nullable.
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Gets or sets the data type of the column.
        /// </summary>
        public string DataType { get; set; }
    }
}
