// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisteredSequence.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Domain
{
    using System.Collections.Generic;
    using Naos.Protocol.Domain;

    /// <summary>
    /// TODO: Starting point for new project.
    /// </summary>
    public class RegisteredSequence : OperationSequence
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredSequence"/> class.
        /// </summary>
        /// <param name="registrationDetails">The registration details.</param>
        /// <param name="operationPrototypes">The operations.</param>
        public RegisteredSequence(
            SequenceRegistrationDetails             registrationDetails,
            IReadOnlyCollection<OperationPrototype> operationPrototypes)
            : base(operationPrototypes)
        {
            this.RegistrationDetails = registrationDetails;
        }

        /// <summary>
        /// Gets the registration details.
        /// </summary>
        /// <value>The registration details.</value>
        public SequenceRegistrationDetails RegistrationDetails { get; private set; }
    }
}
