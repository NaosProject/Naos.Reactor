// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordExistsMatchStrategyExtensions.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Naos.Database.Domain;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Enum.Recipes;
    using static System.FormattableString;

    /// <summary>
    /// Extensions on <see cref="RecordExistsMatchStrategy"/>.
    /// </summary>
    public static class RecordExistsMatchStrategyExtensions
    {
        /// <summary>
        /// Matches to set using the provided <see cref="RecordExistsMatchStrategy"/>.
        /// </summary>
        /// <param name="recordExistsSet">The set to inspect.</param>
        /// <param name="recordExistsMatchStrategy">The strategy to use for evaluating the set.</param>
        /// <returns>
        /// <c>true</c> if the set match, otherwise <c>false</c>.
        /// </returns>
        public static bool MatchesAccordingToStrategy(
            this IReadOnlyCollection<bool> recordExistsSet,
            RecordExistsMatchStrategy recordExistsMatchStrategy)
        {
            recordExistsSet.MustForArg(nameof(recordExistsSet)).NotBeNullNorEmptyEnumerableNorContainAnyNulls();
            recordExistsMatchStrategy.MustForArg(nameof(recordExistsMatchStrategy)).NotBeEqualTo(RecordExistsMatchStrategy.Unknown);

            bool result;

            if (recordExistsMatchStrategy == RecordExistsMatchStrategy.AllFound)
            {
                result = recordExistsSet.All(_ => _);
            }
            else if (recordExistsMatchStrategy == RecordExistsMatchStrategy.SomeFound)
            {
                result = recordExistsSet.Any(_ => _);
            }
            else if (recordExistsMatchStrategy == RecordExistsMatchStrategy.NoneFound)
            {
                result = recordExistsSet.All(_ => !_);
            }
            else
            {
                throw new NotSupportedException(Invariant($"This {nameof(RecordExistsMatchStrategy)} is not supported {recordExistsMatchStrategy}."));
            }

            return result;
        }
    }
}
