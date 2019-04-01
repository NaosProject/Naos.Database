// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommandHandler.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    /// <summary>
    ///     Represents a command handler (typically a database write operation).
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    public interface ICommandHandler<in TCommand>
        where TCommand : ICommand
    {
        /// <summary>
        ///     Handles the specified command.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        void Handle(TCommand command);
    }
}
