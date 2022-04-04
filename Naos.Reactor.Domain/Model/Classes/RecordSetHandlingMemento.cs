// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordSetHandlingMemento.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;

    /// <summary>
    /// A memento design pattern container to store the state that needs to be completed or rolled back.
    /// </summary>
    public class RecordSetHandlingMemento
    {
        private readonly Action completeAction;
        private readonly Action cancelAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordSetHandlingMemento"/> class.
        /// </summary>
        /// <param name="completeAction">The action that completes the handling cycle of the records.</param>
        /// <param name="cancelAction">The action that cancels the handling cycle of the records.</param>
        public RecordSetHandlingMemento(
            Action completeAction,
            Action cancelAction)
        {
            completeAction.MustForArg(nameof(completeAction)).NotBeNull();
            cancelAction.MustForArg(nameof(cancelAction)).NotBeNull();

            this.completeAction = completeAction;
            this.cancelAction = cancelAction;
        }

        /// <summary>
        /// Completes the handling cycle of the records.
        /// </summary>
        public void CompleteSet() => this.completeAction();

        /// <summary>
        /// Cancels the handling cycle of the records.
        /// </summary>
        public void CancelSet() => this.cancelAction();
    }
}
