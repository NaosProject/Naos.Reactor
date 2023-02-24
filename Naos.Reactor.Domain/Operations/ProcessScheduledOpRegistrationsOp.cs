// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessExecuteOnScheduleEventsOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System.Threading.Tasks;
    using OBeautifulCode.Representation.System;
    using OBeautifulCode.Type;

    /// <summary>
    /// Evaluates all <see cref="ScheduledOpRegistration"/>'s.
    /// </summary>
    public partial class ProcessScheduledOpRegistrationsOp : VoidOperationBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessScheduledOpRegistrationsOp"/> class.
        /// </summary>
        /// <param name="degreesOfParallelismForDependencyChecks">The ideal number of <see cref="ReactionRegistration"/>'s to evaluate in parallel; used in <see cref="ParallelOptions"/> for 'MaxDegreeOfParallelism', see documentation for restrictions.</param>
        /// <param name="deprecatedIdentifierType">OPTIONAL type to consider as a deprecated identifier (e.g. <see cref="T:Naos.Database.Domain.IdDeprecatedEvent" />).  DEFAULT is none.</param>
        public ProcessScheduledOpRegistrationsOp(
            int degreesOfParallelismForDependencyChecks = -1,
            TypeRepresentation deprecatedIdentifierType = null)
        {
            this.DegreesOfParallelismForDependencyChecks = degreesOfParallelismForDependencyChecks;
            this.DeprecatedIdentifierType = deprecatedIdentifierType;
        }

        /// <summary>
        /// Get the ideal number of <see cref="ReactionRegistration"/>'s to evaluate in parallel; used in <see cref="ParallelOptions"/> for 'MaxDegreeOfParallelism', see documentation for restrictions.
        /// </summary>
        public int DegreesOfParallelismForDependencyChecks { get; private set; }

        /// <summary>
        /// Gets the type to consider as a deprecated identifier (e.g. <see cref="T:Naos.Database.Domain.IdDeprecatedEvent" />).
        /// </summary>
        public TypeRepresentation DeprecatedIdentifierType { get; private set; }
    }
}