// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteRecordOnHandlingCompletedOp{TId}.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System.Collections.Generic;
    using Naos.Database.Domain;
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
        /// <param name="checkSingleRecordHandlingOps">The <see cref="CheckRecordHandlingOp"/>'s to execute.</param>
        /// <param name="statusToRecordToWriteMap">The map of <see cref="HandlingStatus"/> to a <see cref="ObjectToPutWithId{TId}"/> to write.</param>
        /// <param name="handlingStatusCompositionStrategyForConcerns">The optional <see cref="HandlingStatusCompositionStrategy"/> to use when reducing the strategies of all concerns on a record.</param>
        /// <param name="handlingStatusCompositionStrategyForRecords">The optional <see cref="HandlingStatusCompositionStrategy"/> to use when reducing the strategies of all records (after reduced by concerns).</param>
        public WriteRecordOnHandlingCompletedOp(
            IReadOnlyCollection<CheckRecordHandlingOp> checkSingleRecordHandlingOps,
            IReadOnlyDictionary<HandlingStatus, ObjectToPutWithId<TId>> statusToRecordToWriteMap,
            HandlingStatusCompositionStrategy handlingStatusCompositionStrategyForConcerns = null,
            HandlingStatusCompositionStrategy handlingStatusCompositionStrategyForRecords = null)
        {
            checkSingleRecordHandlingOps.MustForArg(nameof(checkSingleRecordHandlingOps)).NotBeNullNorEmptyEnumerable();
            statusToRecordToWriteMap.MustForArg(nameof(statusToRecordToWriteMap)).NotBeNullNorEmptyDictionaryNorContainAnyNullValues();

            this.CheckSingleRecordHandlingOps = checkSingleRecordHandlingOps;
            this.StatusToRecordToWriteMap = statusToRecordToWriteMap;
            this.HandlingStatusCompositionStrategyForConcerns = handlingStatusCompositionStrategyForConcerns;
            this.HandlingStatusCompositionStrategyForRecords = handlingStatusCompositionStrategyForRecords;
        }

        /// <summary>
        /// Gets the <see cref="CheckRecordHandlingOp"/>'s to execute.
        /// </summary>
        /// <value>The <see cref="CheckRecordHandlingOp"/>'s to execute.</value>
        public IReadOnlyCollection<CheckRecordHandlingOp> CheckSingleRecordHandlingOps { get; private set; }

        /// <summary>
        /// Gets the map of <see cref="HandlingStatus"/> to a <see cref="ObjectToPutWithId{TId}"/> to write.
        /// </summary>
        /// <value>The map of <see cref="HandlingStatus"/> to a <see cref="ObjectToPutWithId{TId}"/> to write.</value>
        public IReadOnlyDictionary<HandlingStatus, ObjectToPutWithId<TId>> StatusToRecordToWriteMap { get; private set; }

        /// <summary>
        /// Gets the <see cref="HandlingStatusCompositionStrategy"/> to use when reducing the strategies of all concerns on a record.
        /// </summary>
        /// <value>The <see cref="HandlingStatusCompositionStrategy"/> to use when reducing the strategies of all concerns on a record.</value>
        public HandlingStatusCompositionStrategy HandlingStatusCompositionStrategyForConcerns { get; private set; }

        /// <summary>
        /// Gets the <see cref="HandlingStatusCompositionStrategy"/> to use when reducing the strategies of all records (after reduced by concerns).
        /// </summary>
        /// <value>The <see cref="HandlingStatusCompositionStrategy"/> to use when reducing the strategies of all records (after reduced by concerns).</value>
        public HandlingStatusCompositionStrategy HandlingStatusCompositionStrategyForRecords { get; private set; }
    }
}
