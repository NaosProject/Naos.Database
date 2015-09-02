// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecoveryOption.cs" company="Naos">
//   Copyright 2015 Naos
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Contract
{
    /// <summary>
    /// Controls rollback in a restore operation.
    /// </summary>
    /// <remarks>
    /// See here: <a href="https://technet.microsoft.com/en-us/library/ms191455%28v=sql.105%29.aspx"/>
    /// </remarks>
    public enum RecoveryOption
    {
        /// <summary>
        /// Rollback any active transactions after data has been restored.
        /// </summary>
        Recovery,

        /// <summary>
        /// Preserve uncommitted transactions.  Database will be offline after restore procedure.
        /// </summary>
        NoRecovery
    }
}
