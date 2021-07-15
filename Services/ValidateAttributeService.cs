using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Arcaim.CQRS.WebApi.Attributes;
using Arcaim.CQRS.WebApi.Interfaces;

namespace Arcaim.CQRS.WebApi.Services
{
    internal class ValidateAttributeService : IValidateAttributeService
    {
        private readonly HashSet<Type> _attributeList = new HashSet<Type>();

        public ValidateAttributeService()
            => FindAllMethodsDecoratedByAttribute();

        private void FindAllMethodsDecoratedByAttribute()
        {
            var assemblies = Directory
                .GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)));

            assemblies.SelectMany(assembly => assembly.GetTypes().SelectMany(type => type.GetMethods()))
                .Where(method => method.GetCustomAttribute<ValidateAttribute>() is not null)
                .SelectMany(methodInfo => methodInfo.GetParameters())
                .ToList().ForEach(x =>
                    _attributeList.Add(x.ParameterType)
                );
        }

        public bool IsDecorated<T>()
            => _attributeList.Contains(typeof(T));
    }
}
