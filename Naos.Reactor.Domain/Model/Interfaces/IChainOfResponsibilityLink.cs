// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IChainOfResponsibilityLink.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    /// <summary>
    /// Chain of Responsibility Pattern link interface.
    /// </summary>
    public interface IChainOfResponsibilityLink
    {
        /// <summary>
        /// Gets a value indicating whether or not to terminate the chain of responsibility on match.
        /// </summary>
        bool MatchTerminatesChain { get; }

        /// <summary>
        /// Gets a value indicating whether or not to terminate the larger execution context on match.
        /// </summary>
        bool MatchTerminatesExecution { get; }

    }
}
