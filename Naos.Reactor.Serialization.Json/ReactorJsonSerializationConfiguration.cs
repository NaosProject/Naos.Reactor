// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReactorJsonSerializationConfiguration.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Serialization.Json
{
    using System;
    using System.Collections.Generic;

    using OBeautifulCode.Serialization.Json;

    /// <inheritdoc />
    public class ReactorJsonSerializationConfiguration : JsonConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<Type> TypesToAutoRegister => new Type[]
        {
            // ADD TYPES TO REGISTER HERE
        };
    }
}
