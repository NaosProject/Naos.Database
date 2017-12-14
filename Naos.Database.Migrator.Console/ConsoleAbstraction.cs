// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsoleAbstraction.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Migrator.Console
{
    using System;
    using System.IO;
    using System.Reflection;

    using CLAP;

    using Naos.Database.Migrator;
    using Naos.Logging.Domain;

    using OBeautifulCode.Reflection.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// The wrapper class for CLAP that is responsible for performing the act of migrating a database.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors", Justification = "Gets newed up by CLAP.")]
    public class ConsoleAbstraction : ConsoleAbstractionBase
    {
        /// <summary>
        /// Migrate up to a version.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="databaseName">The database name to target.</param>
        /// <param name="assemblyPath">The path to the assembly that contains the migration.</param>
        /// <param name="timeoutInSeconds">The command timeout (in seconds) for the command(s) executed as part of the migration.</param>
        /// <param name="applicationContext">Optional application context.</param>
        /// <param name="targetVersion">The version to migrate to.</param>
        /// <param name="dependentAssemblyPath">Optional path to load depedendant assemblies from, default is none.</param>
        /// <param name="debug">Optional indication to launch the debugger from inside the application (default is false).</param>
        /// <param name="environment">Optional value to use when setting the Its.Configuration precedence to use specific settings.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFile", Justification = "Need to load the file.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "assemblyPath", Justification = "Spelling/name is correct.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "targetVersion", Justification = "Spelling/name is correct.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "connectionString", Justification = "Spelling/name is correct.")]
        [Verb(Aliases = "Up", Description = "Perform a migration up.")]
        public static void Up(
            [Required] [Aliases("")] [Description("The connection string to the database.")] string connectionString,
            [Required] [Aliases("")] [Description("The database name to target.")] string databaseName,
            [Required] [Aliases("")] [Description("The path to the assembly that contains the migration.")] string assemblyPath,
            [DefaultValue(30)] [Aliases("")] [Description("The command timeout (in seconds) for the command(s) executed as part of the migration.")] int timeoutInSeconds,
            [DefaultValue(null)] [Aliases("")] [Description("Optional application context.")] string applicationContext,
            [DefaultValue(null)] [Aliases("")] [Description("Optional version to migrate to, default is latest.")] long? targetVersion,
            [DefaultValue(null)] [Aliases("")] [Description("Optional path to load depedendant assemblies from, default is none.")] string dependentAssemblyPath,
            [Aliases("")] [Description("Launches the debugger.")] [DefaultValue(false)] bool debug,
            [Aliases("")] [Description("Sets the Its.Configuration precedence to use specific settings.")] [DefaultValue(null)] string environment)
        {
            CommonSetup(debug, environment, new LogProcessorSettings(new[] { new ConsoleLogConfiguration(LogContexts.All, LogContexts.None) }));

            var timeout = TimeSpan.FromSeconds(timeoutInSeconds);

            PrintArguments(new { targetVersion, connectionString, assemblyPath, timeoutInSeconds, timeout, dependentAssemblyPath });

            if (!File.Exists(assemblyPath))
            {
                throw new ArgumentException("Path to migration assembly: " + assemblyPath + " does not exist.", nameof(assemblyPath));
            }

            if (!string.IsNullOrEmpty(dependentAssemblyPath))
            {
                // need to run loose because FluentMigrator doesn't play nice...
                using (var loader = AssemblyLoader.CreateAndLoadFromDirectory(dependentAssemblyPath, suppressFileLoadException: true, suppressBadImageFormatException: true))
                {
                    foreach (var fileToAssembly in loader.FilePathToAssemblyMap)
                    {
                        Console.WriteLine(Invariant($"Loaded - {fileToAssembly.Key} - {fileToAssembly.Value}"));
                    }

                    var assembly = loader.FilePathToAssemblyMap[assemblyPath];
                    MigrationExecutor.Up(assembly, connectionString, databaseName, targetVersion, Console.WriteLine, timeout, applicationContext);
                }
            }
            else
            {
                var assembly = Assembly.LoadFile(assemblyPath);
                MigrationExecutor.Up(assembly, connectionString, databaseName, targetVersion, Console.WriteLine, timeout, applicationContext);
            }
        }

        /// <summary>
        /// Migrate down to a version.
        /// </summary>
        /// <param name="connectionString">The connection string to the database.</param>
        /// <param name="databaseName">The database name to target.</param>
        /// <param name="assemblyPath">The path to the assembly that contains the migration.</param>
        /// <param name="timeoutInSeconds">The command timeout (in seconds) for the command(s) executed as part of the migration.</param>
        /// <param name="applicationContext">Optional application context.</param>
        /// <param name="targetVersion">The version to migrate to.</param>
        /// <param name="dependentAssemblyPath">Optional path to load depedendant assemblies from, default is none.</param>
        /// <param name="debug">Optional indication to launch the debugger from inside the application (default is false).</param>
        /// <param name="environment">Optional value to use when setting the Its.Configuration precedence to use specific settings.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFile", Justification = "Need to load the file.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "dependentAssemblyPath", Justification = "Spelling/name is correct.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "assemblyPath", Justification = "Spelling/name is correct.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "connectionString", Justification = "Spelling/name is correct.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "targetVersion", Justification = "Spelling/name is correct.")]
        [Verb(Aliases = "Down", Description = "Perform a migration down.")]
        public static void Down(
            [Required] [Aliases("")] [Description("The connection string to the database.")] string connectionString,
            [Required] [Aliases("")] [Description("The database name to target.")] string databaseName,
            [Required] [Aliases("")] [Description("The path to the assembly that contains the migration.")] string assemblyPath,
            [DefaultValue(30)] [Aliases("")] [Description("The command timeout (in seconds) for the command(s) executed as part of the migration.")] int timeoutInSeconds,
            [DefaultValue(null)] [Aliases("")] [Description("Optional application context.")] string applicationContext,
            [Required] [Aliases("")] [Description("The version to migrate to.")] long targetVersion,
            [DefaultValue(null)] [Aliases("")] [Description("Optional path to load depedendant assemblies from, default is none.")] string dependentAssemblyPath,
            [Aliases("")] [Description("Launches the debugger.")] [DefaultValue(false)] bool debug,
            [Aliases("")] [Description("Sets the Its.Configuration precedence to use specific settings.")] [DefaultValue(null)] string environment)
        {
            CommonSetup(debug, environment, new LogProcessorSettings(new[] { new ConsoleLogConfiguration(LogContexts.All, LogContexts.None) }));

            var timeout = TimeSpan.FromSeconds(timeoutInSeconds);

            PrintArguments(new { targetVersion, connectionString, assemblyPath, timeoutInSeconds, timeout, dependentAssemblyPath });

            if (!File.Exists(assemblyPath))
            {
                throw new ArgumentException("Path to migration assembly: " + assemblyPath + " does not exist.", nameof(assemblyPath));
            }

            Console.WriteLine(string.Empty);

            if (!string.IsNullOrEmpty(dependentAssemblyPath))
            {
                // need to run loose because FluentMigrator doesn't play nice...
                using (var loader = AssemblyLoader.CreateAndLoadFromDirectory(dependentAssemblyPath, suppressFileLoadException: true, suppressBadImageFormatException: true))
                {
                    foreach (var fileToAssembly in loader.FilePathToAssemblyMap)
                    {
                        Console.WriteLine(Invariant($"Loaded - {fileToAssembly.Key} - {fileToAssembly.Value}"));
                    }

                    var assembly = loader.FilePathToAssemblyMap[assemblyPath];
                    MigrationExecutor.Down(assembly, connectionString, databaseName, targetVersion, Console.WriteLine, timeout, applicationContext);
                }
            }
            else
            {
                var assembly = Assembly.LoadFile(assemblyPath);
                MigrationExecutor.Down(assembly, connectionString, databaseName, targetVersion, Console.WriteLine, timeout, applicationContext);
            }
        }
    }
}