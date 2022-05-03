using System;

namespace Arcaim.CQRS.WebApi.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AuthorizeAttribute : Attribute
{
  public string Roles { get; set; }

  public AuthorizeAttribute()
  {
  }
}