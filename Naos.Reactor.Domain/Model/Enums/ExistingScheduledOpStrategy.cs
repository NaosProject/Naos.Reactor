// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExistingScheduledOpStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    /// <summary>
    /// Enumeration of how to deal with the execution of a <see cref="ScheduledExecuteOpRequestedEvent{TOperation}" /> when it's prior one is still running.
    /// </summary>
    public enum ScheduledOpAlreadyRunningStrategy
    {
        /// <summary>
        /// Unknown (default).
        /// </summary>
        Unknown,

        /// <summary>
        /// Execute a new task without waiting for the currently running one to finish.
        /// </summary>
        ExecuteNewInParallel,

        /// <summary>
        /// Skip this execution and wait for the schedule to add a new one.
        /// </summary>
        Skip,

        /// <summary>
        /// Wait until current execution is done and then execute immediately after.
        /// </summary>
        ExecuteNewWhenComplete,
    }
}
