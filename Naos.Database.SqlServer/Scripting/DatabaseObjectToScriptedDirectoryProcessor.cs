// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseObjectToScriptedDirectoryProcessor.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.SqlServer.Management.Smo;

    using Spritely.Recipes;

    /// <summary>
    /// Manage processing objects to disk along with documentation if applicable.
    /// </summary>
    public class DatabaseObjectToScriptedDirectoryProcessor
    {
        private const string FileExtensionStoredProcedure = ".prc";
        private const string FileExtensionTable = ".tab";
        private const string FileExtensionFunction = ".fnc";
        private const string FileExtensionRole = ".sql";
        private const string FileExtensionView = ".viw";
        private const string FileExtensionIndex = ".idx";
        private const string FileExtensionTrigger = ".trg";
        private const string FileExtensionChecks = ".sql";
        private const string FileExtensionForeignKey = ".fky";
        private const string FileExtensionUser = ".usr";
        private const string FileExtensionUserDefinedDataType = ".udt";

        private readonly List<string> createdDirectories = new List<string>();

        private readonly IDocumentGenerator documentGenerator;
        private readonly DatabaseDocumenter databaseDocumenter;

        private readonly string tablePath;
        private readonly string sprocPath;
        private readonly string securityPath;
        private readonly string funcPath;
        private readonly string viewPath;
        private readonly string userDefinedDataTypePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseObjectToScriptedDirectoryProcessor"/> class.
        /// </summary>
        /// <param name="documentGenerator">Document generator to use.</param>
        /// <param name="basePath">Path to write output.</param>
        public DatabaseObjectToScriptedDirectoryProcessor(IDocumentGenerator documentGenerator, string basePath)
        {
            new { documentGenerator }.Must().NotBeNull().OrThrowFirstFailure();
            new { basePath }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();

            this.documentGenerator = documentGenerator;
            this.databaseDocumenter = new DatabaseDocumenter(this.documentGenerator);
            this.tablePath = this.CreateDatabaseObjectDirectory("table", basePath);
            this.sprocPath = this.CreateDatabaseObjectDirectory("sproc", basePath);
            this.funcPath = this.CreateDatabaseObjectDirectory("function", basePath);
            this.viewPath = this.CreateDatabaseObjectDirectory("view", basePath);
            this.securityPath = this.CreateDatabaseObjectDirectory("security", basePath);
            this.userDefinedDataTypePath = this.CreateDatabaseObjectDirectory("type", basePath);
        }

        private string CreateDatabaseObjectDirectory(string childFolderName, string basePath)
        {
            string ret = Path.Combine(basePath, childFolderName);
            if (!Directory.Exists(ret))
            {
                Directory.CreateDirectory(ret);
                this.createdDirectories.Add(ret);
            }

            return ret;
        }

        /// <summary>
        /// Process tables.
        /// </summary>
        /// <param name="tables">Object to process.</param>
        public void Process(TableCollection tables)
        {
            new { tables }.Must().NotBeNull().OrThrowFirstFailure();

            if (tables.Count > 0)
            {
                this.documentGenerator.AddEntry("TABLES", 16, true);
            }

            this.documentGenerator.Indent();

            foreach (Table table in tables)
            {
                if (!table.IsSystemObject)
                {
                    string tableName = table.Name;
                    this.databaseDocumenter.Document(table);

                    ScriptAndWriteToFile(table, this.tablePath, FileExtensionTable);

                    this.Process(table.Columns, tableName);
                    this.Process(table.Indexes, tableName, this.tablePath);
                    this.Process(table.Triggers, tableName, this.tablePath);

                    this.Process(table.ForeignKeys, tableName);
                    this.Process(table.Checks, tableName);
                }
            }

            this.documentGenerator.Undent();
        }

        /// <summary>
        /// Process views.
        /// </summary>
        /// <param name="views">Object to process.</param>
        public void Process(ViewCollection views)
        {
            new { views }.Must().NotBeNull().OrThrowFirstFailure();

            if (views.Count > 0)
            {
                this.documentGenerator.AddEntry("VIEWS", 16, true);
            }

            this.documentGenerator.Indent();
            foreach (View view in views)
            {
                if (!view.IsSystemObject)
                {
                    string viewName = view.Name;
                    this.databaseDocumenter.Document(view);
                    ScriptAndWriteToFile(view, this.viewPath, FileExtensionView);

                    this.Process(view.Columns, viewName);
                    this.Process(view.Indexes, viewName, this.viewPath);
                    this.Process(view.Triggers, viewName, this.viewPath);
                }
            }

            this.documentGenerator.Undent();
        }

        /// <summary>
        /// Process stored procedures.
        /// </summary>
        /// <param name="storedProcedures">Object to process.</param>
        public void Process(StoredProcedureCollection storedProcedures)
        {
            new { storedProcedures }.Must().NotBeNull().OrThrowFirstFailure();

            if (storedProcedures.Count > 0)
            {
                this.documentGenerator.AddEntry("STORED PROCEDURES", 16, true);
            }

            this.documentGenerator.Indent();
            foreach (StoredProcedure storedProcedure in storedProcedures)
            {
                if (!storedProcedure.IsSystemObject)
                {
                    this.databaseDocumenter.Document(storedProcedure);
                    ScriptAndWriteToFile(storedProcedure, this.sprocPath, FileExtensionStoredProcedure);
                    if (storedProcedure.Parameters.Count > 0)
                    {
                        this.documentGenerator.AddEntry("Parameters", 12, true);
                    }

                    this.documentGenerator.Indent();
                    this.databaseDocumenter.Document(storedProcedure.Parameters);
                    this.documentGenerator.Undent();
                }
            }

            this.documentGenerator.Undent();
        }

        /// <summary>
        /// Process user defined functions.
        /// </summary>
        /// <param name="userDefinedFunctions">Object to process.</param>
        public void Process(UserDefinedFunctionCollection userDefinedFunctions)
        {
            new { userDefinedFunctions }.Must().NotBeNull().OrThrowFirstFailure();

            if (userDefinedFunctions.Count > 0)
            {
                this.documentGenerator.AddEntry("USER DEFINED FUNCTIONS", 16, true);
            }

            this.documentGenerator.Indent();
            foreach (UserDefinedFunction userDefinedFunction in userDefinedFunctions)
            {
                if (!userDefinedFunction.IsSystemObject)
                {
                    this.databaseDocumenter.Document(userDefinedFunction);
                    ScriptAndWriteToFile(userDefinedFunction, this.funcPath, FileExtensionFunction);
                    if (userDefinedFunction.Parameters.Count > 0)
                    {
                        this.documentGenerator.AddEntry("Parameters", 12, true);
                    }

                    this.documentGenerator.Indent();
                    this.databaseDocumenter.Document(userDefinedFunction.Parameters);
                    this.documentGenerator.Undent();
                }
            }

            this.documentGenerator.Undent();
        }

        /// <summary>
        /// Process roles.
        /// </summary>
        /// <param name="roles">Object to process.</param>
        public void Process(DatabaseRoleCollection roles)
        {
            new { roles }.Must().NotBeNull().OrThrowFirstFailure();

            if (roles.Count > 0)
            {
                this.documentGenerator.AddEntry("ROLES", 16, true);
            }

            this.documentGenerator.Indent();
            foreach (DatabaseRole role in roles)
            {
                if (!role.IsFixedRole && role.Name != "public")
                {
                    this.databaseDocumenter.Document(role);
                    ScriptAndWriteToFile(role, this.securityPath, FileExtensionRole);
                }
            }

            this.documentGenerator.Undent();
        }

        /// <summary>
        /// Process columns.
        /// </summary>
        /// <param name="columns">Object to process.</param>
        /// <param name="tableOrViewName">Name of table or view containing columns.</param>
        public void Process(ColumnCollection columns, string tableOrViewName)
        {
            new { columns }.Must().NotBeNull().OrThrowFirstFailure();

            if (columns.Count > 0)
            {
                this.documentGenerator.AddEntry("Columns on table - " + tableOrViewName, 12, true);
            }

            this.documentGenerator.Indent();
            this.databaseDocumenter.Document(columns);
            this.documentGenerator.Undent();
        }

        /// <summary>
        /// Process indexes.
        /// </summary>
        /// <param name="indexes">Object to process.</param>
        /// <param name="tableOrViewName">Name of table or view containing indexes.</param>
        /// <param name="fileBasePath">FIle path to write to (might differ between view and table).</param>
        public void Process(IndexCollection indexes, string tableOrViewName, string fileBasePath)
        {
            new { indexes }.Must().NotBeNull().OrThrowFirstFailure();
            new { tableName = tableOrViewName }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();
            new { fileBasePath }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();

            var filtered = FilterPrimaryUniqueKeys(indexes);
            if (filtered.Count > 0)
            {
                this.documentGenerator.AddEntry("Indexes on table - " + tableOrViewName, 12, true);
            }

            this.documentGenerator.Indent();
            foreach (Index index in filtered)
            {
                if (!index.IsSystemObject)
                {
                    this.databaseDocumenter.Document(index);
                    ScriptAndWriteToFile(index, fileBasePath, FileExtensionIndex);
                }
            }

            this.documentGenerator.Undent();
        }

        /// <summary>
        /// Process triggers.
        /// </summary>
        /// <param name="triggers">Object to process.</param>
        /// <param name="tableOrViewName">Name of table or view containing triggers.</param>
        /// <param name="fileBasePath">FIle path to write to (might differ between view and table).</param>
        public void Process(TriggerCollection triggers, string tableOrViewName, string fileBasePath)
        {
            new { triggers }.Must().NotBeNull().OrThrowFirstFailure();
            new { tableName = tableOrViewName }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();
            new { fileBasePath }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();

            if (triggers.Count > 0)
            {
                this.documentGenerator.AddEntry("Triggers on table - " + tableOrViewName, 12, true);
            }

            this.documentGenerator.Indent();
            foreach (Trigger trigger in triggers)
            {
                if (!trigger.IsSystemObject)
                {
                    this.databaseDocumenter.Document(trigger);
                    ScriptAndWriteToFile(trigger, fileBasePath, FileExtensionTrigger);
                }
            }

            this.documentGenerator.Undent();
        }

        /// <summary>
        /// Process checks.
        /// </summary>
        /// <param name="checks">Object to process.</param>
        /// <param name="tableName">Name of table or view containing checks.</param>
        public void Process(CheckCollection checks, string tableName)
        {
            new { checks }.Must().NotBeNull().OrThrowFirstFailure();
            new { tableName }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();

            if (checks.Count > 0)
            {
                this.documentGenerator.AddEntry("Checks for table - " + tableName, 12, true);
            }

            this.documentGenerator.Indent();
            foreach (Check check in checks)
            {
                this.databaseDocumenter.Document(check);
                ScriptAndWriteToFile(check, this.tablePath, FileExtensionChecks);
            }

            this.documentGenerator.Undent();
        }

        /// <summary>
        /// Process foreign keys.
        /// </summary>
        /// <param name="foreignKeys">Object to process.</param>
        /// <param name="tableName">Name of table foreign keys.</param>
        public void Process(ForeignKeyCollection foreignKeys, string tableName)
        {
            new { foreignKeys }.Must().NotBeNull().OrThrowFirstFailure();
            new { tableName }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();

            if (foreignKeys.Count > 0)
            {
                this.documentGenerator.AddEntry("Foreign Keys for table - " + tableName, 12, true);
            }

            this.documentGenerator.Indent();
            foreach (ForeignKey foreignKey in foreignKeys)
            {
                this.databaseDocumenter.Document(foreignKey);
                ScriptAndWriteToFile(foreignKey, this.tablePath, FileExtensionForeignKey);
            }

            this.documentGenerator.Undent();
        }

        /// <summary>
        /// Process users.
        /// </summary>
        /// <param name="users">Object to process.</param>
        public void Process(UserCollection users)
        {
            new { users }.Must().NotBeNull().OrThrowFirstFailure();

            if (users.Count > 0)
            {
                this.documentGenerator.AddEntry("USERS", 16, true);
            }

            this.documentGenerator.Indent();
            foreach (User user in users)
            {
                if (!user.IsSystemObject)
                {
                    this.databaseDocumenter.Document(user);
                    ScriptAndWriteToFile(user, this.securityPath, FileExtensionUser);
                }
            }

            this.documentGenerator.Undent();
        }

        /// <summary>
        /// Process user defined data types.
        /// </summary>
        /// <param name="userDefinedDataTypes">Object to process.</param>
        public void Process(UserDefinedDataTypeCollection userDefinedDataTypes)
        {
            new { userDefinedDataTypes }.Must().NotBeNull().OrThrowFirstFailure();

            if (userDefinedDataTypes.Count > 0)
            {
                this.documentGenerator.AddEntry("USER DEFINED DATA TYPES", 16, true);
            }

            this.documentGenerator.Indent();
            foreach (UserDefinedDataType userDefinedDataType in userDefinedDataTypes)
            {
                this.databaseDocumenter.Document(userDefinedDataType);
                ScriptAndWriteToFile(userDefinedDataType, this.userDefinedDataTypePath, FileExtensionUserDefinedDataType);
            }

            this.documentGenerator.Undent();
        }

        private static ICollection<Index> FilterPrimaryUniqueKeys(IndexCollection idxs)
        {
            var ret = new List<Index>(idxs.Count);

            foreach (Index idx in idxs)
            {
                // these are scripted with tables
                if (idx.IndexKeyType != IndexKeyType.DriPrimaryKey && idx.IndexKeyType != IndexKeyType.DriUniqueKey)
                {
                    ret.Add(idx);
                }
            }

            return ret;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "FilePathAndName", Justification = "Spelling/name is correct.")]
        private static void ScriptAndWriteToFile(IScriptable scriptableObject, string basePath, string extensionWithPeriod)
        {
            string script = SqlObjectScripter.Script(scriptableObject);
            if (!string.IsNullOrEmpty(script))
            {
                string fileName = "[Not assigned]";
                string filePathAndName = "[Not assigned]";
                try
                {
                    fileName = ((NamedSmoObject)scriptableObject).Name.Replace(@"\", "--"); // for domain names mainly, making sure to not mess up file path
                    fileName = fileName.Replace(":", "_-COLON-_");
                    filePathAndName = Path.Combine(basePath, fileName);
                    filePathAndName = Path.ChangeExtension(filePathAndName, extensionWithPeriod);
                    using (TextWriter writer = new StreamWriter(filePathAndName, false))
                    {
                        writer.WriteLine(script);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception encountered.");
                    Console.WriteLine("FileName: {0} FilePathAndName: {1}", fileName, filePathAndName);
                    Console.WriteLine(e);
                }
            }
        }

        /// <summary>
        /// Remove directories in output that never had objects placed in them.
        /// </summary>
        public void CleanUpEmptyDirectories()
        {
            foreach (string path in this.createdDirectories)
            {
                if (Directory.GetFiles(path).Length == 0 && Directory.GetDirectories(path).Length == 0)
                {
                    Console.WriteLine("Removing {0}", path);
                    Directory.Delete(path);
                }
            }
        }
    }
}
