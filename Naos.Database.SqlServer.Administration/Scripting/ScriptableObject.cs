// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScriptableObject.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.SqlServer.Administration
{
    using Microsoft.SqlServer.Management.Smo;

    using Naos.Database.SqlServer.Domain;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// Model object that describes an object that can be scripted.
    /// </summary>
    public class ScriptableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptableObject"/> class.
        /// </summary>
        /// <param name="table">Object that can be scripted.</param>
        public ScriptableObject(Table table)
        {
            new { table }.AsArg().Must().NotBeNull();

            this.Name = table.Name;
            this.ObjectToScript = table;
            this.DatabaseObjectType = ScriptableObjectType.Table;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptableObject"/> class.
        /// </summary>
        /// <param name="view">Object that can be scripted.</param>
        public ScriptableObject(View view)
        {
            new { view }.AsArg().Must().NotBeNull();

            this.Name = view.Name;
            this.ObjectToScript = view;
            this.DatabaseObjectType = ScriptableObjectType.View;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptableObject"/> class.
        /// </summary>
        /// <param name="storedProcedure">Object that can be scripted.</param>
        public ScriptableObject(StoredProcedure storedProcedure)
        {
            new { storedProcedure }.AsArg().Must().NotBeNull();

            this.Name = storedProcedure.Name;
            this.ObjectToScript = storedProcedure;
            this.DatabaseObjectType = ScriptableObjectType.StoredProcedure;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptableObject"/> class.
        /// </summary>
        /// <param name="userDefinedFunction">Object that can be scripted.</param>
        public ScriptableObject(UserDefinedFunction userDefinedFunction)
        {
            new { userDefinedFunction }.AsArg().Must().NotBeNull();

            this.Name = userDefinedFunction.Name;
            this.ObjectToScript = userDefinedFunction;
            this.DatabaseObjectType = ScriptableObjectType.UserDefinedFunction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptableObject"/> class.
        /// </summary>
        /// <param name="userDefinedDataType">Object that can be scripted.</param>
        public ScriptableObject(UserDefinedDataType userDefinedDataType)
        {
            new { userDefinedDataType }.AsArg().Must().NotBeNull();

            this.Name = userDefinedDataType.Name;
            this.ObjectToScript = userDefinedDataType;
            this.DatabaseObjectType = ScriptableObjectType.UserDefinedDataType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptableObject"/> class.
        /// </summary>
        /// <param name="databaseRole">Object that can be scripted.</param>
        public ScriptableObject(DatabaseRole databaseRole)
        {
            new { databaseRole }.AsArg().Must().NotBeNull();

            this.Name = databaseRole.Name;
            this.ObjectToScript = databaseRole;
            this.DatabaseObjectType = ScriptableObjectType.DatabaseRole;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptableObject"/> class.
        /// </summary>
        /// <param name="user">Object that can be scripted.</param>
        public ScriptableObject(User user)
        {
            new { user }.AsArg().Must().NotBeNull();

            this.Name = user.Name;
            this.ObjectToScript = user;
            this.DatabaseObjectType = ScriptableObjectType.User;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptableObject"/> class.
        /// </summary>
        /// <param name="foreignKey">Object that can be scripted.</param>
        public ScriptableObject(ForeignKey foreignKey)
        {
            new { foreignKey }.AsArg().Must().NotBeNull();

            this.Name = foreignKey.Name;
            this.ObjectToScript = foreignKey;
            this.DatabaseObjectType = ScriptableObjectType.ForeignKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptableObject"/> class.
        /// </summary>
        /// <param name="index">Object that can be scripted.</param>
        public ScriptableObject(Index index)
        {
            new { index }.AsArg().Must().NotBeNull();

            this.Name = index.Name;
            this.ObjectToScript = index;
            this.DatabaseObjectType = ScriptableObjectType.Index;
        }

        /// <summary>
        /// Gets the name of the object.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the database type of object.
        /// </summary>
        public ScriptableObjectType DatabaseObjectType { get; private set; }

        /// <summary>
        /// Gets the object to script.
        /// </summary>
        public IScriptable ObjectToScript { get; private set; }
    }
}
