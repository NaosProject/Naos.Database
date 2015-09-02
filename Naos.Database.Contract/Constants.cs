// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Contract
{
    /// <summary>
    /// Constants used in project.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The extension (WITHOUT the PERIOD) of an MS SQL Server Data File
        /// </summary>
        public const string MsSqlDataFileExtension = "mdf";

        /// <summary>
        /// The extension (WITHOUT the PERIOD) of an MS SQL Server Log File
        /// </summary>
        public const string MsSqlLogFileExtension = "ldf";

        /// <summary>
        /// The value to be specified for unlimited file growth in an MS SQL Server file.
        /// </summary>
        public const int InfinityMaxSize = -1;
    }
}
