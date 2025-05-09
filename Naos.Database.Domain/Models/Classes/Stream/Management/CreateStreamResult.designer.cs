﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.192.0)
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using global::System;
    using global::System.CodeDom.Compiler;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Collections.ObjectModel;
    using global::System.Diagnostics.CodeAnalysis;
    using global::System.Globalization;
    using global::System.Linq;

    using global::OBeautifulCode.Cloning.Recipes;
    using global::OBeautifulCode.Equality.Recipes;
    using global::OBeautifulCode.Type;
    using global::OBeautifulCode.Type.Recipes;

    using static global::System.FormattableString;

    [Serializable]
    public partial class CreateStreamResult : IModel<CreateStreamResult>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="CreateStreamResult"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(CreateStreamResult left, CreateStreamResult right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }

            var result = left.Equals(right);

            return result;
        }

        /// <summary>
        /// Determines whether two objects of type <see cref="CreateStreamResult"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(CreateStreamResult left, CreateStreamResult right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(CreateStreamResult other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.AlreadyExisted.IsEqualTo(other.AlreadyExisted)
                      && this.WasCreated.IsEqualTo(other.WasCreated);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as CreateStreamResult);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.AlreadyExisted)
            .Hash(this.WasCreated)
            .Value;

        /// <inheritdoc />
        public object Clone() => this.DeepClone();

        /// <inheritdoc />
        public CreateStreamResult DeepClone()
        {
            var result = new CreateStreamResult(
                                 this.AlreadyExisted.DeepClone(),
                                 this.WasCreated.DeepClone());

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public override string ToString()
        {
            var result = Invariant($"Naos.Database.Domain.CreateStreamResult: AlreadyExisted = {this.AlreadyExisted.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, WasCreated = {this.WasCreated.ToString(CultureInfo.InvariantCulture) ?? "<null>"}.");

            return result;
        }
    }
}