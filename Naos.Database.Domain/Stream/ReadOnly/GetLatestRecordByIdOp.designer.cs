﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.145.0)
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

    using global::Naos.Protocol.Domain;

    using global::OBeautifulCode.Equality.Recipes;
    using global::OBeautifulCode.Representation.System;
    using global::OBeautifulCode.Type;
    using global::OBeautifulCode.Type.Recipes;

    using static global::System.FormattableString;

    [Serializable]
    public partial class GetLatestRecordByIdOp : IModel<GetLatestRecordByIdOp>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="GetLatestRecordByIdOp"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(GetLatestRecordByIdOp left, GetLatestRecordByIdOp right)
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
        /// Determines whether two objects of type <see cref="GetLatestRecordByIdOp"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(GetLatestRecordByIdOp left, GetLatestRecordByIdOp right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(GetLatestRecordByIdOp other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.StringSerializedId.IsEqualTo(other.StringSerializedId, StringComparer.Ordinal)
                      && this.IdentifierType.IsEqualTo(other.IdentifierType)
                      && this.ObjectType.IsEqualTo(other.ObjectType)
                      && this.TypeVersionMatchStrategy.IsEqualTo(other.TypeVersionMatchStrategy)
                      && this.ExistingRecordNotEncounteredStrategy.IsEqualTo(other.ExistingRecordNotEncounteredStrategy)
                      && this.SpecifiedResourceLocator.IsEqualTo(other.SpecifiedResourceLocator);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as GetLatestRecordByIdOp);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.StringSerializedId)
            .Hash(this.IdentifierType)
            .Hash(this.ObjectType)
            .Hash(this.TypeVersionMatchStrategy)
            .Hash(this.ExistingRecordNotEncounteredStrategy)
            .Hash(this.SpecifiedResourceLocator)
            .Value;

        /// <inheritdoc />
        public new GetLatestRecordByIdOp DeepClone() => (GetLatestRecordByIdOp)this.DeepCloneInternal();

        /// <summary>
        /// Deep clones this object with a new <see cref="StringSerializedId" />.
        /// </summary>
        /// <param name="stringSerializedId">The new <see cref="StringSerializedId" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="GetLatestRecordByIdOp" /> using the specified <paramref name="stringSerializedId" /> for <see cref="StringSerializedId" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002: DoNotExposeGenericLists")]
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
        public GetLatestRecordByIdOp DeepCloneWithStringSerializedId(string stringSerializedId)
        {
            var result = new GetLatestRecordByIdOp(
                                 stringSerializedId,
                                 this.IdentifierType?.DeepClone(),
                                 this.ObjectType?.DeepClone(),
                                 this.TypeVersionMatchStrategy,
                                 this.ExistingRecordNotEncounteredStrategy,
                                 (IResourceLocator)DeepCloneInterface(this.SpecifiedResourceLocator));

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="IdentifierType" />.
        /// </summary>
        /// <param name="identifierType">The new <see cref="IdentifierType" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="GetLatestRecordByIdOp" /> using the specified <paramref name="identifierType" /> for <see cref="IdentifierType" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002: DoNotExposeGenericLists")]
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
        public GetLatestRecordByIdOp DeepCloneWithIdentifierType(TypeRepresentation identifierType)
        {
            var result = new GetLatestRecordByIdOp(
                                 this.StringSerializedId?.DeepClone(),
                                 identifierType,
                                 this.ObjectType?.DeepClone(),
                                 this.TypeVersionMatchStrategy,
                                 this.ExistingRecordNotEncounteredStrategy,
                                 (IResourceLocator)DeepCloneInterface(this.SpecifiedResourceLocator));

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="ObjectType" />.
        /// </summary>
        /// <param name="objectType">The new <see cref="ObjectType" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="GetLatestRecordByIdOp" /> using the specified <paramref name="objectType" /> for <see cref="ObjectType" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002: DoNotExposeGenericLists")]
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
        public GetLatestRecordByIdOp DeepCloneWithObjectType(TypeRepresentation objectType)
        {
            var result = new GetLatestRecordByIdOp(
                                 this.StringSerializedId?.DeepClone(),
                                 this.IdentifierType?.DeepClone(),
                                 objectType,
                                 this.TypeVersionMatchStrategy,
                                 this.ExistingRecordNotEncounteredStrategy,
                                 (IResourceLocator)DeepCloneInterface(this.SpecifiedResourceLocator));

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="TypeVersionMatchStrategy" />.
        /// </summary>
        /// <param name="typeVersionMatchStrategy">The new <see cref="TypeVersionMatchStrategy" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="GetLatestRecordByIdOp" /> using the specified <paramref name="typeVersionMatchStrategy" /> for <see cref="TypeVersionMatchStrategy" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002: DoNotExposeGenericLists")]
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
        public GetLatestRecordByIdOp DeepCloneWithTypeVersionMatchStrategy(TypeVersionMatchStrategy typeVersionMatchStrategy)
        {
            var result = new GetLatestRecordByIdOp(
                                 this.StringSerializedId?.DeepClone(),
                                 this.IdentifierType?.DeepClone(),
                                 this.ObjectType?.DeepClone(),
                                 typeVersionMatchStrategy,
                                 this.ExistingRecordNotEncounteredStrategy,
                                 (IResourceLocator)DeepCloneInterface(this.SpecifiedResourceLocator));

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="ExistingRecordNotEncounteredStrategy" />.
        /// </summary>
        /// <param name="existingRecordNotEncounteredStrategy">The new <see cref="ExistingRecordNotEncounteredStrategy" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="GetLatestRecordByIdOp" /> using the specified <paramref name="existingRecordNotEncounteredStrategy" /> for <see cref="ExistingRecordNotEncounteredStrategy" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002: DoNotExposeGenericLists")]
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
        public GetLatestRecordByIdOp DeepCloneWithExistingRecordNotEncounteredStrategy(ExistingRecordNotEncounteredStrategy existingRecordNotEncounteredStrategy)
        {
            var result = new GetLatestRecordByIdOp(
                                 this.StringSerializedId?.DeepClone(),
                                 this.IdentifierType?.DeepClone(),
                                 this.ObjectType?.DeepClone(),
                                 this.TypeVersionMatchStrategy,
                                 existingRecordNotEncounteredStrategy,
                                 (IResourceLocator)DeepCloneInterface(this.SpecifiedResourceLocator));

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="SpecifiedResourceLocator" />.
        /// </summary>
        /// <param name="specifiedResourceLocator">The new <see cref="SpecifiedResourceLocator" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="GetLatestRecordByIdOp" /> using the specified <paramref name="specifiedResourceLocator" /> for <see cref="SpecifiedResourceLocator" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002: DoNotExposeGenericLists")]
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
        public GetLatestRecordByIdOp DeepCloneWithSpecifiedResourceLocator(IResourceLocator specifiedResourceLocator)
        {
            var result = new GetLatestRecordByIdOp(
                                 this.StringSerializedId?.DeepClone(),
                                 this.IdentifierType?.DeepClone(),
                                 this.ObjectType?.DeepClone(),
                                 this.TypeVersionMatchStrategy,
                                 this.ExistingRecordNotEncounteredStrategy,
                                 specifiedResourceLocator);

            return result;
        }

        /// <inheritdoc />
        protected override OperationBase DeepCloneInternal()
        {
            var result = new GetLatestRecordByIdOp(
                                 this.StringSerializedId?.DeepClone(),
                                 this.IdentifierType?.DeepClone(),
                                 this.ObjectType?.DeepClone(),
                                 this.TypeVersionMatchStrategy,
                                 this.ExistingRecordNotEncounteredStrategy,
                                 (IResourceLocator)DeepCloneInterface(this.SpecifiedResourceLocator));

            return result;
        }

        private static object DeepCloneInterface(object value)
        {
            object result;

            if (ReferenceEquals(value, null))
            {
                result = null;
            }
            else
            {
                var type = value.GetType();

                if (type.IsValueType)
                {
                    result = value;
                }
                else if (value is string valueAsString)
                {
                    result = valueAsString.DeepClone();
                }
                else if (value is global::System.Version valueAsVersion)
                {
                    result = valueAsVersion.DeepClone();
                }
                else if (value is global::System.Uri valueAsUri)
                {
                    result = valueAsUri.DeepClone();
                }
                else
                {
                    var deepCloneableInterface = typeof(IDeepCloneable<>).MakeGenericType(type);

                    if (deepCloneableInterface.IsAssignableFrom(type))
                    {
                        var deepCloneMethod = deepCloneableInterface.GetMethod(nameof(IDeepCloneable<object>.DeepClone));

                        result = deepCloneMethod.Invoke(value, null);
                    }
                    else
                    {
                        throw new NotSupportedException(Invariant($"I do not know how to deep clone an object of type '{type.ToStringReadable()}'"));
                    }
                }
            }

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public override string ToString()
        {
            var result = Invariant($"Naos.Database.Domain.GetLatestRecordByIdOp: StringSerializedId = {this.StringSerializedId?.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, IdentifierType = {this.IdentifierType?.ToString() ?? "<null>"}, ObjectType = {this.ObjectType?.ToString() ?? "<null>"}, TypeVersionMatchStrategy = {this.TypeVersionMatchStrategy.ToString() ?? "<null>"}, ExistingRecordNotEncounteredStrategy = {this.ExistingRecordNotEncounteredStrategy.ToString() ?? "<null>"}, SpecifiedResourceLocator = {this.SpecifiedResourceLocator?.ToString() ?? "<null>"}.");

            return result;
        }
    }
}