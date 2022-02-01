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
        /// <param name="checkRecordHandlingOps">The <see cref="CheckRecordHandlingOp"/>'s to execute.</param>
        /// <param name="statusToRecordToWriteMap">The map of <see cref="CompositeHandlingStatus"/> to a <see cref="EventToPutWithId{TId}"/> to write.</param>
        public WriteRecordOnHandlingCompletedOp(
            IReadOnlyCollection<CheckRecordHandlingOp> checkRecordHandlingOps,
            IReadOnlyDictionary<CompositeHandlingStatus, EventToPutWithId<TId>> statusToRecordToWriteMap)
        {
            checkRecordHandlingOps.MustForArg(nameof(checkRecordHandlingOps)).NotBeNullNorEmptyEnumerable();
            statusToRecordToWriteMap.MustForArg(nameof(statusToRecordToWriteMap)).NotBeNullNorEmptyDictionaryNorContainAnyNullValues();

            this.CheckRecordHandlingOps = checkRecordHandlingOps;
            this.StatusToRecordToWriteMap = statusToRecordToWriteMap;
        }

        /// <summary>
        /// Gets the <see cref="CheckRecordHandlingOp"/>'s to execute.
        /// </summary>
        /// <value>The <see cref="CheckRecordHandlingOp"/>'s to execute.</value>
        public IReadOnlyCollection<CheckRecordHandlingOp> CheckRecordHandlingOps { get; private set; }

        /// <summary>
        /// Gets the map of <see cref="CompositeHandlingStatus"/> to a <see cref="EventToPutWithId{TId}"/> to write.
        /// </summary>
        /// <value>The map of <see cref="CompositeHandlingStatus"/> to a <see cref="EventToPutWithId{TId}"/> to write.</value>
        public IReadOnlyDictionary<CompositeHandlingStatus, EventToPutWithId<TId>> StatusToRecordToWriteMap { get; private set; }
    }
}
