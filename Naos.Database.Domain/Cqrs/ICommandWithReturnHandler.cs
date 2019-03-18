// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommandWithReturnHandler.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer.Client
{
    /// <summary>
    ///     Represents a command with return handler (a write operation that returns a value).
    /// </summary>
    /// <typeparam name="TCommandWithReturn">The type of the command parameters.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface ICommandWithReturnHandler<in TCommandWithReturn, out TResult>
        where TCommandWithReturn : ICommandWithReturn<TResult>
    {
        /// <summary>
        ///     Handles the specified command.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <returns>The command results.</returns>
        TResult Handle(TCommandWithReturn command);
    }
}
