﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.178.0)
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
    using global::OBeautifulCode.Representation.System;
    using global::OBeautifulCode.Type;
    using global::OBeautifulCode.Type.Recipes;

    using static global::System.FormattableString;

    [Serializable]
    public partial class TypeRepresentationWithAndWithoutVersion : IModel<TypeRepresentationWithAndWithoutVersion>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="TypeRepresentationWithAndWithoutVersion"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(TypeRepresentationWithAndWithoutVersion left, TypeRepresentationWithAndWithoutVersion right)
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
        /// Determines whether two objects of type <see cref="TypeRepresentationWithAndWithoutVersion"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(TypeRepresentationWithAndWithoutVersion left, TypeRepresentationWithAndWithoutVersion right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(TypeRepresentationWithAndWithoutVersion other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.WithVersion.IsEqualTo(other.WithVersion)
                      && this.WithoutVersion.IsEqualTo(other.WithoutVersion);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as TypeRepresentationWithAndWithoutVersion);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.WithVersion)
            .Hash(this.WithoutVersion)
            .Value;

        /// <inheritdoc />
        public object Clone() => this.DeepClone();

        /// <inheritdoc />
        public TypeRepresentationWithAndWithoutVersion DeepClone()
        {
            var result = new TypeRepresentationWithAndWithoutVersion(
                                 this.WithVersion?.DeepClone(),
                                 this.WithoutVersion?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="WithVersion" />.
        /// </summary>
        /// <param name="withVersion">The new <see cref="WithVersion" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="TypeRepresentationWithAndWithoutVersion" /> using the specified <paramref name="withVersion" /> for <see cref="WithVersion" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames")]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        [SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public TypeRepresentationWithAndWithoutVersion DeepCloneWithWithVersion(TypeRepresentation withVersion)
        {
            var result = new TypeRepresentationWithAndWithoutVersion(
                                 withVersion,
                                 this.WithoutVersion?.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="WithoutVersion" />.
        /// </summary>
        /// <param name="withoutVersion">The new <see cref="WithoutVersion" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="TypeRepresentationWithAndWithoutVersion" /> using the specified <paramref name="withoutVersion" /> for <see cref="WithoutVersion" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings")]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames")]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
        [SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration")]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public TypeRepresentationWithAndWithoutVersion DeepCloneWithWithoutVersion(TypeRepresentation withoutVersion)
        {
            var result = new TypeRepresentationWithAndWithoutVersion(
                                 this.WithVersion?.DeepClone(),
                                 withoutVersion);

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public override string ToString()
        {
            var result = Invariant($"Naos.Database.Domain.TypeRepresentationWithAndWithoutVersion: WithVersion = {this.WithVersion?.ToString() ?? "<null>"}, WithoutVersion = {this.WithoutVersion?.ToString() ?? "<null>"}.");

            return result;
        }
    }
}