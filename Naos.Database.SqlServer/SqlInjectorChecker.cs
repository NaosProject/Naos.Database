// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlInjectorChecker.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    using Spritely.Recipes;

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
            new { textToCheck }.Must().NotBeNull().OrThrow();

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
            new { pathToCheck }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();

            try
            {
                var fileInfoToCheck = new FileInfo(pathToCheck);
                new { fileInfoToCheck }.Must().NotBeNull().OrThrowFirstFailure();
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
