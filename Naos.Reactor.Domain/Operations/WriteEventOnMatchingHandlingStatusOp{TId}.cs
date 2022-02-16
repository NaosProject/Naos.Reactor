// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteRecordOnHandlingCompletedOp{TId}.cs" company="Naos Project">
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
    /// Saga in effect, an operation to check on handling status of record(s) and write event(s) based on matching criteria.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier for <see cref="EventToPutWithId{TId}"/>.</typeparam>
    public partial class WriteEventOnMatchingHandlingStatusOp<TId> : VoidOperationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteEventOnMatchingHandlingStatusOp{TId}"/> class.
        /// </summary>
        /// <param name="checkRecordHandlingOps">The <see cref="CheckRecordHandlingOp"/>'s to execute.</param>
        /// <param name="eventToPutOnMatchChainOfResponsibility">The list of <see cref="EventToPutWithIdOnHandlingStatusMatch{TId}"/> links to check for a match and write event as appropriate.</param>
        /// <param name="waitTimeBeforeRetry">The wait time before retry checking statuses.</param>
        public WriteEventOnMatchingHandlingStatusOp(
            IReadOnlyCollection<CheckRecordHandlingOp> checkRecordHandlingOps,
            IReadOnlyList<EventToPutWithIdOnHandlingStatusMatch<TId>> eventToPutOnMatchChainOfResponsibility,
            TimeSpan waitTimeBeforeRetry)
        {
            checkRecordHandlingOps.MustForArg(nameof(checkRecordHandlingOps)).NotBeNullNorEmptyEnumerable();
            eventToPutOnMatchChainOfResponsibility.MustForArg(nameof(eventToPutOnMatchChainOfResponsibility)).NotBeNullNorEmptyDictionaryNorContainAnyNullValues();

            this.CheckRecordHandlingOps = checkRecordHandlingOps;
            this.EventToPutOnMatchChainOfResponsibility = eventToPutOnMatchChainOfResponsibility;
            this.WaitTimeBeforeRetry = waitTimeBeforeRetry;
        }

        /// <summary>
        /// Gets the <see cref="CheckRecordHandlingOp"/>'s to execute.
        /// </summary>
        public IReadOnlyCollection<CheckRecordHandlingOp> CheckRecordHandlingOps { get; private set; }

        /// <summary>
        /// Gets the list of <see cref="EventToPutWithIdOnHandlingStatusMatch{TId}"/> links to check for a match and write event as appropriate.
        /// </summary>
        public IReadOnlyList<EventToPutWithIdOnHandlingStatusMatch<TId>> EventToPutOnMatchChainOfResponsibility { get; private set; }

        /// <summary>
        /// Gets the wait time before retry checking statuses.
        /// </summary>
        public TimeSpan WaitTimeBeforeRetry { get; private set; }
    }
}
