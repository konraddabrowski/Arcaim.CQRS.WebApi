using System;

namespace Arcaim.CQRS.WebApi
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateAttribute : Attribute
    {
    }
}