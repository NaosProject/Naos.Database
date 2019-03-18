// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteByCommand.cs">
//   Copyright (c) 2015. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Spritely.ReadModel.Sql
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Dapper;
    using Spritely.Cqrs;

    /// <summary>
    ///     Command for deleting results from a particular model type (aka. table name).
    /// </summary>
    public abstract class DeleteByCommand : ICommand
    {
        /// <summary>
        ///     Gets or sets the type of the model (the table name).
        /// </summary>
        /// <value>
        ///     The type of the model.
        /// </value>
        public string ModelType { get; set; }
    }

    /// <summary>
    ///     Command handler for DeleteByCommand.
    /// </summary>
    /// <typeparam name="TDatabase">The type of the database.</typeparam>
    public sealed class DeleteByCommandHandler<TDatabase> : ICommandHandler<DeleteByCommand>
        where TDatabase : ReadModelDatabase<TDatabase>
    {
        private readonly TDatabase readModelDatabase;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DeleteByCommandHandler" /> class.
        /// </summary>
        /// <param name="readModelDatabase">The read model database.</param>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
            Justification = "ArgumentExceptions do use argument names, but provide additional information.")]
        public DeleteByCommandHandler(TDatabase readModelDatabase)
        {
            if (readModelDatabase == null)
            {
                throw new ArgumentNullException("readModelDatabase");
            }

            if (readModelDatabase.ConnectionSettings == null)
            {
                throw new ArgumentNullException("readModelDatabase.ConnectionSettings");
            }

            this.readModelDatabase = readModelDatabase;
        }

        /// <summary>
        ///     Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <exception cref="System.ArgumentNullException">query</exception>
        public void Handle(DeleteByCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var commandParameters = command.ToCommandParameters();

            using (var connection = this.readModelDatabase.CreateConnection())
            {
                connection.Execute(
                    string.Format(CultureInfo.InvariantCulture, "delete from [{0}] where {1}", command.ModelType, commandParameters),
                    command);
            }
        }
    }
}
