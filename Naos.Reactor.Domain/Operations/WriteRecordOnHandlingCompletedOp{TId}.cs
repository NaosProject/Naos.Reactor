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
    /// Saga in effect, an operation to check on handling status of records and complete an action under certain scenarios.
    /// </summary>
    /// <typeparam name="TId">The type of the identifier.</typeparam>
    public partial class WriteRecordOnHandlingCompletedOp<TId> : VoidOperationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteRecordOnHandlingCompletedOp{TId}"/> class.
        /// </summary>
        /// <param name="checkRecordHandlingOps">The <see cref="CheckRecordHandlingOp"/>'s to execute.</param>
        /// <param name="eventToPutOnMatchChainOfResponsibility">The list of <see cref="EventToPutWithIdOnMatch{TId}"/> links to check for a match and write event as appropriate.</param>
        /// <param name="waitTimeBeforeRetry">The wait time before retry checking statuses.</param>
        public WriteRecordOnHandlingCompletedOp(
            IReadOnlyCollection<CheckRecordHandlingOp> checkRecordHandlingOps,
            IReadOnlyList<EventToPutWithIdOnMatch<TId>> eventToPutOnMatchChainOfResponsibility,
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
        /// Gets the list of <see cref="EventToPutWithIdOnMatch{TId}"/> links to check for a match and write event as appropriate.
        /// </summary>
        public IReadOnlyList<EventToPutWithIdOnMatch<TId>> EventToPutOnMatchChainOfResponsibility { get; private set; }

        /// <summary>
        /// Gets the wait time before retry checking statuses.
        /// </summary>
        public TimeSpan WaitTimeBeforeRetry { get; private set; }
    }
}
