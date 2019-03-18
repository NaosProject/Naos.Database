// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddOrUpdateCommand.cs">
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
    using Newtonsoft.Json;
    using Spritely.Cqrs;

    /// <summary>
    ///     Command to add or update a model in the store.
    /// </summary>
    public class AddOrUpdateCommand<TModel> : ICommand
    {
        /// <summary>
        ///     Gets or sets the model.
        /// </summary>
        /// <value>
        ///     The model.
        /// </value>
        public TModel Model { get; set; }

        /// <summary>
        ///     Gets or sets the type of the model (the table name).
        /// </summary>
        /// <value>
        ///     The type of the model.
        /// </value>
        public string ModelType { get; set; }
    }

    /// <summary>
    ///     Command handler for AddOrUpdateCommand{TModel}.
    /// </summary>
    public class AddOrUpdateCommandHandler<TDatabase, TModel> : ICommandHandler<AddOrUpdateCommand<TModel>>
        where TDatabase : ReadModelDatabase<TDatabase>
    {
        private readonly JsonSerializerSettings jsonSerializerSettings;
        private readonly TDatabase readModelDatabase;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddTearsheetCommandHandler" /> class.
        /// </summary>
        /// <param name="readModelDatabase">The read model database.</param>
        /// <param name="jsonSerializerSettings">The json serializer settings.</param>
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
            Justification = "ArgumentExceptions do use argument names, but provide additional information.")]
        public AddOrUpdateCommandHandler(
            TDatabase readModelDatabase,
            JsonSerializerSettings jsonSerializerSettings = null)
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

            this.jsonSerializerSettings = jsonSerializerSettings ?? Json.SerializerSettings;
        }

        /// <summary>
        ///     Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        public void Handle(AddOrUpdateCommand<TModel> command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            var sql = string.Format(CultureInfo.InvariantCulture,
                @"
if exists(select * from [{0}] where [{1}] = @Id) begin
    update [{0}] set [{2}] = @Model, [{3}] = getutcdate() where [{1}] = @Id
end
else begin
    insert into [{0}] ([{1}], [{2}]) values (@Id, @Model)
end",
                command.ModelType,
                this.readModelDatabase.GetIdColumnName(command.ModelType),
                this.readModelDatabase.GetModelColumnName(command.ModelType),
                this.readModelDatabase.GetUpdatedColumnName(command.ModelType));

            var model = JsonConvert.SerializeObject(command.Model, this.jsonSerializerSettings);

            using (var connection = this.readModelDatabase.CreateConnection())
            {
                connection.Execute(sql, new { Id = this.readModelDatabase.ReadId(command.Model), Model = model });
            }
        }
    }
}
