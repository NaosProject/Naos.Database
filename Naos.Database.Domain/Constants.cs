// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    /// Constants used in project.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The extension (WITHOUT the PERIOD) of an MS SQL Server Data File
        /// </summary>
        public const string MicrosoftSqlDataFileExtension = "mdf";

        /// <summary>
        /// The extension (WITHOUT the PERIOD) of an MS SQL Server Log File
        /// </summary>
        public const string MicrosoftSqlLogFileExtension = "ldf";

        /// <summary>
        /// The value to be specified for unlimited file growth in an MS SQL Server file.
        /// </summary>
        public const int InfinityMaxSize = -1;
    }
}
