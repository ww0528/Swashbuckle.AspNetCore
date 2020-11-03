﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;

namespace Swashbuckle.AspNetCore.SwaggerGen
{
    public class SchemaGeneratorOptions
    {
        public SchemaGeneratorOptions()
        {
            DataContractResolvers = new List<IDataContractResolver>();
            CustomTypeMappings = new Dictionary<Type, Func<OpenApiSchema>>();
            SchemaIdSelector = DefaultSchemaIdSelector;
            KnownTypesSelector = DefaultKnownTypesSelector;
            SchemaFilters = new List<ISchemaFilter>();
        }

        public IList<IDataContractResolver> DataContractResolvers { get; set; }

        public IDictionary<Type, Func<OpenApiSchema>> CustomTypeMappings { get; set; }

        public bool UseInlineDefinitionsForEnums { get; set; }

        public Func<Type, string> SchemaIdSelector { get; set; }

        public bool IgnoreObsoleteProperties { get; set; }

        public bool UseAllOfToExtendReferenceSchemas { get; set; }

        public bool UseAllOfForInheritance { get; set; }

        public bool UseOneOfForPolymorphism { get; set; }

        public Func<Type, IEnumerable<Type>> KnownTypesSelector { get; set; }

        public Func<Type, string> DiscriminatorNameSelector { get; set; }

        public Func<Type, string> DiscriminatorValueSelector { get; set; }

        public IList<ISchemaFilter> SchemaFilters { get; set; }

        private string DefaultSchemaIdSelector(Type modelType)
        {
            if (!modelType.IsConstructedGenericType) return modelType.Name.Replace("[]", "Array");

            var prefix = modelType.GetGenericArguments()
                .Select(genericArg => DefaultSchemaIdSelector(genericArg))
                .Aggregate((previous, current) => previous + current);

            return prefix + modelType.Name.Split('`').First();
        }

        private IEnumerable<Type> DefaultKnownTypesSelector(Type baseType)
        {
            return baseType.IsAbstract
                ? baseType.Assembly.GetTypes().Where(type => type.IsSubclassOf(baseType))
                : Enumerable.Empty<Type>();
        }
    }
}