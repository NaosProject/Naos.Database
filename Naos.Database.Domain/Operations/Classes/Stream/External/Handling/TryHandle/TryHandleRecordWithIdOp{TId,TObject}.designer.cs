﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.174.0)
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
    public partial class TryHandleRecordWithIdOp<TId, TObject> : IModel<TryHandleRecordWithIdOp<TId, TObject>>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="TryHandleRecordWithIdOp{TId, TObject}"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(TryHandleRecordWithIdOp<TId, TObject> left, TryHandleRecordWithIdOp<TId, TObject> right)
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
        /// Determines whether two objects of type <see cref="TryHandleRecordWithIdOp{TId, TObject}"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(TryHandleRecordWithIdOp<TId, TObject> left, TryHandleRecordWithIdOp<TId, TObject> right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(TryHandleRecordWithIdOp<TId, TObject> other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.Concern.IsEqualTo(other.Concern, StringComparer.Ordinal)
                      && this.VersionMatchStrategy.IsEqualTo(other.VersionMatchStrategy)
                      && this.TagsToMatch.IsEqualTo(other.TagsToMatch)
                      && this.TagMatchStrategy.IsEqualTo(other.TagMatchStrategy)
                      && this.OrderRecordsBy.IsEqualTo(other.OrderRecordsBy)
                      && this.Tags.IsEqualTo(other.Tags)
                      && this.Details.IsEqualTo(other.Details, StringComparer.Ordinal)
                      && this.MinimumInternalRecordId.IsEqualTo(other.MinimumInternalRecordId)
                      && this.InheritRecordTags.IsEqualTo(other.InheritRecordTags);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as TryHandleRecordWithIdOp<TId, TObject>);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.Concern)
            .Hash(this.VersionMatchStrategy)
            .Hash(this.TagsToMatch)
            .Hash(this.TagMatchStrategy)
            .Hash(this.OrderRecordsBy)
            .Hash(this.Tags)
            .Hash(this.Details)
            .Hash(this.MinimumInternalRecordId)
            .Hash(this.InheritRecordTags)
            .Value;

        /// <inheritdoc />
        public new TryHandleRecordWithIdOp<TId, TObject> DeepClone() => (TryHandleRecordWithIdOp<TId, TObject>)this.DeepCloneInternal();

        /// <summary>
        /// Deep clones this object with a new <see cref="Concern" />.
        /// </summary>
        /// <param name="concern">The new <see cref="Concern" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="TryHandleRecordWithIdOp{TId, TObject}" /> using the specified <paramref name="concern" /> for <see cref="Concern" /> and a deep clone of every other property.</returns>
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
        public TryHandleRecordWithIdOp<TId, TObject> DeepCloneWithConcern(string concern)
        {
            var result = new TryHandleRecordWithIdOp<TId, TObject>(
                                 concern,
                                 this.VersionMatchStrategy.DeepClone(),
                                 this.TagsToMatch?.DeepClone(),
                                 this.TagMatchStrategy.DeepClone(),
                                 this.OrderRecordsBy.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 this.Details?.DeepClone(),
                                 this.MinimumInternalRecordId?.DeepClone(),
                                 this.InheritRecordTags.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="VersionMatchStrategy" />.
        /// </summary>
        /// <param name="versionMatchStrategy">The new <see cref="VersionMatchStrategy" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="TryHandleRecordWithIdOp{TId, TObject}" /> using the specified <paramref name="versionMatchStrategy" /> for <see cref="VersionMatchStrategy" /> and a deep clone of every other property.</returns>
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
        public TryHandleRecordWithIdOp<TId, TObject> DeepCloneWithVersionMatchStrategy(VersionMatchStrategy versionMatchStrategy)
        {
            var result = new TryHandleRecordWithIdOp<TId, TObject>(
                                 this.Concern?.DeepClone(),
                                 versionMatchStrategy,
                                 this.TagsToMatch?.DeepClone(),
                                 this.TagMatchStrategy.DeepClone(),
                                 this.OrderRecordsBy.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 this.Details?.DeepClone(),
                                 this.MinimumInternalRecordId?.DeepClone(),
                                 this.InheritRecordTags.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="TagsToMatch" />.
        /// </summary>
        /// <param name="tagsToMatch">The new <see cref="TagsToMatch" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="TryHandleRecordWithIdOp{TId, TObject}" /> using the specified <paramref name="tagsToMatch" /> for <see cref="TagsToMatch" /> and a deep clone of every other property.</returns>
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
        public TryHandleRecordWithIdOp<TId, TObject> DeepCloneWithTagsToMatch(IReadOnlyCollection<NamedValue<string>> tagsToMatch)
        {
            var result = new TryHandleRecordWithIdOp<TId, TObject>(
                                 this.Concern?.DeepClone(),
                                 this.VersionMatchStrategy.DeepClone(),
                                 tagsToMatch,
                                 this.TagMatchStrategy.DeepClone(),
                                 this.OrderRecordsBy.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 this.Details?.DeepClone(),
                                 this.MinimumInternalRecordId?.DeepClone(),
                                 this.InheritRecordTags.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="TagMatchStrategy" />.
        /// </summary>
        /// <param name="tagMatchStrategy">The new <see cref="TagMatchStrategy" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="TryHandleRecordWithIdOp{TId, TObject}" /> using the specified <paramref name="tagMatchStrategy" /> for <see cref="TagMatchStrategy" /> and a deep clone of every other property.</returns>
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
        public TryHandleRecordWithIdOp<TId, TObject> DeepCloneWithTagMatchStrategy(TagMatchStrategy tagMatchStrategy)
        {
            var result = new TryHandleRecordWithIdOp<TId, TObject>(
                                 this.Concern?.DeepClone(),
                                 this.VersionMatchStrategy.DeepClone(),
                                 this.TagsToMatch?.DeepClone(),
                                 tagMatchStrategy,
                                 this.OrderRecordsBy.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 this.Details?.DeepClone(),
                                 this.MinimumInternalRecordId?.DeepClone(),
                                 this.InheritRecordTags.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="OrderRecordsBy" />.
        /// </summary>
        /// <param name="orderRecordsBy">The new <see cref="OrderRecordsBy" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="TryHandleRecordWithIdOp{TId, TObject}" /> using the specified <paramref name="orderRecordsBy" /> for <see cref="OrderRecordsBy" /> and a deep clone of every other property.</returns>
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
        public TryHandleRecordWithIdOp<TId, TObject> DeepCloneWithOrderRecordsBy(OrderRecordsBy orderRecordsBy)
        {
            var result = new TryHandleRecordWithIdOp<TId, TObject>(
                                 this.Concern?.DeepClone(),
                                 this.VersionMatchStrategy.DeepClone(),
                                 this.TagsToMatch?.DeepClone(),
                                 this.TagMatchStrategy.DeepClone(),
                                 orderRecordsBy,
                                 this.Tags?.DeepClone(),
                                 this.Details?.DeepClone(),
                                 this.MinimumInternalRecordId?.DeepClone(),
                                 this.InheritRecordTags.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="Tags" />.
        /// </summary>
        /// <param name="tags">The new <see cref="Tags" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="TryHandleRecordWithIdOp{TId, TObject}" /> using the specified <paramref name="tags" /> for <see cref="Tags" /> and a deep clone of every other property.</returns>
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
        public TryHandleRecordWithIdOp<TId, TObject> DeepCloneWithTags(IReadOnlyCollection<NamedValue<string>> tags)
        {
            var result = new TryHandleRecordWithIdOp<TId, TObject>(
                                 this.Concern?.DeepClone(),
                                 this.VersionMatchStrategy.DeepClone(),
                                 this.TagsToMatch?.DeepClone(),
                                 this.TagMatchStrategy.DeepClone(),
                                 this.OrderRecordsBy.DeepClone(),
                                 tags,
                                 this.Details?.DeepClone(),
                                 this.MinimumInternalRecordId?.DeepClone(),
                                 this.InheritRecordTags.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="Details" />.
        /// </summary>
        /// <param name="details">The new <see cref="Details" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="TryHandleRecordWithIdOp{TId, TObject}" /> using the specified <paramref name="details" /> for <see cref="Details" /> and a deep clone of every other property.</returns>
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
        public TryHandleRecordWithIdOp<TId, TObject> DeepCloneWithDetails(string details)
        {
            var result = new TryHandleRecordWithIdOp<TId, TObject>(
                                 this.Concern?.DeepClone(),
                                 this.VersionMatchStrategy.DeepClone(),
                                 this.TagsToMatch?.DeepClone(),
                                 this.TagMatchStrategy.DeepClone(),
                                 this.OrderRecordsBy.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 details,
                                 this.MinimumInternalRecordId?.DeepClone(),
                                 this.InheritRecordTags.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="MinimumInternalRecordId" />.
        /// </summary>
        /// <param name="minimumInternalRecordId">The new <see cref="MinimumInternalRecordId" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="TryHandleRecordWithIdOp{TId, TObject}" /> using the specified <paramref name="minimumInternalRecordId" /> for <see cref="MinimumInternalRecordId" /> and a deep clone of every other property.</returns>
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
        public TryHandleRecordWithIdOp<TId, TObject> DeepCloneWithMinimumInternalRecordId(long? minimumInternalRecordId)
        {
            var result = new TryHandleRecordWithIdOp<TId, TObject>(
                                 this.Concern?.DeepClone(),
                                 this.VersionMatchStrategy.DeepClone(),
                                 this.TagsToMatch?.DeepClone(),
                                 this.TagMatchStrategy.DeepClone(),
                                 this.OrderRecordsBy.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 this.Details?.DeepClone(),
                                 minimumInternalRecordId,
                                 this.InheritRecordTags.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="InheritRecordTags" />.
        /// </summary>
        /// <param name="inheritRecordTags">The new <see cref="InheritRecordTags" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="TryHandleRecordWithIdOp{TId, TObject}" /> using the specified <paramref name="inheritRecordTags" /> for <see cref="InheritRecordTags" /> and a deep clone of every other property.</returns>
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
        public TryHandleRecordWithIdOp<TId, TObject> DeepCloneWithInheritRecordTags(bool inheritRecordTags)
        {
            var result = new TryHandleRecordWithIdOp<TId, TObject>(
                                 this.Concern?.DeepClone(),
                                 this.VersionMatchStrategy.DeepClone(),
                                 this.TagsToMatch?.DeepClone(),
                                 this.TagMatchStrategy.DeepClone(),
                                 this.OrderRecordsBy.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 this.Details?.DeepClone(),
                                 this.MinimumInternalRecordId?.DeepClone(),
                                 inheritRecordTags);

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        protected override OperationBase DeepCloneInternal()
        {
            var result = new TryHandleRecordWithIdOp<TId, TObject>(
                                 this.Concern?.DeepClone(),
                                 this.VersionMatchStrategy.DeepClone(),
                                 this.TagsToMatch?.DeepClone(),
                                 this.TagMatchStrategy.DeepClone(),
                                 this.OrderRecordsBy.DeepClone(),
                                 this.Tags?.DeepClone(),
                                 this.Details?.DeepClone(),
                                 this.MinimumInternalRecordId?.DeepClone(),
                                 this.InheritRecordTags.DeepClone());

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public override string ToString()
        {
            var result = Invariant($"Naos.Database.Domain.{this.GetType().ToStringReadable()}: Concern = {this.Concern?.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, VersionMatchStrategy = {this.VersionMatchStrategy.ToString() ?? "<null>"}, TagsToMatch = {this.TagsToMatch?.ToString() ?? "<null>"}, TagMatchStrategy = {this.TagMatchStrategy.ToString() ?? "<null>"}, OrderRecordsBy = {this.OrderRecordsBy.ToString() ?? "<null>"}, Tags = {this.Tags?.ToString() ?? "<null>"}, Details = {this.Details?.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, MinimumInternalRecordId = {this.MinimumInternalRecordId?.ToString(CultureInfo.InvariantCulture) ?? "<null>"}, InheritRecordTags = {this.InheritRecordTags.ToString(CultureInfo.InvariantCulture) ?? "<null>"}.");

            return result;
        }
    }
}