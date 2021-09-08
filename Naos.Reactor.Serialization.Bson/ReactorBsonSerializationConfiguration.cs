// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReactorBsonSerializationConfiguration.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Reactor.Serialization.Bson
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Naos.Database.Serialization.Bson;
    using OBeautifulCode.Serialization.Bson;
    using OBeautifulCode.Type;
    using OBeautifulCode.Type.Recipes;

    /// <inheritdoc />
    public class ReactorBsonSerializationConfiguration : BsonSerializationConfigurationBase
    {
        /// <inheritdoc />
        protected override IReadOnlyCollection<BsonSerializationConfigurationType> DependentBsonSerializationConfigurationTypes =>
            new[]
            {
                new BsonSerializationConfigurationType(typeof(DatabaseBsonSerializationConfiguration)),
            };

        /// <inheritdoc />
        protected override IReadOnlyCollection<TypeToRegisterForBson> TypesToRegisterForBson =>
            new Type[0]
               .Concat(
                    new[]
                    {
                        typeof(IModel),
                    })
               .Concat(Domain.ProjectInfo.Assembly.GetPublicEnumTypes())
               .Select(_ => _.ToTypeToRegisterForBson())
               .ToList();
    }
}
