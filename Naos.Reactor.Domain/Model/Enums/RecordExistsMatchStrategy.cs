// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordExistsMatchStrategy.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    /// <summary>
    /// Enumeration of the ways to match existing records.
    /// </summary>
    public enum RecordExistsMatchStrategy
    {
        /// <summary>
        /// Unknown (default).
        /// </summary>
        Unknown,

        /// <summary>
        /// Match if all records exist per the specified filter.
        /// </summary>
        AllFound,

        /// <summary>
        /// Match if some records exist per the specified filter.
        /// </summary>
        SomeFound,

        /// <summary>
        /// Match if no records exist per the specified filter.
        /// </summary>
        NoneFound,
    }
}
