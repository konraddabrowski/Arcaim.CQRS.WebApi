using System;

namespace Arcaim.CQRS.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ValidateAttribute : Attribute
    {
    }
}