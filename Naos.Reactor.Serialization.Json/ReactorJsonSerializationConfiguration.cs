﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReactorJsonSerializationConfiguration.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Serialization.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OBeautifulCode.Serialization.Json;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;

    /// <inheritdoc />
    public class ReactorJsonSerializationConfiguration : JsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForJson> TypesToRegisterForJson =>
            new Type[0]
               .Concat(
                    new[]
                    {
                        typeof(IModel),
                    })
               .Concat(Domain.ProjectInfo.Assembly.GetPublicEnumTypes())
               .Select(_ => _.ToTypeToRegisterForJson())
               .ToList();
    }
}
