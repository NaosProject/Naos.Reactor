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
        /// Gets the chain of responsibility link match strategy.
        /// </summary>
        ChainOfResponsibilityLinkMatchStrategy ChainOfResponsibilityLinkMatchStrategy { get; }
    }
}
