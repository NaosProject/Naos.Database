﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.191.0)
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
    public partial class GetAllRecordsByIdOp<TId> : IModel<GetAllRecordsByIdOp<TId>>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="GetAllRecordsByIdOp{TId}"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(GetAllRecordsByIdOp<TId> left, GetAllRecordsByIdOp<TId> right)
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
        /// Determines whether two objects of type <see cref="GetAllRecordsByIdOp{TId}"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(GetAllRecordsByIdOp<TId> left, GetAllRecordsByIdOp<TId> right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(GetAllRecordsByIdOp<TId> other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.Id.IsEqualTo(other.Id)
                      && this.ObjectType.IsEqualTo(other.ObjectType)
                      && this.VersionMatchStrategy.IsEqualTo(other.VersionMatchStrategy)
                      && this.TagsToMatch.IsEqualTo(other.TagsToMatch)
                      && this.TagMatchStrategy.IsEqualTo(other.TagMatchStrategy)
                      && this.RecordNotFoundStrategy.IsEqualTo(other.RecordNotFoundStrategy)
                      && this.OrderRecordsBy.IsEqualTo(other.OrderRecordsBy)
                      && this.DeprecatedIdTypes.IsEqualTo(other.DeprecatedIdTypes)
                      && this.TypeSelectionStrategy.IsEqualTo(other.TypeSelectionStrategy);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as GetAllRecordsByIdOp<TId>);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.Id)
            .Hash(this.ObjectType)
            .Hash(this.VersionMatchStrategy)
            .Hash(this.TagsToMatch)
            .Hash(this.TagMatchStrategy)
            .Hash(this.RecordNotFoundStrategy)
            .Hash(this.OrderRecordsBy)
            .Hash(this.DeprecatedIdTypes)
            .Hash(this.TypeSelectionStrategy)
            .Value;

        /// <inheritdoc />
        public new GetAllRecordsByIdOp<TId> DeepClone() => (GetAllRecordsByIdOp<TId>)this.DeepCloneInternal();

        /// <summary>
        /// Deep clones this object with a new <see cref="Id" />.
        /// </summary>
        /// <param name="id">The new <see cref="Id" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="GetAllRecordsByIdOp{TId}" /> using the specified <paramref name="id" /> for <see cref="Id" /> and a deep clone of every other property.</returns>
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
        public GetAllRecordsByIdOp<TId> DeepCloneWithId(TId id)
        {
            var result = new GetAllRecordsByIdOp<TId>(
                                 id,
                                 this.ObjectType?.DeepClone(),
                                 this.VersionMatchStrategy.DeepClone(),
                                 this.TagsToMatch?.DeepClone(),
                                 this.TagMatchStrategy.DeepClone(),
                                 this.RecordNotFoundStrategy.DeepClone(),
                                 this.OrderRecordsBy.DeepClone(),
                                 this.DeprecatedIdTypes?.DeepClone(),
                                 this.TypeSelectionStrategy.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="ObjectType" />.
        /// </summary>
        /// <param name="objectType">The new <see cref="ObjectType" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="GetAllRecordsByIdOp{TId}" /> using the specified <paramref name="objectType" /> for <see cref="ObjectType" /> and a deep clone of every other property.</returns>
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
        public GetAllRecordsByIdOp<TId> DeepCloneWithObjectType(TypeRepresentation objectType)
        {
            var result = new GetAllRecordsByIdOp<TId>(
                                 this.Id == null ? default : this.Id.DeepClone(),
                                 objectType,
                                 this.VersionMatchStrategy.DeepClone(),
                                 this.TagsToMatch?.DeepClone(),
                                 this.TagMatchStrategy.DeepClone(),
                                 this.RecordNotFoundStrategy.DeepClone(),
                                 this.OrderRecordsBy.DeepClone(),
                                 this.DeprecatedIdTypes?.DeepClone(),
                                 this.TypeSelectionStrategy.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="VersionMatchStrategy" />.
        /// </summary>
        /// <param name="versionMatchStrategy">The new <see cref="VersionMatchStrategy" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="GetAllRecordsByIdOp{TId}" /> using the specified <paramref name="versionMatchStrategy" /> for <see cref="VersionMatchStrategy" /> and a deep clone of every other property.</returns>
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
        public GetAllRecordsByIdOp<TId> DeepCloneWithVersionMatchStrategy(VersionMatchStrategy versionMatchStrategy)
        {
            var result = new GetAllRecordsByIdOp<TId>(
                                 this.Id == null ? default : this.Id.DeepClone(),
                                 this.ObjectType?.DeepClone(),
                                 versionMatchStrategy,
                                 this.TagsToMatch?.DeepClone(),
                                 this.TagMatchStrategy.DeepClone(),
                                 this.RecordNotFoundStrategy.DeepClone(),
                                 this.OrderRecordsBy.DeepClone(),
                                 this.DeprecatedIdTypes?.DeepClone(),
                                 this.TypeSelectionStrategy.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="TagsToMatch" />.
        /// </summary>
        /// <param name="tagsToMatch">The new <see cref="TagsToMatch" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="GetAllRecordsByIdOp{TId}" /> using the specified <paramref name="tagsToMatch" /> for <see cref="TagsToMatch" /> and a deep clone of every other property.</returns>
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
        public GetAllRecordsByIdOp<TId> DeepCloneWithTagsToMatch(IReadOnlyCollection<NamedValue<string>> tagsToMatch)
        {
            var result = new GetAllRecordsByIdOp<TId>(
                                 this.Id == null ? default : this.Id.DeepClone(),
                                 this.ObjectType?.DeepClone(),
                                 this.VersionMatchStrategy.DeepClone(),
                                 tagsToMatch,
                                 this.TagMatchStrategy.DeepClone(),
                                 this.RecordNotFoundStrategy.DeepClone(),
                                 this.OrderRecordsBy.DeepClone(),
                                 this.DeprecatedIdTypes?.DeepClone(),
                                 this.TypeSelectionStrategy.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="TagMatchStrategy" />.
        /// </summary>
        /// <param name="tagMatchStrategy">The new <see cref="TagMatchStrategy" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="GetAllRecordsByIdOp{TId}" /> using the specified <paramref name="tagMatchStrategy" /> for <see cref="TagMatchStrategy" /> and a deep clone of every other property.</returns>
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
        public GetAllRecordsByIdOp<TId> DeepCloneWithTagMatchStrategy(TagMatchStrategy tagMatchStrategy)
        {
            var result = new GetAllRecordsByIdOp<TId>(
                                 this.Id == null ? default : this.Id.DeepClone(),
                                 this.ObjectType?.DeepClone(),
                                 this.VersionMatchStrategy.DeepClone(),
                                 this.TagsToMatch?.DeepClone(),
                                 tagMatchStrategy,
                                 this.RecordNotFoundStrategy.DeepClone(),
                                 this.OrderRecordsBy.DeepClone(),
                                 this.DeprecatedIdTypes?.DeepClone(),
                                 this.TypeSelectionStrategy.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="RecordNotFoundStrategy" />.
        /// </summary>
        /// <param name="recordNotFoundStrategy">The new <see cref="RecordNotFoundStrategy" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="GetAllRecordsByIdOp{TId}" /> using the specified <paramref name="recordNotFoundStrategy" /> for <see cref="RecordNotFoundStrategy" /> and a deep clone of every other property.</returns>
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
        public GetAllRecordsByIdOp<TId> DeepCloneWithRecordNotFoundStrategy(RecordNotFoundStrategy recordNotFoundStrategy)
        {
            var result = new GetAllRecordsByIdOp<TId>(
                                 this.Id == null ? default : this.Id.DeepClone(),
                                 this.ObjectType?.DeepClone(),
                                 this.VersionMatchStrategy.DeepClone(),
                                 this.TagsToMatch?.DeepClone(),
                                 this.TagMatchStrategy.DeepClone(),
                                 recordNotFoundStrategy,
                                 this.OrderRecordsBy.DeepClone(),
                                 this.DeprecatedIdTypes?.DeepClone(),
                                 this.TypeSelectionStrategy.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="OrderRecordsBy" />.
        /// </summary>
        /// <param name="orderRecordsBy">The new <see cref="OrderRecordsBy" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="GetAllRecordsByIdOp{TId}" /> using the specified <paramref name="orderRecordsBy" /> for <see cref="OrderRecordsBy" /> and a deep clone of every other property.</returns>
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
        public GetAllRecordsByIdOp<TId> DeepCloneWithOrderRecordsBy(OrderRecordsBy orderRecordsBy)
        {
            var result = new GetAllRecordsByIdOp<TId>(
                                 this.Id == null ? default : this.Id.DeepClone(),
                                 this.ObjectType?.DeepClone(),
                                 this.VersionMatchStrategy.DeepClone(),
                                 this.TagsToMatch?.DeepClone(),
                                 this.TagMatchStrategy.DeepClone(),
                                 this.RecordNotFoundStrategy.DeepClone(),
                                 orderRecordsBy,
                                 this.DeprecatedIdTypes?.DeepClone(),
                                 this.TypeSelectionStrategy.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="DeprecatedIdTypes" />.
        /// </summary>
        /// <param name="deprecatedIdTypes">The new <see cref="DeprecatedIdTypes" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="GetAllRecordsByIdOp{TId}" /> using the specified <paramref name="deprecatedIdTypes" /> for <see cref="DeprecatedIdTypes" /> and a deep clone of every other property.</returns>
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
        public GetAllRecordsByIdOp<TId> DeepCloneWithDeprecatedIdTypes(IReadOnlyCollection<TypeRepresentation> deprecatedIdTypes)
        {
            var result = new GetAllRecordsByIdOp<TId>(
                                 this.Id == null ? default : this.Id.DeepClone(),
                                 this.ObjectType?.DeepClone(),
                                 this.VersionMatchStrategy.DeepClone(),
                                 this.TagsToMatch?.DeepClone(),
                                 this.TagMatchStrategy.DeepClone(),
                                 this.RecordNotFoundStrategy.DeepClone(),
                                 this.OrderRecordsBy.DeepClone(),
                                 deprecatedIdTypes,
                                 this.TypeSelectionStrategy.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="TypeSelectionStrategy" />.
        /// </summary>
        /// <param name="typeSelectionStrategy">The new <see cref="TypeSelectionStrategy" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="GetAllRecordsByIdOp{TId}" /> using the specified <paramref name="typeSelectionStrategy" /> for <see cref="TypeSelectionStrategy" /> and a deep clone of every other property.</returns>
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
        public GetAllRecordsByIdOp<TId> DeepCloneWithTypeSelectionStrategy(TypeSelectionStrategy typeSelectionStrategy)
        {
            var result = new GetAllRecordsByIdOp<TId>(
                                 this.Id == null ? default : this.Id.DeepClone(),
                                 this.ObjectType?.DeepClone(),
                                 this.VersionMatchStrategy.DeepClone(),
                                 this.TagsToMatch?.DeepClone(),
                                 this.TagMatchStrategy.DeepClone(),
                                 this.RecordNotFoundStrategy.DeepClone(),
                                 this.OrderRecordsBy.DeepClone(),
                                 this.DeprecatedIdTypes?.DeepClone(),
                                 typeSelectionStrategy);

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        protected override OperationBase DeepCloneInternal()
        {
            var result = new GetAllRecordsByIdOp<TId>(
                                 this.Id == null ? default : this.Id.DeepClone(),
                                 this.ObjectType?.DeepClone(),
                                 this.VersionMatchStrategy.DeepClone(),
                                 this.TagsToMatch?.DeepClone(),
                                 this.TagMatchStrategy.DeepClone(),
                                 this.RecordNotFoundStrategy.DeepClone(),
                                 this.OrderRecordsBy.DeepClone(),
                                 this.DeprecatedIdTypes?.DeepClone(),
                                 this.TypeSelectionStrategy.DeepClone());

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public override string ToString()
        {
            var result = Invariant($"Naos.Database.Domain.{this.GetType().ToStringReadable()}: Id = {this.Id?.ToString() ?? "<null>"}, ObjectType = {this.ObjectType?.ToString() ?? "<null>"}, VersionMatchStrategy = {this.VersionMatchStrategy.ToString() ?? "<null>"}, TagsToMatch = {this.TagsToMatch?.ToString() ?? "<null>"}, TagMatchStrategy = {this.TagMatchStrategy.ToString() ?? "<null>"}, RecordNotFoundStrategy = {this.RecordNotFoundStrategy.ToString() ?? "<null>"}, OrderRecordsBy = {this.OrderRecordsBy.ToString() ?? "<null>"}, DeprecatedIdTypes = {this.DeprecatedIdTypes?.ToString() ?? "<null>"}, TypeSelectionStrategy = {this.TypeSelectionStrategy.ToString() ?? "<null>"}.");

            return result;
        }
    }
}