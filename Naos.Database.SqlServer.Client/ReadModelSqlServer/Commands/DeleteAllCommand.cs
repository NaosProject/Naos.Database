// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteAllCommand.cs">
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
    ///     Command for deleting all results of a particular model type (aka. table name).
    /// </summary>
    public class DeleteAllCommand : ICommand
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
    ///     Command handler for DeleteAllCommand.
    /// </summary>
    /// <typeparam name="TDatabase">The type of the database.</typeparam>
    public sealed class DeleteAllCommandHandler<TDatabase> : ICommandHandler<DeleteAllCommand>
        where TDatabase : ReadModelDatabase<TDatabase>
    {
        private readonly TDatabase readModelDatabase;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DeleteAllCommandHandler" /> class.
        /// </summary>
        /// <param name="readModelDatabase">The read model database.</param>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
            Justification = "ArgumentExceptions do use argument names, but provide additional information.")]
        public DeleteAllCommandHandler(TDatabase readModelDatabase)
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
        /// <exception cref="System.ArgumentNullException">command</exception>
        public void Handle(DeleteAllCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            using (var connection = this.readModelDatabase.CreateConnection())
            {
                connection.Execute(string.Format(CultureInfo.InvariantCulture, "delete from [{0}]", command.ModelType));
            }
        }
    }
}
