﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MigratorConsoleHarness.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.MigratorHarness
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using CLAP;

    using Naos.Database.Migrator;

    using OBeautifulCode.Libs.Collections;

    /// <summary>
    /// The wrapper class for CLAP that is responsible for performing the act of migrating a database.
    /// </summary>
    public class MigratorConsoleHarness
    {
        /// <summary>
        /// The entry point to run migration up.
        /// </summary>
        /// <param name="connectionString">Connection string to the target database.</param>
        /// <param name="databaseName">Database name to target.</param>
        /// <param name="assemblyPath">Path to the assembly that the migration lives in.</param>
        /// <param name="timeoutInSeconds">Command timeout (in seconds) for the command(s) executed as part of the migration.</param>
        /// <param name="applicationContext">Optional application context.</param>
        /// <param name="targetVersion">Optional version to migrate up to, default is latest.</param>
        [Verb(Aliases = "Up", Description = "Perform a migration up.")]
        public static void Up(
            [Required] [Aliases("")] [Description("The connection string to the database.")] string connectionString,
            [Required] [Aliases("")] [Description("The database name to target.")] string databaseName,
            [Required] [Aliases("")] [Description("The path to the assembly that contains the migration.")] string assemblyPath,
            [DefaultValue(30)] [Aliases("")] [Description("The command timeout (in seconds) for the command(s) executed as part of the migration.")] int timeoutInSeconds,
            [DefaultValue(null)] [Aliases("")] [Description("Optional application context.")] string applicationContext,
            [DefaultValue(null)] [Aliases("")] [Description("Optional version to migrate to, default is latest.")] long? targetVersion)
        {
            if (!File.Exists(assemblyPath))
            {
                throw new ArgumentException("Path to migration assembly: " + assemblyPath + " does not exist.", "assemblyPath");
            }

            var assembly = Assembly.LoadFile(assemblyPath);
            var timeout = TimeSpan.FromSeconds(timeoutInSeconds);

            Console.WriteLine("PARAMETERS:");
            Console.WriteLine("             targetVersion: " + targetVersion);
            Console.WriteLine("          connectionString: " + connectionString);
            Console.WriteLine("              assemblyPath: " + assemblyPath);
            Console.WriteLine("                  assembly: " + assembly);
            Console.WriteLine("        timeout in seconds: " + timeoutInSeconds);
            Console.WriteLine("                   timeout: " + timeout);
            Console.WriteLine(string.Empty);

            MigrationExecutor.Up(assembly, connectionString, databaseName, targetVersion, Console.WriteLine, timeout, applicationContext);

            Console.WriteLine("Done");
        }

        /// <summary>
        /// The entry point to run migration down.
        /// </summary>
        /// <param name="connectionString">The connection string to the target database.</param>
        /// <param name="databaseName">The database name to target.</param>
        /// <param name="assemblyPath">The path to the assembly that the migration lives in.</param>
        /// <param name="timeoutInSeconds">The command timeout (in seconds) for the command(s) executed as part of the migration.</param>
        /// <param name="applicationContext">Optional application context.</param>
        /// <param name="targetVersion">The version to migrate up to.</param>
        [Verb(Aliases = "Down", Description = "Perform a migration down.")]
        public static void Down(
            [Required] [Aliases("")] [Description("The connection string to the database.")] string connectionString,
            [Required] [Aliases("")] [Description("The database name to target.")] string databaseName,
            [Required] [Aliases("")] [Description("The path to the assembly that contains the migration.")] string assemblyPath,
            [DefaultValue(30)] [Aliases("")] [Description("The command timeout (in seconds) for the command(s) executed as part of the migration.")] int timeoutInSeconds,
            [DefaultValue(null)] [Aliases("")] [Description("Optional application context.")] string applicationContext,
            [Required] [Aliases("")] [Description("The version to migrate to.")] long targetVersion)
        {
            var assembly = Assembly.LoadFile(assemblyPath);
            var timeout = TimeSpan.FromSeconds(timeoutInSeconds);

            Console.WriteLine("PARAMETERS:");
            Console.WriteLine("             targetVersion: " + targetVersion);
            Console.WriteLine("          connectionString: " + connectionString);
            Console.WriteLine("              assemblyPath: " + assemblyPath);
            Console.WriteLine("                  assembly: " + assembly);
            Console.WriteLine("        timeout in seconds: " + timeoutInSeconds);
            Console.WriteLine("                   timeout: " + timeout);
            Console.WriteLine(string.Empty);

            MigrationExecutor.Down(assembly, connectionString, databaseName, targetVersion, Console.WriteLine, timeout, applicationContext);

            Console.WriteLine("Done");
        }

        /// <summary>
        /// The print help method.
        /// </summary>
        /// <param name="help">The generated help text.</param>
        [Empty]
        [Help(Aliases = "h,?,-h,-help")]
        [Verb(IsDefault = true)]
        public static void Help(string help)
        {
            Console.WriteLine("   Usage");
            Console.Write("   -----");

            // strip out the usage info about help, it's confusing
            help = help.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Skip(3).ToNewLineDelimited();
            Console.WriteLine(help);
            Console.WriteLine();
        }

        /// <summary>
        /// The handle errors method.
        /// </summary>
        /// <param name="context">The context around the exception.</param>
        [Error]
        public static void Error(ExceptionContext context)
        {
            // change color to red
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            // parser exception or 
            if (context.Exception is CommandLineParserException)
            {
                Console.WriteLine("I don't understand.  Run the exe with the 'help' command for usage.");
                Console.WriteLine("   " + context.Exception.Message);
            }
            else
            {
                Console.WriteLine("Something broke while running.");
                Console.WriteLine("   " + context.Exception.Message);
                Console.WriteLine(string.Empty);
                Console.WriteLine("   " + context.Exception);
            }

            // restore color
            Console.WriteLine();
            Console.ForegroundColor = originalColor;
        }
    }
}