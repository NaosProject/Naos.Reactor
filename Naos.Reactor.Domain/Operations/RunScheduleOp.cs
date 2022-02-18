// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RunScheduleOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Evaluates all <see cref="ExecuteOpOnScheduleOp"/>'s.
    /// </summary>
    public partial class RunScheduleOp : VoidOperationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RunScheduleOp"/> class.
        /// </summary>
        /// <param name="deprecatedIdentifierType">OPTIONAL type to consider as a deprecated identifier (e.g. <see cref="T:Naos.Database.Domain.IdDeprecatedEvent" />).  DEFAULT is none.</param>
        public RunScheduleOp(
            TypeRepresentation deprecatedIdentifierType = null)
        {
            this.DeprecatedIdentifierType = deprecatedIdentifierType;
        }

        /// <summary>
        /// Gets the type to consider as a deprecated identifier (e.g. <see cref="T:Naos.Database.Domain.IdDeprecatedEvent" />).
        /// </summary>
        public TypeRepresentation DeprecatedIdentifierType { get; private set; }
    }
}
