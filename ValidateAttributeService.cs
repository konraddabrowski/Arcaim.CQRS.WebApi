using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Arcaim.CQRS.WebApi.Interfaces;

namespace Arcaim.CQRS.WebApi
{
    internal class ValidateAttributeService : IValidateAttributeService
    {
        private readonly HashSet<Type> _implementedValidateAttributeList = new HashSet<Type>();

        public ValidateAttributeService()
            => FindAllMethodParametersForValidateAttributeFromAssembly();

        private void FindAllMethodParametersForValidateAttributeFromAssembly()
        {
            var assemblies = Directory
                .GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)));

            assemblies.SelectMany(assembly => assembly.GetTypes().SelectMany(type => type.GetMethods()))
                .Where(method => method.GetCustomAttribute<ValidateAttribute>() is not null)
                .SelectMany(methodInfo => methodInfo.GetParameters())
                .ToList().ForEach(x =>
                    _implementedValidateAttributeList.Add(x.ParameterType)
                );
        }

        public bool IsValidateAttributeImplemented<T>()
            => _implementedValidateAttributeList.Contains(typeof(T));
    }
}
