// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Scripter.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;

    using Microsoft.SqlServer.Management.Common;
    using Microsoft.SqlServer.Management.Smo;

    using Naos.Database.Domain;

    using Spritely.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Will create a directory under root for the database, will script everything possible under the appropriate of three directories; script, sproc, table.
    /// (If not user and password are provided, will use trusted authentication.)
    /// </summary>
    public static class Scripter
    {
        /// <summary>
        /// Script a database to a provided path.
        /// </summary>
        /// <param name="connectionString">Connection to database to find objects in.</param>
        /// <param name="outputFilePath">Path to write files to.</param>
        /// <param name="announcer">Optional announcer to log messages; DEFAULT is null.</param>
        /// <param name="generateDocument">Value indicating whether to produce a document in the output path.</param>
        /// <param name="logServerInfoMessages">Optional value indicating whether or not to wire up <see cref="Its.Log" /> to the <see cref="SqlConnection.InfoMessage" /> event; DEFAULT is true.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "Disposed correctly.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed correctly.")]
        public static void ScriptDatabaseToFilePath(string connectionString, string outputFilePath, Action<string> announcer = null, bool generateDocument = false, bool logServerInfoMessages = true)
        {
            void NullAnnouncer(string message)
            {
                /* no-op */
            }

            var localAnnouncer = announcer ?? NullAnnouncer;

            void Logic(Database database)
            {
                var documentPath = Path.Combine(outputFilePath, database.Name);
                var databasePath = Path.Combine(documentPath, "schema");

                IDocumentGenerator documentGenerator = new NullDocumentGenerator();
                if (generateDocument)
                {
                    if (!Directory.Exists(documentPath))
                    {
                        Directory.CreateDirectory(documentPath);
                    }

                    documentGenerator = new DocumentGenerator(Path.Combine(documentPath, database.Name + "-Documentation.doc"));
                }

                using (documentGenerator)
                {
                    ScriptObjects(database, documentGenerator, databasePath, localAnnouncer);
                    documentGenerator.Close();
                }
            }

            SqlServerDatabaseManager.RunOperationOnSmoDatabase(Logic, connectionString, logServerInfoMessages);
        }

        /// <summary>
        /// Script the objects matching the provided list of names.
        /// </summary>
        /// <param name="connectionString">Connection to database to find objects in.</param>
        /// <param name="objectNames">Names of objects to script.</param>
        /// <param name="logServerInfoMessages">Optional value indicating whether or not to wire up <see cref="Its.Log" /> to the <see cref="SqlConnection.InfoMessage" /> event; DEFAULT is true.</param>
        /// <param name="scrubScript">Value indicating whether or not to scrub the script to make it more readable and remove issues that prvent running.</param>
        /// <returns>Collection of scripted objects matching the provided names.</returns>
        public static IReadOnlyList<ScriptedObject> ScriptObjectsFromDatabase(string connectionString, IReadOnlyCollection<string> objectNames, bool logServerInfoMessages = true, bool scrubScript = true)
        {
            var ret = new List<ScriptedObject>();
            void Logic(Database database)
            {
                var allScriptableObjects = database.GetAllScriptableObjects();
                var filtered = allScriptableObjects.Where(_ => objectNames.Contains(_.Name)).ToList();
                ret = filtered.Select(_ => SqlObjectScripter.ScriptToObject(_, scrubScript)).ToList();
            }

            SqlServerDatabaseManager.RunOperationOnSmoDatabase(Logic, connectionString, logServerInfoMessages);
            return ret;
        }

        private static void ScriptObjects(Database database, IDocumentGenerator documentGenerator, string databasePath, Action<string> announcer)
        {
            DatabaseObjectToScriptedDirectoryProcessor databaseObjectToScriptedDirectoryProcessor = new DatabaseObjectToScriptedDirectoryProcessor(documentGenerator, databasePath);

            documentGenerator.AddEntry(database.Name + " - Object Documentation", 18, true, Alignment.Center);

            announcer(">Tables");
            databaseObjectToScriptedDirectoryProcessor.Process(database.Tables);
            announcer("<Tables");

            announcer(">Views");
            databaseObjectToScriptedDirectoryProcessor.Process(database.Views);
            announcer(">Views");

            announcer(">Roles");
            databaseObjectToScriptedDirectoryProcessor.Process(database.Roles);
            announcer("<Roles");

            announcer(">Stored Procedures");
            databaseObjectToScriptedDirectoryProcessor.Process(database.StoredProcedures);
            announcer("<Stored Procedures");

            announcer(">User Defined Functions");
            databaseObjectToScriptedDirectoryProcessor.Process(database.UserDefinedFunctions);
            announcer("<User Defined Functions");

            announcer(">User Defined Data Types");
            databaseObjectToScriptedDirectoryProcessor.Process(database.UserDefinedDataTypes);
            announcer("<User Defined Data Types");

            announcer(">Users");
            databaseObjectToScriptedDirectoryProcessor.Process(database.Users);
            announcer("<Users");

            announcer(">Remove of empty directories");
            databaseObjectToScriptedDirectoryProcessor.CleanUpEmptyDirectories();
            announcer("<Remove of empty directories");
        }

        /// <summary>
        /// Extension method on <see cref="Database" /> to get scriptable objects.
        /// </summary>
        /// <param name="database">Database to get objects from.</param>
        /// <returns>Collection of scriptable objects.</returns>
        public static IReadOnlyCollection<ScriptableObject> GetAllScriptableObjects(this Database database)
        {
            new { database }.Must().NotBeNull().OrThrowFirstFailure();

            var ret = new List<ScriptableObject>();

            foreach (Table table in database.Tables)
            {
                ret.Add(new ScriptableObject(table));

                foreach (ForeignKey foreignKey in table.ForeignKeys)
                {
                    ret.Add(new ScriptableObject(foreignKey));
                }

                foreach (Index index in table.Indexes)
                {
                    ret.Add(new ScriptableObject(index));
                }
            }

            foreach (View view in database.Views)
            {
                ret.Add(new ScriptableObject(view));

                foreach (Index index in view.Indexes)
                {
                    ret.Add(new ScriptableObject(index));
                }
            }

            foreach (StoredProcedure storedProcedure in database.StoredProcedures)
            {
                ret.Add(new ScriptableObject(storedProcedure));
            }

            foreach (UserDefinedFunction userDefinedFunction in database.UserDefinedFunctions)
            {
                ret.Add(new ScriptableObject(userDefinedFunction));
            }

            foreach (UserDefinedDataType userDefinedDataType in database.UserDefinedDataTypes)
            {
                ret.Add(new ScriptableObject(userDefinedDataType));
            }

            foreach (DatabaseRole databaseRole in database.Roles)
            {
                ret.Add(new ScriptableObject(databaseRole));
            }

            foreach (User user in database.Users)
            {
                ret.Add(new ScriptableObject(user));
            }

            return ret;
        }
    }
}
