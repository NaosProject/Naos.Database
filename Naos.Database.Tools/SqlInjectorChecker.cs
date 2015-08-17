// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlInjectorChecker.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Tools
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    using Conditions;

    /// <summary>
    /// Utility methods to guard against SQL Injection.
    /// </summary>
    public class SqlInjectorChecker
    {
        /// <summary>
        /// Throws an ArgumentException if input has any characters that are not alpha-numeric nor the space character.
        /// </summary>
        /// <param name="textToCheck">Text to check.</param>
        public static void ThrowIfNotAlphanumericOrSpace(string textToCheck)
        {
            Condition.Requires(textToCheck).IsNotNull();
            const string Pattern = @"[a-zA-Z0-9 ]*";
            Match match = Regex.Match(textToCheck, Pattern);
            if (match.Value != textToCheck)
            {
                throw new ArgumentException("The provided input: " + textToCheck + " is not alphanumeric and is not valid.");
            }
        }

        /// <summary>
        /// Throws an ArgumentException if input is not a valid path.
        /// </summary>
        /// <param name="pathToCheck">Path to check.</param>
        public static void ThrowIfNotValidPath(string pathToCheck)
        {
            try
            {
                new FileInfo(pathToCheck);
            }
            catch (Exception)
            {
                throw new ArgumentException("The provided path: " + pathToCheck + " is invalid.");
            }

            if (pathToCheck.Contains("'") || pathToCheck.Contains("\"") || pathToCheck.Contains(";"))
            {
                throw new ArgumentException("The provided path: " + pathToCheck + " contains either quotes or single quotes or semicolons which are not allowed.");
            }
        }
    }
}
