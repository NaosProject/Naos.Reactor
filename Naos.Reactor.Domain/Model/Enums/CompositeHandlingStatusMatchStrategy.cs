// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckCompositeStatusHandlingResult.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using Naos.CodeAnalysis.Recipes;

    /// <summary>
    /// Enumeration of the ways to match to <see cref="Naos.Database.Domain.CompositeHandlingStatus"/>.
    /// </summary>
    public enum CompositeHandlingStatusMatchStrategy
    {
        /// <summary>
        /// Unknown (default).
        /// </summary>
        Unknown,

        /// <summary>
        /// There is a match if the actual composite status contains any of the queried status flags. (This is not likely to be used and you should be certain this is the correct option if you're using it.)
        /// </summary>
        /// <remarks>
        /// Example:
        /// query statuses flags               : status1, status2, status3
        /// matching composite status flags    : status3, status4, status5
        /// non-matching composite status flags: status4, status5 (does not have any of the query status flags)
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flag", Justification = NaosSuppressBecause.CA1726_UsePreferredTerms_NameOfTypeOfIdentifierUsesTheTermFlags)]
        ActualCompositeStatusHasAnyQueryCompositeStatusFlag,

        /// <summary>
        /// There is a match if the actual composite status contains all of the queried composite status flags (with extra status flags on the composite status ignored).
        /// </summary>
        /// <remarks>
        /// Example:
        /// query statuses flags               : status1, status2, status3
        /// matching composite status flags    : status1, status2, status3, status4
        /// non-matching composite status flags: status1, status3, status4 (missing status2)
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags", Justification = NaosSuppressBecause.CA1726_UsePreferredTerms_NameOfTypeOfIdentifierUsesTheTermFlags)]
        ActualCompositeStatusHasAllQueryCompositeStatusFlags,

        /// <summary>
        /// There is a match if the actual composite status is exactly equal to the queried composite status (e.g. contains all of the queried status flags and no other status flags).
        /// </summary>
        /// <remarks>
        /// Example:
        /// query statuses flags               : status1, status2, status3
        /// matching composite status flags    : status1, status2, status3
        /// non-matching composite status flags: status1, status2 (missing status3)
        /// </remarks>
        ActualCompositeStatusEqualsQueryCompositeStatus,
    }
}
