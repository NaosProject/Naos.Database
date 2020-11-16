// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetStreamFromRepresentationOp{TStreamRepresentation,TStream}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Database.Domain
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Naos.Protocol.Domain;
    using OBeautifulCode.CodeAnalysis.Recipes;
    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Operation to get a specific <see cref="IStream"/> by a specific <see cref="IStreamRepresentation"/>.
    /// </summary>
    /// <typeparam name="TStreamRepresentation">Type of <see cref="IStreamRepresentation"/> to use.</typeparam>
    /// <typeparam name="TStream">Type of <see cref="IStream"/> to get.</typeparam>
    public partial class GetStreamFromRepresentationOp<TStreamRepresentation, TStream> :
        ReturningOperationBase<TStream>,
        IForsakeInheritedModelViaCodeGen // this is because the tests generated will be the wrong type so the generated code was promoted up in both class and test.
        where TStreamRepresentation : IStreamRepresentation
        where TStream : IStream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetStreamFromRepresentationOp{TStreamRepresentation, TStream}"/> class.
        /// </summary>
        /// <param name="typedStreamRepresentation">The typed stream representation.</param>
        public GetStreamFromRepresentationOp(
            TStreamRepresentation typedStreamRepresentation)
        {
            this.TypedStreamRepresentation = typedStreamRepresentation;
        }

        /// <summary>
        /// Gets the stream representation.
        /// </summary>
        /// <value>The stream representation.</value>
        public IStreamRepresentation StreamRepresentation => this.TypedStreamRepresentation;

        /// <summary>
        /// Gets the typed stream representation.
        /// </summary>
        /// <value>The typed stream representation.</value>
        public TStreamRepresentation TypedStreamRepresentation { get; private set; }
    }

    [Serializable]
    public partial class GetStreamFromRepresentationOp<TStreamRepresentation, TStream> : IModel<GetStreamFromRepresentationOp<TStreamRepresentation, TStream>>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="GetStreamFromRepresentationOp{TStreamRepresentation, TStream}"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(GetStreamFromRepresentationOp<TStreamRepresentation, TStream> left, GetStreamFromRepresentationOp<TStreamRepresentation, TStream> right)
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
        /// Determines whether two objects of type <see cref="GetStreamFromRepresentationOp{TStreamRepresentation, TStream}"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(GetStreamFromRepresentationOp<TStreamRepresentation, TStream> left, GetStreamFromRepresentationOp<TStreamRepresentation, TStream> right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(GetStreamFromRepresentationOp<TStreamRepresentation, TStream> other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.TypedStreamRepresentation.IsEqualTo(other.TypedStreamRepresentation);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as GetStreamFromRepresentationOp<TStreamRepresentation, TStream>);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.TypedStreamRepresentation)
            .Value;

        /// <inheritdoc />
        public new GetStreamFromRepresentationOp<TStreamRepresentation, TStream> DeepClone() => (GetStreamFromRepresentationOp<TStreamRepresentation, TStream>)this.DeepCloneInternal();

        /// <summary>
        /// Deep clones this object with a new <see cref="TypedStreamRepresentation" />.
        /// </summary>
        /// <param name="typedStreamRepresentation">The new <see cref="TypedStreamRepresentation" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="GetStreamFromRepresentationOp{TStreamRepresentation, TStream}" /> using the specified <paramref name="typedStreamRepresentation" /> for <see cref="TypedStreamRepresentation" /> and a deep clone of every other property.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002: DoNotExposeGenericLists", Justification = ObcSuppressBecause.CA_ALL_NotApplicable)]
        [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", Justification = ObcSuppressBecause.CA_ALL_NotApplicable)]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = ObcSuppressBecause.CA_ALL_NotApplicable)]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = ObcSuppressBecause.CA_ALL_NotApplicable)]
        [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = ObcSuppressBecause.CA_ALL_NotApplicable)]
        [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = ObcSuppressBecause.CA_ALL_NotApplicable)]
        [SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix", Justification = ObcSuppressBecause.CA_ALL_NotApplicable)]
        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Justification = ObcSuppressBecause.CA_ALL_NotApplicable)]
        [SuppressMessage("Microsoft.Naming", "CA1719:ParameterNamesShouldNotMatchMemberNames", Justification = ObcSuppressBecause.CA_ALL_NotApplicable)]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Justification = ObcSuppressBecause.CA_ALL_NotApplicable)]
        [SuppressMessage("Microsoft.Naming", "CA1722:IdentifiersShouldNotHaveIncorrectPrefix", Justification = ObcSuppressBecause.CA_ALL_NotApplicable)]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", Justification = ObcSuppressBecause.CA_ALL_NotApplicable)]
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", Justification = ObcSuppressBecause.CA_ALL_NotApplicable)]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", Justification = ObcSuppressBecause.CA_ALL_NotApplicable)]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = ObcSuppressBecause.CA_ALL_NotApplicable)]
        public GetStreamFromRepresentationOp<TStreamRepresentation, TStream> DeepCloneWithTypedStreamRepresentation(TStreamRepresentation typedStreamRepresentation)
        {
            var result = new GetStreamFromRepresentationOp<TStreamRepresentation, TStream>(
                                 typedStreamRepresentation);

            return result;
        }

        /// <inheritdoc />
        protected override OperationBase DeepCloneInternal()
        {
            var result = new GetStreamFromRepresentationOp<TStreamRepresentation, TStream>(
                                 DeepCloneGeneric(this.TypedStreamRepresentation));

            return result;
        }

        private static TStreamRepresentation DeepCloneGeneric(TStreamRepresentation value)
        {
            object result;

            var type = typeof(TStreamRepresentation);

            if (type.IsValueType)
            {
                result = value;
            }
            else
            {
                if (ReferenceEquals(value, null))
                {
                    result = default;
                }
                else if (value is IDeepCloneable<TStreamRepresentation> deepCloneableValue)
                {
                    result = deepCloneableValue.DeepClone();
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
                    throw new NotSupportedException(Invariant($"I do not know how to deep clone an object of type '{type.ToStringReadable()}'"));
                }
            }

            return (TStreamRepresentation)result;
        }

        private static TStream DeepCloneGeneric(TStream value)
        {
            object result;

            var type = typeof(TStream);

            if (type.IsValueType)
            {
                result = value;
            }
            else
            {
                if (ReferenceEquals(value, null))
                {
                    result = default;
                }
                else if (value is IDeepCloneable<TStream> deepCloneableValue)
                {
                    result = deepCloneableValue.DeepClone();
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
                    throw new NotSupportedException(Invariant($"I do not know how to deep clone an object of type '{type.ToStringReadable()}'"));
                }
            }

            return (TStream)result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = ObcSuppressBecause.CA_ALL_NotApplicable)]
        public override string ToString()
        {
            var result = Invariant($"Naos.Database.Domain.{this.GetType().ToStringReadable()}: TypedStreamRepresentation = {this.TypedStreamRepresentation?.ToString() ?? "<null>"}.");

            return result;
        }
    }
}
