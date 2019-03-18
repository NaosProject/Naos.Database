// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlObjectScripter.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer.Administration
{
    using System;
    using System.Collections.Specialized;
    using System.Text;

    using Microsoft.SqlServer.Management.Smo;

    using Naos.Database.Domain;
    using Naos.Database.SqlServer.Domain;
    using OBeautifulCode.Validation.Recipes;

    /// <summary>
    /// Class to script objects from database.
    /// </summary>
    public static class SqlObjectScripter
    {
        /// <summary>
        /// Scripts a <see cref="IScriptable" /> along with provided options.
        /// </summary>
        /// <param name="scriptableObject">Object to script.</param>
        /// <param name="includeDrop">Value indicating whether or not to include drop logic at the beginning.</param>
        /// <param name="scrubScript">Value indicating whether or not to scrub the script to make it more readable and remove issues that prvent running.</param>
        /// <returns>Scripted object as a string.</returns>
        public static string Script(IScriptable scriptableObject, bool includeDrop = true, bool scrubScript = true)
        {
            ScriptingOptions dropOptions = null;
            if (includeDrop)
            {
                dropOptions = BuildDefaultDropScriptingOptions();
            }

            var createOptions = BuildDefaultCreateScriptingOptions(scriptableObject);

            return Script(scriptableObject, dropOptions, createOptions, scrubScript);
        }

        private static ScriptingOptions BuildDefaultDropScriptingOptions()
        {
            var dropOptions = new ScriptingOptions { AllowSystemObjects = false, ScriptDrops = true, IncludeIfNotExists = true };
            return dropOptions;
        }

        private static ScriptingOptions BuildDefaultCreateScriptingOptions(IScriptable scriptableObject)
        {
            var createOptions = new ScriptingOptions
                                    {
                                        AllowSystemObjects = false,
                                        Bindings = true,
                                        Default = true,
                                        Permissions = true,
                                        ExtendedProperties = true,
                                        DriPrimaryKey = true,
                                        DriUniqueKeys = true,
                                        DriChecks = true,
                                        DriDefaults = true,
                                        DriForeignKeys = scriptableObject is ForeignKey,
                                    };
            return createOptions;
        }

        /// <summary>
        /// Scripts a <see cref="IScriptable" /> along with provided options.
        /// </summary>
        /// <param name="scriptableObject">Object to script.</param>
        /// <param name="dropOptions">Drop options; null will ommit.</param>
        /// <param name="createOptions">Create options.</param>
        /// <param name="scrubScript">Value indicating whether or not to scrub the script to make it more readable and remove issues that prvent running.</param>
        /// <returns>Scripted object as a string.</returns>
        public static string Script(IScriptable scriptableObject, ScriptingOptions dropOptions, ScriptingOptions createOptions, bool scrubScript)
        {
            new { scriptableObject }.Must().NotBeNull();
            new { dropOptions }.Must().NotBeNull();
            new { createOptions }.Must().NotBeNull();

            StringCollection dropScript = null;
            if (dropOptions != null)
            {
                dropScript = scriptableObject.Script(dropOptions);
                dropScript.Add("GO");
            }

            var createScript = scriptableObject.Script(createOptions);
            createScript.Add("GO");

            if (scrubScript)
            {
                ScrubScript(createScript);
            }

            return StringCollectionToSingleString(dropScript) + StringCollectionToSingleString(createScript);
        }

        /// <summary>
        /// Scripts a <see cref="IScriptable" /> along with provided options.
        /// </summary>
        /// <param name="scriptableObject">Object to script.</param>
        /// <param name="scrubScript">Optional value indicating whether or not to scrub the script to make it more readable and remove issues that prvent running; DEFAULT is true.</param>
        /// <returns>Scripted object as a string.</returns>
        public static ScriptedObject ScriptToObject(ScriptableObject scriptableObject, bool scrubScript = true)
        {
            new { scriptableObject }.Must().NotBeNull();

            var dropOptions = BuildDefaultDropScriptingOptions();
            var createOptions = BuildDefaultCreateScriptingOptions(scriptableObject.ObjectToScript);
            var dropScript = scriptableObject.ObjectToScript.Script(dropOptions);
            dropScript.Add("GO");

            var createScript = scriptableObject.ObjectToScript.Script(createOptions);
            createScript.Add("GO");

            if (scrubScript)
            {
                ScrubScript(createScript);
            }

            var dropScriptString = StringCollectionToSingleString(dropScript);
            var createScriptString = StringCollectionToSingleString(createScript);
            return new ScriptedObject(scriptableObject.Name, scriptableObject.DatabaseObjectType, dropScriptString, createScriptString);
        }

        /// <summary>
        /// Take a <see cref="StringCollection" /> and in-line tweak various things in the default scripting that make it less human readable and are not necessary for application.
        /// </summary>
        /// <param name="createScript">Script to tweak.</param>
        internal static void ScrubScript(StringCollection createScript)
        {
            for (var lineNumber = 0; lineNumber < createScript.Count; lineNumber++)
            { // these have to be stripped since the create statement must be first in transaction for many objects
                // and these are added at beginning of many scripts by default.
                if (createScript[lineNumber] == "SET ANSI_NULLS OFF"
                    || createScript[lineNumber] == "SET ANSI_NULLS ON"
                    || createScript[lineNumber] == "SET QUOTED_IDENTIFIER OFF"
                    || createScript[lineNumber] == "SET QUOTED_IDENTIFIER ON")
                {
                    createScript[lineNumber] = createScript[lineNumber] + Environment.NewLine + "GO"; // adding a go after the checked commands
                }
                else if (createScript[lineNumber].Contains("GRANT EXECUTE ON"))
                {
                    createScript[lineNumber] = "GO" + Environment.NewLine + createScript[lineNumber];
                }
                else if (createScript[lineNumber].Contains("EXEC dbo.sp_addextendedproperty "))
                {
                    var lineToTweak = createScript[lineNumber];
                    lineToTweak = lineToTweak.Replace("EXEC dbo.", string.Empty);
                    lineToTweak = lineToTweak.Replace("@name=", string.Empty);
                    lineToTweak = lineToTweak.Replace("@value=", string.Empty);
                    lineToTweak = lineToTweak.Replace("@level0type=", string.Empty);
                    lineToTweak = lineToTweak.Replace("@level0name=", string.Empty);
                    lineToTweak = lineToTweak.Replace("@level1type=", string.Empty);
                    lineToTweak = lineToTweak.Replace("@level1name=", string.Empty);
                    if (lineToTweak.Contains("@level2type") && lineToTweak.Contains("@level2name"))
                    {
                        lineToTweak = lineToTweak.Replace("@level2type=", string.Empty);
                        lineToTweak = lineToTweak.Replace("@level2name=", string.Empty);
                    }
                    else
                    {
                        lineToTweak = lineToTweak + ", NULL, NULL";
                    }

                    createScript[lineNumber] = Environment.NewLine + "GO" + Environment.NewLine + lineToTweak;
                }

                // adding a go before the grant so it will be in a separate transaction
                // calling .insert generated out of memeory exception so concatenating to the string element
            }
        }

        private static string StringCollectionToSingleString(StringCollection sc)
        {
            if (sc == null)
            {
                return string.Empty;
            }
            else
            {
                var temp = new StringBuilder(sc.Count);

                foreach (string line in sc)
                {
                    temp.AppendLine(line);
                }

                return temp.ToString();
            }
        }
    }
}
