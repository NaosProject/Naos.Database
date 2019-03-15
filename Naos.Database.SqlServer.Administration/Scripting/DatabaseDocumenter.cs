// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseDocumenter.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer.Administration
{
    using System.Globalization;

    using Microsoft.SqlServer.Management.Smo;

    using OBeautifulCode.Validation.Recipes;

    /// <summary>
    /// Documenter for database objects.
    /// </summary>
    public class DatabaseDocumenter
    {
        private readonly IDocumentGenerator documentGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseDocumenter"/> class.
        /// </summary>
        /// <param name="documentGenerator">Document generator to actually persist the information.</param>
        public DatabaseDocumenter(IDocumentGenerator documentGenerator)
        {
            this.documentGenerator = documentGenerator;
        }

        /// <summary>
        /// Document a <see cref="UserDefinedDataType" />.
        /// </summary>
        /// <param name="userDefinedDataType">Object to document.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Justification = "Specifically using a multi-dimensional array.")]
        public void Document(UserDefinedDataType userDefinedDataType)
        {
            new { userDefinedDataType }.Must().NotBeNull();

            string[,] values = new string[3, 4];
            int leftRowIndexer = 0;
            int rightRowIndexer = 0;
            values[leftRowIndexer++, 0] = "[[BOLD]]Max Length";
            values[leftRowIndexer++, 0] = "[[BOLD]]Variable Length";
            values[rightRowIndexer++, 2] = "[[BOLD]]Numeric Scale";
            values[rightRowIndexer++, 2] = "[[BOLD]]Numeric Precision";
            values[leftRowIndexer++, 0] = "[[BOLD]]Description";

            leftRowIndexer = rightRowIndexer = 0;

            values[leftRowIndexer++, 1] = userDefinedDataType.MaxLength.ToString(CultureInfo.CurrentCulture);
            values[leftRowIndexer++, 1] = userDefinedDataType.VariableLength.ToString(CultureInfo.CurrentCulture);
            values[rightRowIndexer++, 3] = userDefinedDataType.NumericScale.ToString(CultureInfo.CurrentCulture);
            values[rightRowIndexer++, 3] = userDefinedDataType.NumericPrecision.ToString(CultureInfo.CurrentCulture);
            values[leftRowIndexer++, 1] = string.Empty; // TODO: get trg.Description? if possible;

            int[,] merges = new int[2, 4];
            merges[0, 0] = 2;
            merges[0, 1] = 1;
            merges[0, 2] = 2;
            merges[0, 3] = 3;

            this.documentGenerator.AddTable(userDefinedDataType.Name, values, merges);
        }

        /// <summary>
        /// Document a <see cref="DatabaseRole" />.
        /// </summary>
        /// <param name="databaseRole">Object to document.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Justification = "Specifically using a multi-dimensional array.")]
        public void Document(DatabaseRole databaseRole)
        {
            new { databaseRole }.Must().NotBeNull();

            string[,] values = new string[1, 2];
            int rowIndexer = 0;
            values[rowIndexer++, 0] = "[[BOLD]]Description";

            rowIndexer = 0;
            values[rowIndexer++, 1] = string.Empty; // TODO: get rol.Description? if possible;

            int[,] merges = new int[0, 0];
            this.documentGenerator.AddTable(databaseRole.Name, values, merges);
        }

        /// <summary>
        /// Document a <see cref="User" />.
        /// </summary>
        /// <param name="user">Object to document.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Justification = "Specifically using a multi-dimensional array.")]
        public void Document(User user)
        {
            new { user }.Must().NotBeNull();

            string[,] values = new string[1, 2];
            int rowIndexer = 0;
            values[rowIndexer++, 0] = "[[BOLD]]Description";

            rowIndexer = 0;
            values[rowIndexer++, 1] = string.Empty; // TODO: get rol.Description? if possible;

            int[,] merges = new int[0, 0];
            this.documentGenerator.AddTable(user.Name, values, merges);
        }

        /// <summary>
        /// Document a <see cref="Index" />.
        /// </summary>
        /// <param name="index">Object to document.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Justification = "Specifically using a multi-dimensional array.")]
        public void Document(Index index)
        {
            new { index }.Must().NotBeNull();

            string[,] values = new string[4, 4];
            int leftRowIndexer = 0;
            int rightRowIndexer = 0;
            values[leftRowIndexer++, 0] = "[[W100]][[BOLD]]Indexed key type";
            values[rightRowIndexer++, 2] = "[[W80]][[BOLD]]Full text key";
            values[leftRowIndexer++, 0] = "[[W100]][[BOLD]]Clustered";
            values[rightRowIndexer++, 2] = "[[W80]][[BOLD]]Unique";
            values[leftRowIndexer++, 0] = "[[W100]][[BOLD]]Columns";
            values[leftRowIndexer++, 0] = "[[W100]][[BOLD]]Description";

            leftRowIndexer = rightRowIndexer = 0;

            values[leftRowIndexer++, 1] = "[[W100]]" + index.IndexKeyType.ToString();
            values[rightRowIndexer++, 3] = "[[W80]]" + index.IsFullTextKey.ToString();
            values[leftRowIndexer++, 1] = "[[W100]]" + index.IsClustered.ToString();
            values[rightRowIndexer++, 3] = "[[W80]]" + index.IsUnique.ToString();
            values[leftRowIndexer, 1] = "[[W100]]";
            bool first = true;
            string addComma = ", ";
            foreach (IndexedColumn col in index.IndexedColumns)
            {
                values[leftRowIndexer, 1] += (first ? "[[W100]]" : addComma) + col.Name;
                first = false;
            }

            leftRowIndexer++; // since it has to be referenced in the loop it can't be incremented inline like the rest
            values[leftRowIndexer++, 1] = string.Empty; // TODO: get trg.Description? if possible;

            int[,] merges = new int[2, 4];
            merges[0, 0] = 2;
            merges[0, 1] = 1;
            merges[0, 2] = 2;
            merges[0, 3] = 3;

            merges[1, 0] = 3;
            merges[1, 1] = 1;
            merges[1, 2] = 3;
            merges[1, 3] = 3;
            this.documentGenerator.AddTable(index.Name, values, merges);
        }

        /// <summary>
        /// Document a <see cref="Check" />.
        /// </summary>
        /// <param name="check">Object to document.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Justification = "Specifically using a multi-dimensional array.")]
        public void Document(Check check)
        {
            new { check }.Must().NotBeNull();

            string[,] values = new string[1, 2];
            int rowIndexer = 0;
            values[rowIndexer++, 0] = "[[BOLD]]Description";

            rowIndexer = 0;
            values[rowIndexer++, 1] = string.Empty; // TODO: get trg.Description? if possible;

            int[,] merges = new int[0, 0];
            this.documentGenerator.AddTable(check.Name, values, merges);
        }

        /// <summary>
        /// Document a <see cref="ParameterCollectionBase" />.
        /// </summary>
        /// <param name="parameters">Object to document.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Justification = "Specifically using a multi-dimensional array.")]
        public void Document(ParameterCollectionBase parameters)
        {
            new { parameters }.Must().NotBeNull();

            int rowCount = parameters.Count + 1; // 1 for the header
            string[,] values = new string[rowCount, 4];
            int[,] merges = new int[0, 0];
            int rowIndexer = 0;
            values[rowIndexer, 0] = "[[W150]][[BOLD]]Name";
            values[rowIndexer, 1] = "[[W80]][[BOLD]]Type (Size)";
            values[rowIndexer, 2] = "[[W50]][[BOLD]]Output";
            values[rowIndexer, 3] = "[[W120]][[BOLD]]Description";
            rowIndexer++;

            foreach (ParameterBase param in parameters)
            {
                string outputParamString = "N/A";
                var storedProcedureParameter = param as StoredProcedureParameter;
                if (storedProcedureParameter != null)
                {
                    outputParamString = storedProcedureParameter.IsOutputParameter.ToString();
                }

                values[rowIndexer, 0] = "[[W150]]" + param.Name;
                values[rowIndexer, 1] = "[[W80]]" + param.DataType.Name + " (" + param.DataType.MaximumLength.ToString(CultureInfo.CurrentCulture) + ")";
                values[rowIndexer, 2] = "[[W50]]" + outputParamString;
                values[rowIndexer, 3] = "[[W120]]";

                rowIndexer++;
            }

            this.documentGenerator.AddTable(string.Empty, values, merges);
        }

        /// <summary>
        /// Document a <see cref="Trigger" />.
        /// </summary>
        /// <param name="trigger">Object to document.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Justification = "Specifically using a multi-dimensional array.")]
        public void Document(Trigger trigger)
        {
            new { trigger }.Must().NotBeNull();

            string[,] values = new string[2, 2];
            int rowIndexer = 0;
            values[rowIndexer++, 0] = "[[BOLD]]Text Body";
            values[rowIndexer++, 0] = "[[BOLD]]Description";

            rowIndexer = 0;

            values[rowIndexer++, 1] = trigger.TextBody;
            values[rowIndexer++, 1] = string.Empty; // TODO: get trg.Description? if possible;

            int[,] merges = new int[0, 0];
            this.documentGenerator.AddTable(trigger.Name, values, merges);
        }

        /// <summary>
        /// Document a <see cref="StoredProcedure" />.
        /// </summary>
        /// <param name="storedProcedure">Object to document.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Justification = "Specifically using a multi-dimensional array.")]
        public void Document(StoredProcedure storedProcedure)
        {
            new { storedProcedure }.Must().NotBeNull();

            this.documentGenerator.AddEntry(storedProcedure.Name, 15, true);
            string[,] values = new string[1, 2];
            values[0, 0] = "[[BOLD]]Description";
            values[0, 1] = string.Empty; // TODO: add tab.description?;
            int[,] merges = new int[0, 0];
            this.documentGenerator.AddTable(string.Empty, values, merges);
        }

        /// <summary>
        /// Document a <see cref="UserDefinedFunction" />.
        /// </summary>
        /// <param name="userDefinedFunction">Object to document.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Justification = "Specifically using a multi-dimensional array.")]
        public void Document(UserDefinedFunction userDefinedFunction)
        {
            new { userDefinedFunction }.Must().NotBeNull();

            this.documentGenerator.AddEntry(userDefinedFunction.Name, 15, true);
            string[,] values = new string[2, 2];
            values[0, 0] = "[[BOLD]]Return Type (Size)";
            values[0, 1] = userDefinedFunction.DataType.Name + " (" + userDefinedFunction.DataType.MaximumLength + ")";
            values[1, 0] = "[[BOLD]]Description";
            values[1, 1] = string.Empty; // TODO: add tab.description?;
            int[,] merges = new int[0, 0];
            this.documentGenerator.AddTable(string.Empty, values, merges);
        }

        /// <summary>
        /// Document a <see cref="Table" />.
        /// </summary>
        /// <param name="table">Object to document.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Justification = "Specifically using a multi-dimensional array.")]
        public void Document(Table table)
        {
            new { table }.Must().NotBeNull();

            this.documentGenerator.AddEntry("Table - " + table.Name, 15, true);
            string[,] values = new string[1, 2];
            values[0, 0] = "[[BOLD]]Description";
            values[0, 1] = string.Empty; // TODO: add tab.description?;
            int[,] merges = new int[0, 0];
            this.documentGenerator.AddTable(string.Empty, values, merges);
        }

        /// <summary>
        /// Document a <see cref="View" />.
        /// </summary>
        /// <param name="view">Object to document.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Justification = "Specifically using a multi-dimensional array.")]
        public void Document(View view)
        {
            new { view }.Must().NotBeNull();

            this.documentGenerator.AddEntry(view.Name, 15, true);
            string[,] values = new string[1, 2];
            values[0, 0] = "[[BOLD]]Description";
            values[0, 1] = string.Empty; // TODO: add viw.description?;
            int[,] merges = new int[0, 0];
            this.documentGenerator.AddTable(string.Empty, values, merges);
        }

        /// <summary>
        /// Document a <see cref="ForeignKey" />.
        /// </summary>
        /// <param name="foreignKey">Object to document.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Justification = "Specifically using a multi-dimensional array.")]
        public void Document(ForeignKey foreignKey)
        {
            new { foreignKey }.Must().NotBeNull();

            string[,] values = new string[3, 4];

            int leftRowIndexer = 0;
            int rightRowIndexer = 0;
            values[leftRowIndexer++, 0] = "[[W100]][[BOLD]]Referenced Table";
            values[rightRowIndexer++, 2] = "[[W80]][[BOLD]]Referenced Key";
            values[leftRowIndexer++, 0] = "[[W100]][[BOLD]]Local Columns";
            values[leftRowIndexer++, 0] = "[[W100]][[BOLD]]Description";

            leftRowIndexer = rightRowIndexer = 0;

            values[leftRowIndexer++, 1] = "[[W100]]" + foreignKey.ReferencedTable;
            values[rightRowIndexer++, 3] = "[[W80]]" + foreignKey.ReferencedKey;

            values[leftRowIndexer, 1] = "[[W100]]";
            bool first = true;
            string addComma = ", ";
            foreach (ForeignKeyColumn col in foreignKey.Columns)
            {
                values[leftRowIndexer, 1] += (first ? string.Empty : addComma) + col.Name;
                first = false;
            }

            leftRowIndexer++; // since the row has to be referenced after more than once can't be incremented inline

            values[leftRowIndexer++, 1] = "[[W100]]"; // TODO: get working col.Description?;

            int[,] merges = new int[2, 4];
            merges[0, 0] = 1;
            merges[0, 1] = 1;
            merges[0, 2] = 2;
            merges[0, 3] = 3;

            merges[1, 0] = 2;
            merges[1, 1] = 1;
            merges[1, 2] = 2;
            merges[1, 3] = 3;

            this.documentGenerator.AddTable(foreignKey.Name, values, merges);
        }

        /// <summary>
        /// Document a <see cref="ColumnCollection" />.
        /// </summary>
        /// <param name="columns">Object to document.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body", Justification = "Specifically using a multi-dimensional array.")]
        public void Document(ColumnCollection columns)
        {
            new { columns }.Must().NotBeNull();

            int rowCount = columns.Count + 1; // 1 for the header
            string[,] values = new string[rowCount, 7];
            int[,] merges = new int[0, 0];
            int rowIndexer = 0;
            values[rowIndexer, 0] = "[[W100]][[BOLD]]Name";
            values[rowIndexer, 1] = "[[W70]][[BOLD]]Type (Size)";
            values[rowIndexer, 2] = "[[W50]][[BOLD]]Nullable";
            values[rowIndexer, 3] = "[[W50]][[BOLD]]Identity";
            values[rowIndexer, 4] = "[[W70]][[BOLD]]Primary Key";
            values[rowIndexer, 5] = "[[W50]][[BOLD]]Default";
            values[rowIndexer, 6] = "[[W40]][[BOLD]]Notes";
            rowIndexer++;
            foreach (Column col in columns)
            {
                values[rowIndexer, 0] = "[[W100]]" + col.Name;
                values[rowIndexer, 1] = "[[W70]]" + col.DataType.Name + " (" + col.DataType.MaximumLength.ToString(CultureInfo.CurrentCulture) + ")";
                values[rowIndexer, 2] = "[[W50]]" + col.Nullable.ToString();
                values[rowIndexer, 3] = "[[W50]]" + col.Identity.ToString();
                values[rowIndexer, 4] = "[[W70]]" + col.InPrimaryKey.ToString();
                values[rowIndexer, 5] = "[[W50]]" + ((col.DefaultConstraint != null) ? col.DefaultConstraint.Text : string.Empty);
                values[rowIndexer, 6] = "[[W40]]"; // empty string is contents
                rowIndexer++;
            }

            this.documentGenerator.AddTable(string.Empty, values, merges);
        }
    }
}
