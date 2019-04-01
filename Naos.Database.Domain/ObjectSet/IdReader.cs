// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdReader.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using static System.FormattableString;

    /// <summary>
    /// Class capable of reading Id properties from objects.
    /// </summary>
    public static class IdReader
    {
        private static readonly ConcurrentDictionary<Type, IdDefinition> IdentifierDefinitions = new ConcurrentDictionary<Type, IdDefinition>();

        /// <summary>
        /// Read the value from an Id member of the specified model instance.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="model">The model.</param>
        /// <returns>The value of the Id member.</returns>
        public static object ReadValue<TModel>(TModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var identifierDefinition = IdentifierDefinitions.GetOrAdd(typeof(TModel), new IdDefinition(typeof(TModel)));
            return identifierDefinition.ReadValue(model);
        }

        /// <summary>
        /// Reads the name of the Id member.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns>The name of the Id member.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This type safe method is preferred to its non-type safe equivalent.")]
        public static string ReadName<TModel>()
        {
            var identifierDefinition = IdentifierDefinitions.GetOrAdd(typeof(TModel), new IdDefinition(typeof(TModel)));
            return identifierDefinition.Name;
        }

        /// <summary>
        /// Reads the type of the Id member.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <returns>The type of the Id member.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "There is an overload that doesn't require a generic parameter.")]
        public static Type ReadType<TModel>()
        {
            var identifierDefinition = IdentifierDefinitions.GetOrAdd(typeof(TModel), new IdDefinition(typeof(TModel)));
            return identifierDefinition.Type;
        }

        /// <summary>
        /// Reads the type of the Id member from the given model type.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>The type of the Id member.</returns>
        public static Type ReadType(Type modelType)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            var identifierDefinition = IdentifierDefinitions.GetOrAdd(modelType, new IdDefinition(modelType));
            return identifierDefinition.Type;
        }

        /// <summary>
        /// Sets the identifier member used during reads of a particular model type.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="identifierMember">The identifier member.</param>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "This type safe method is preferred to its non-type safe equivalent.")]
        public static void SetIdMember<TModel>(string identifierMember)
        {
            if (string.IsNullOrWhiteSpace(identifierMember))
            {
                throw new ArgumentNullException(nameof(identifierMember));
            }

            var identifierDefinition = IdentifierDefinitions.GetOrAdd(typeof(TModel), new IdDefinition(typeof(TModel)));
            identifierDefinition.Names = new[] { identifierMember };
        }

        /// <summary>
        /// Identifier definition.
        /// </summary>
        internal class IdDefinition
        {
            private static readonly object Lock = new object();

            private readonly Type modelType;

            private ICollection<string> names = new[] { "Id", "id", "_id" };

            private ReadIdFromModel readValue;

            private string name;

            private Type identifierType;

            /// <summary>
            /// Initializes a new instance of the <see cref="IdDefinition"/> class.
            /// </summary>
            /// <param name="modelType">Type of model.</param>
            public IdDefinition(Type modelType)
            {
                this.modelType = modelType;
            }

            /// <summary>
            /// Gets or sets the names.
            /// </summary>
            public ICollection<string> Names
            {
                get
                {
                    return this.names;
                }

                set
                {
                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(value));
                    }

                    lock (Lock)
                    {
                        this.names = value;
                        this.ReadValue = null;
                        this.Type = null;
                    }
                }
            }

            /// <summary>
            /// Gets a read value function.
            /// </summary>
            public ReadIdFromModel ReadValue
            {
                get
                {
                    if (this.readValue == null)
                    {
                        this.Initialize();
                    }

                    return this.readValue;
                }

                private set
                {
                    this.readValue = value;
                }
            }

            /// <summary>
            /// Gets the name of the identifier.
            /// </summary>
            public string Name
            {
                get
                {
                    if (this.name == null)
                    {
                        this.Initialize();
                    }

                    return this.name;
                }

                private set
                {
                    this.name = value;
                }
            }

            /// <summary>
            /// Gets the type of identifier.
            /// </summary>
            public Type Type
            {
                get
                {
                    if (this.identifierType == null)
                    {
                        this.Initialize();
                    }

                    return this.identifierType;
                }

                private set
                {
                    this.identifierType = value;
                }
            }

            private void Initialize()
            {
                var allMembers =
                    this.modelType.GetMembers(
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).ToList();

                var eligibleMembers =
                    allMembers.Where(
                        m => m.MemberType == MemberTypes.Field || (m.MemberType == MemberTypes.Property && ((PropertyInfo)m).CanRead));

                lock (Lock)
                {
                    var matchingMembers = eligibleMembers.Where(m => this.Names.Contains(m.Name));
                    var identifierMember = this.Names.SelectMany(identifierName => matchingMembers.Where(m => m.Name == identifierName)).FirstOrDefault();

                    if (identifierMember == null)
                    {
                        throw new ArgumentException(
                            Invariant(
                                $"{this.modelType.Name} does not have an Id property or it is not readable (set only). Add a property or field named [{this.Names}] or if your object has a different name call IdReader.SetIdMember<{this.modelType.Name}>(\"YourIdProperty\")"));
                    }

                    if (identifierMember.MemberType == MemberTypes.Field)
                    {
                        var identifierField = (FieldInfo)identifierMember;

                        this.ReadValue = model =>
                        {
                            var identifier = identifierField.GetValue(model);

                            return identifier;
                        };

                        this.Name = identifierField.Name;
                        this.Type = identifierField.FieldType;
                    }
                    else
                    {
                        var identifierProperty = (PropertyInfo)identifierMember;

                        this.ReadValue = model =>
                        {
                            var identifier = identifierProperty.GetMethod.Invoke(model, new object[] { });

                            return identifier;
                        };

                        this.Name = identifierProperty.Name;
                        this.Type = identifierProperty.GetMethod.ReturnType;
                    }
                }
            }
        }
    }
}
