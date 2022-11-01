// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChainOfResponsibilityLinkMatchStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    /// <summary>
    /// Enumeration of the ways to deal with a match on a <see cref="IChainOfResponsibilityLink"/>.
    /// </summary>
    public enum ChainOfResponsibilityLinkMatchStrategy
    {
        /// <summary>
        /// Unknown (default).
        /// </summary>
        Unknown,

        /// <summary>
        /// Continue evaluating other links.
        /// </summary>
        Continue,

        /// <summary>
        /// On a match, halt the evaluation of the chain of responsibility and self-cancel handling.
        /// </summary>
        MatchHaltsEvaluationOfChainAndSelfCancels,

        /// <summary>
        ///  On a match, halt the evaluation of the chain of responsibility and complete handling.
        /// </summary>
        MatchHaltsEvaluationOfChainAndCompletes,

        /// <summary>
        ///  On a match, halt the evaluation of the chain of responsibility and fail handling.
        /// </summary>
        MatchHaltsEvaluationOfChainAndFails,
    }
}
