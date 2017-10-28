// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScriptedObject.cs" company="Naos">
//    Copyright (c) Naos 2017. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using Spritely.Recipes;

    /// <summary>
    /// Model object to hold a scripted object that can be applied to a different database.
    /// </summary>
    public class ScriptedObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptedObject"/> class.
        /// </summary>
        /// <param name="name">Name of the object.</param>
        /// <param name="databaseObjectType">Database type of object.</param>
        /// <param name="dropScript">Script to drop the object.</param>
        /// <param name="createScript">Script to create the object.</param>
        public ScriptedObject(string name, ScriptableObjectType databaseObjectType, string dropScript, string createScript)
        {
            new { name }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();
            new { dropScript }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();
            new { createScript }.Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();
            new { databaseObjectType }.Must().NotBeEqualTo(ScriptableObjectType.Invalid).OrThrowFirstFailure();

            this.Name = name;
            this.DatabaseObjectType = databaseObjectType;
            this.DropScript = dropScript;
            this.CreateScript = createScript;
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
        /// Gets the drop script.
        /// </summary>
        public string DropScript { get; private set; }

        /// <summary>
        /// Gets the create script.
        /// </summary>
        public string CreateScript { get; private set; }
    }

    /// <summary>
    /// Enumeration of scriptable object types.
    /// </summary>
    public enum ScriptableObjectType
    {
        /// <summary>
        /// Invalid default state.
        /// </summary>
        Invalid,

        /// <summary>
        /// Database table.
        /// </summary>
        Table,

        /// <summary>
        /// Table foreign key.
        /// </summary>
        ForeignKey,

        /// <summary>
        /// Table or view index.
        /// </summary>
        Index,

        /// <summary>
        /// Database view.
        /// </summary>
        View,

        /// <summary>
        /// Stored procedure.
        /// </summary>
        StoredProcedure,

        /// <summary>
        /// User defined function.
        /// </summary>
        UserDefinedFunction,

        /// <summary>
        /// User defined data type.
        /// </summary>
        UserDefinedDataType,

        /// <summary>
        /// Database role.
        /// </summary>
        DatabaseRole,

        /// <summary>
        /// Database user.
        /// </summary>
        User,
    }
}
