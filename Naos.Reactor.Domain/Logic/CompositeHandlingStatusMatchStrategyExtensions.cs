// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckCompositeStatusHandlingResult.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using System.Linq;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Enum.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Extensions on <see cref="CompositeHandlingStatusMatchStrategy"/>.
    /// </summary>
    public static class CompositeHandlingStatusMatchStrategyExtensions
    {
        /// <summary>
        /// Matches one <see cref="CompositeHandlingStatus"/> to another using the provided <see cref="CompositeHandlingStatusMatchStrategy"/>.
        /// </summary>
        /// <param name="actualStatus">The status to inspect.</param>
        /// <param name="statusToMatch">The status to compare it to.</param>
        /// <param name="compositeHandlingStatusMatchStrategy">The strategy to use for comparing tags.</param>
        /// <returns>
        /// <c>true</c> if the tags match, otherwise <c>false</c>.
        /// </returns>
        public static bool MatchesAccordingToStrategy(
            this CompositeHandlingStatus actualStatus,
            CompositeHandlingStatus statusToMatch,
            CompositeHandlingStatusMatchStrategy compositeHandlingStatusMatchStrategy)
        {
            actualStatus.MustForArg(nameof(actualStatus)).NotBeEqualTo(CompositeHandlingStatus.Unknown);
            statusToMatch.MustForArg(nameof(statusToMatch)).NotBeEqualTo(CompositeHandlingStatus.Unknown);
            compositeHandlingStatusMatchStrategy.MustForArg(nameof(compositeHandlingStatusMatchStrategy)).NotBeEqualTo(CompositeHandlingStatusMatchStrategy.Unknown);

            var actualFlags = actualStatus.GetIndividualFlags<CompositeHandlingStatus>().Where(_ => _ != CompositeHandlingStatus.Unknown).ToArray();
            var flagsToMatch = statusToMatch.GetIndividualFlags<CompositeHandlingStatus>().Where(_ => _ != CompositeHandlingStatus.Unknown).ToArray();

            bool result;

            if (compositeHandlingStatusMatchStrategy == CompositeHandlingStatusMatchStrategy.ActualCompositeStatusHasAnyQueryCompositeStatusFlag)
            {
                result = flagsToMatch.Intersect(actualFlags).Any();
            }
            else if (compositeHandlingStatusMatchStrategy == CompositeHandlingStatusMatchStrategy.ActualCompositeStatusHasAllQueryCompositeStatusFlags)
            {
                result = !flagsToMatch.Except(actualFlags).Any();
            }
            else if (compositeHandlingStatusMatchStrategy == CompositeHandlingStatusMatchStrategy.ActualCompositeStatusEqualsQueryCompositeStatus)
            {
                result = actualStatus == statusToMatch;
            }
            else
            {
                throw new NotSupportedException(Invariant($"This {nameof(CompositeHandlingStatusMatchStrategy)} is not supported {compositeHandlingStatusMatchStrategy}."));
            }

            return result;
        }
    }
}
