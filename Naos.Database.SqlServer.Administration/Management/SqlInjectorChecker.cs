// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlInjectorChecker.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer.Administration
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    using OBeautifulCode.Validation.Recipes;

    /// <summary>
    /// Utility methods to guard against SQL Injection.
    /// </summary>
    public static class SqlInjectorChecker
    {
        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if input has any characters that are not alpha-numeric nor the space character nor the underscore character.
        /// </summary>
        /// <param name="textToCheck">Text to check.</param>
        public static void ThrowIfNotAlphanumericOrSpaceOrUnderscore(string textToCheck)
        {
            new { textToCheck }.Must().NotBeNull();

            const string pattern = @"[a-zA-Z0-9 _]*";
            Match match = Regex.Match(textToCheck, pattern);
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
            new { pathToCheck }.Must().NotBeNullNorWhiteSpace();

            try
            {
                var fileInfoToCheck = new FileInfo(pathToCheck);
                new { fileInfoToCheck }.Must().NotBeNull();
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
