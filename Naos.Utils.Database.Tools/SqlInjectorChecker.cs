// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlInjectorChecker.cs" company="Naos">
//   Copyright 2014 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Utils.Database.Tools
{
    using System;
    using System.IO;

    using OBeautifulCode.Libs.String;

    /// <summary>
    /// Utility methods to guard against SQL Injection.
    /// </summary>
    public class SqlInjectorChecker
    {
        #region Public Methods

        /// <summary>
        /// Throws an ArgumentException if input is not an alpha numeric string.
        /// </summary>
        /// <param name="textToCheck">Text to check.</param>
        public static void ThrowIfNotAlphanumeric(string textToCheck)
        {
            if (!textToCheck.IsAlphanumeric())
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

        #endregion
    }
}
