// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteRecordOnMatchingRecordFilterOp{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using System.Collections.Generic;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Saga in effect, an operation to check existence of record(s) and write event(s) based on matching criteria.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier for <see cref="EventToPutWithId{TId}"/>.</typeparam>
    public partial class WriteEventOnMatchingRecordFilterOp<TId> : VoidOperationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteEventOnMatchingRecordFilterOp{TId}"/> class.
        /// </summary>
        /// <param name="checkRecordExistsOps">The <see cref="CheckRecordHandlingOp"/>'s to execute.</param>
        /// <param name="eventToPutOnMatchChainOfResponsibility">The list of <see cref="EventToPutWithIdOnRecordFilterMatch{TId}"/> links to check for a match and write event as appropriate.</param>
        /// <param name="waitTimeBeforeRetry">The wait time before retry checking statuses.</param>
        public WriteEventOnMatchingRecordFilterOp(
            IReadOnlyCollection<CheckRecordExistsOp> checkRecordExistsOps,
            IReadOnlyList<EventToPutWithIdOnRecordFilterMatch<TId>> eventToPutOnMatchChainOfResponsibility,
            TimeSpan waitTimeBeforeRetry)
        {
            checkRecordExistsOps.MustForArg(nameof(checkRecordExistsOps)).NotBeNullNorEmptyEnumerableNorContainAnyNulls();
            eventToPutOnMatchChainOfResponsibility.MustForArg(nameof(eventToPutOnMatchChainOfResponsibility)).NotBeNullNorEmptyEnumerableNorContainAnyNulls();

            this.CheckRecordExistsOps = checkRecordExistsOps;
            this.EventToPutOnMatchChainOfResponsibility = eventToPutOnMatchChainOfResponsibility;
            this.WaitTimeBeforeRetry = waitTimeBeforeRetry;
        }

        /// <summary>
        /// Gets the <see cref="CheckRecordHandlingOp"/>'s to execute.
        /// </summary>
        public IReadOnlyCollection<CheckRecordExistsOp> CheckRecordExistsOps { get; private set; }

        /// <summary>
        /// Gets the list of <see cref="EventToPutWithIdOnRecordFilterMatch{TId}"/> links to check for a match and write event as appropriate.
        /// </summary>
        public IReadOnlyList<EventToPutWithIdOnRecordFilterMatch<TId>> EventToPutOnMatchChainOfResponsibility { get; private set; }

        /// <summary>
        /// Gets the wait time before retry checking statuses.
        /// </summary>
        public TimeSpan WaitTimeBeforeRetry { get; private set; }
    }
}
