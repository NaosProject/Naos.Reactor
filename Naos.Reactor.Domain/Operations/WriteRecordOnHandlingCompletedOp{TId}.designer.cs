﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.177.0)
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
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
    public partial class WriteRecordOnHandlingCompletedOp<TId> : IModel<WriteRecordOnHandlingCompletedOp<TId>>
    {
        /// <summary>
        /// Determines whether two objects of type <see cref="WriteRecordOnHandlingCompletedOp{TId}"/> are equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are equal; otherwise false.</returns>
        public static bool operator ==(WriteRecordOnHandlingCompletedOp<TId> left, WriteRecordOnHandlingCompletedOp<TId> right)
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
        /// Determines whether two objects of type <see cref="WriteRecordOnHandlingCompletedOp{TId}"/> are not equal.
        /// </summary>
        /// <param name="left">The object to the left of the equality operator.</param>
        /// <param name="right">The object to the right of the equality operator.</param>
        /// <returns>true if the two items are not equal; otherwise false.</returns>
        public static bool operator !=(WriteRecordOnHandlingCompletedOp<TId> left, WriteRecordOnHandlingCompletedOp<TId> right) => !(left == right);

        /// <inheritdoc />
        public bool Equals(WriteRecordOnHandlingCompletedOp<TId> other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (ReferenceEquals(other, null))
            {
                return false;
            }

            var result = this.CheckRecordHandlingOps.IsEqualTo(other.CheckRecordHandlingOps)
                      && this.EventToPutOnMatchChainOfResponsibility.IsEqualTo(other.EventToPutOnMatchChainOfResponsibility)
                      && this.WaitTimeBeforeRetry.IsEqualTo(other.WaitTimeBeforeRetry);

            return result;
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => this == (obj as WriteRecordOnHandlingCompletedOp<TId>);

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeHelper.Initialize()
            .Hash(this.CheckRecordHandlingOps)
            .Hash(this.EventToPutOnMatchChainOfResponsibility)
            .Hash(this.WaitTimeBeforeRetry)
            .Value;

        /// <inheritdoc />
        public new WriteRecordOnHandlingCompletedOp<TId> DeepClone() => (WriteRecordOnHandlingCompletedOp<TId>)this.DeepCloneInternal();

        /// <summary>
        /// Deep clones this object with a new <see cref="CheckRecordHandlingOps" />.
        /// </summary>
        /// <param name="checkRecordHandlingOps">The new <see cref="CheckRecordHandlingOps" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="WriteRecordOnHandlingCompletedOp{TId}" /> using the specified <paramref name="checkRecordHandlingOps" /> for <see cref="CheckRecordHandlingOps" /> and a deep clone of every other property.</returns>
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
        public WriteRecordOnHandlingCompletedOp<TId> DeepCloneWithCheckRecordHandlingOps(IReadOnlyCollection<CheckRecordHandlingOp> checkRecordHandlingOps)
        {
            var result = new WriteRecordOnHandlingCompletedOp<TId>(
                                 checkRecordHandlingOps,
                                 this.EventToPutOnMatchChainOfResponsibility?.DeepClone(),
                                 this.WaitTimeBeforeRetry.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="EventToPutOnMatchChainOfResponsibility" />.
        /// </summary>
        /// <param name="eventToPutOnMatchChainOfResponsibility">The new <see cref="EventToPutOnMatchChainOfResponsibility" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="WriteRecordOnHandlingCompletedOp{TId}" /> using the specified <paramref name="eventToPutOnMatchChainOfResponsibility" /> for <see cref="EventToPutOnMatchChainOfResponsibility" /> and a deep clone of every other property.</returns>
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
        public WriteRecordOnHandlingCompletedOp<TId> DeepCloneWithEventToPutOnMatchChainOfResponsibility(IReadOnlyList<EventToPutWithIdOnMatch<TId>> eventToPutOnMatchChainOfResponsibility)
        {
            var result = new WriteRecordOnHandlingCompletedOp<TId>(
                                 this.CheckRecordHandlingOps?.DeepClone(),
                                 eventToPutOnMatchChainOfResponsibility,
                                 this.WaitTimeBeforeRetry.DeepClone());

            return result;
        }

        /// <summary>
        /// Deep clones this object with a new <see cref="WaitTimeBeforeRetry" />.
        /// </summary>
        /// <param name="waitTimeBeforeRetry">The new <see cref="WaitTimeBeforeRetry" />.  This object will NOT be deep cloned; it is used as-is.</param>
        /// <returns>New <see cref="WriteRecordOnHandlingCompletedOp{TId}" /> using the specified <paramref name="waitTimeBeforeRetry" /> for <see cref="WaitTimeBeforeRetry" /> and a deep clone of every other property.</returns>
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
        public WriteRecordOnHandlingCompletedOp<TId> DeepCloneWithWaitTimeBeforeRetry(TimeSpan waitTimeBeforeRetry)
        {
            var result = new WriteRecordOnHandlingCompletedOp<TId>(
                                 this.CheckRecordHandlingOps?.DeepClone(),
                                 this.EventToPutOnMatchChainOfResponsibility?.DeepClone(),
                                 waitTimeBeforeRetry);

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        protected override OperationBase DeepCloneInternal()
        {
            var result = new WriteRecordOnHandlingCompletedOp<TId>(
                                 this.CheckRecordHandlingOps?.DeepClone(),
                                 this.EventToPutOnMatchChainOfResponsibility?.DeepClone(),
                                 this.WaitTimeBeforeRetry.DeepClone());

            return result;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        public override string ToString()
        {
            var result = Invariant($"Naos.Reactor.Domain.{this.GetType().ToStringReadable()}: CheckRecordHandlingOps = {this.CheckRecordHandlingOps?.ToString() ?? "<null>"}, EventToPutOnMatchChainOfResponsibility = {this.EventToPutOnMatchChainOfResponsibility?.ToString() ?? "<null>"}, WaitTimeBeforeRetry = {this.WaitTimeBeforeRetry.ToString() ?? "<null>"}.");

            return result;
        }
    }
}