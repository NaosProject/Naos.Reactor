﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FactoryExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Serialization.Recipes source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Serialization.Recipes
{
    using OBeautifulCode.Compression.Recipes;
    using OBeautifulCode.Representation.System;

    /// <summary>
    /// Extension methods.
    /// </summary>
#if !OBeautifulCodeSerializationRecipesProject
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("OBeautifulCode.Serialization.Recipes", "See package version number")]
    internal
#else
    public
#endif
    static class FactoryExtensions
    {
        /// <summary>
        /// Converts an object to a self described serialization to persist or share.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="objectToPackageIntoDescribedSerialization">Object to serialize.</param>
        /// <param name="serializationDescription">Description of the serializer to use.</param>
        /// <param name="typeMatchStrategy">Optional type match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="TypeMatchStrategy.NamespaceAndName" />.</param>
        /// <param name="multipleMatchStrategy">Optional multiple match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="MultipleMatchStrategy.ThrowOnMultiple" />.</param>
        /// <param name="unregisteredTypeEncounteredStrategy">Optional strategy of what to do when encountering a type that has never been registered; DEFAULT is <see cref="UnregisteredTypeEncounteredStrategy.Throw" />.</param>
        /// <returns>Self described serialization.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = "Spelling/name is correct.")]
        public static DescribedSerialization ToDescribedSerialization<T>(
            this T objectToPackageIntoDescribedSerialization,
            SerializationDescription serializationDescription,
            TypeMatchStrategy typeMatchStrategy = TypeMatchStrategy.NamespaceAndName,
            MultipleMatchStrategy multipleMatchStrategy = MultipleMatchStrategy.ThrowOnMultiple,
            UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default)
        {
            return DomainExtensions.ToDescribedSerializationUsingSpecificFactory(
                objectToPackageIntoDescribedSerialization,
                serializationDescription,
                SerializerFactory.Instance,
                CompressorFactory.Instance,
                typeMatchStrategy,
                multipleMatchStrategy,
                unregisteredTypeEncounteredStrategy);
        }

        /// <summary>
        /// Converts a self described serialization back into it's object.
        /// </summary>
        /// <param name="describedSerialization">Self described serialized object.</param>
        /// <param name="typeMatchStrategy">Optional type match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="TypeMatchStrategy.NamespaceAndName" />.</param>
        /// <param name="multipleMatchStrategy">Optional multiple match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="MultipleMatchStrategy.ThrowOnMultiple" />.</param>
        /// <param name="unregisteredTypeEncounteredStrategy">Optional strategy of what to do when encountering a type that has never been registered; DEFAULT is <see cref="UnregisteredTypeEncounteredStrategy.Throw" />.</param>
        /// <returns>Originally serialized object.</returns>
        public static object DeserializePayload(this DescribedSerialization describedSerialization, TypeMatchStrategy typeMatchStrategy = TypeMatchStrategy.NamespaceAndName, MultipleMatchStrategy multipleMatchStrategy = MultipleMatchStrategy.ThrowOnMultiple, UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default)
        {
            return DomainExtensions.DeserializePayloadUsingSpecificFactory(
                describedSerialization,
                SerializerFactory.Instance,
                CompressorFactory.Instance,
                typeMatchStrategy,
                multipleMatchStrategy,
                unregisteredTypeEncounteredStrategy);
        }

        /// <summary>
        /// Converts a self described serialization back into it's object.
        /// </summary>
        /// <param name="describedSerialization">Self described serialized object.</param>
        /// <param name="typeMatchStrategy">Optional type match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="TypeMatchStrategy.NamespaceAndName" />.</param>
        /// <param name="multipleMatchStrategy">Optional multiple match strategy for resolving the type of object as well as the configuration type if any; DEFAULT is <see cref="MultipleMatchStrategy.ThrowOnMultiple" />.</param>
        /// <param name="unregisteredTypeEncounteredStrategy">Optional strategy of what to do when encountering a type that has never been registered; DEFAULT is <see cref="UnregisteredTypeEncounteredStrategy.Throw" />.</param>
        /// <typeparam name="T">Expected return type.</typeparam>
        /// <returns>Originally serialized object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Checked with Must and tested.")]
        public static T DeserializePayload<T>(this DescribedSerialization describedSerialization, TypeMatchStrategy typeMatchStrategy = TypeMatchStrategy.NamespaceAndName, MultipleMatchStrategy multipleMatchStrategy = MultipleMatchStrategy.ThrowOnMultiple, UnregisteredTypeEncounteredStrategy unregisteredTypeEncounteredStrategy = UnregisteredTypeEncounteredStrategy.Default)
        {
            return DomainExtensions.DeserializePayloadUsingSpecificFactory<T>(
                describedSerialization,
                SerializerFactory.Instance,
                CompressorFactory.Instance,
                typeMatchStrategy,
                multipleMatchStrategy,
                unregisteredTypeEncounteredStrategy);
        }
    }
}