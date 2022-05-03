using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Arcaim.CQRS.WebApi.Attributes;

namespace Arcaim.CQRS.WebApi.Services;

internal sealed class AuthorizeAttributeService : IAuthorizeAttributeService
{
  private readonly HashSet<Type> _classTypes = new HashSet<Type>();
  private readonly HashSet<Type> _genericTypeArguments = new HashSet<Type>();
  private List<AuthorizeAppData> _authorizeAppDataList;

  public AuthorizeAttributeService()
  {
    FindClassesDecoratedByAttribute();
    SetAuthorizeAppData();
  }

  private void FindClassesDecoratedByAttribute()
  {
    var assemblies = Directory
      .GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
      .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)));

    assemblies.SelectMany(assembly => assembly.GetTypes())
      .Where(type => type.GetCustomAttributes<AuthorizeAttribute>().Any())
      .ToList()
      .ForEach(x => _classTypes.Add(x));
  }

  private void SetAuthorizeAppData()
  {
    _authorizeAppDataList = _classTypes.Select(classType => new AuthorizeAppData {
      ClassType = classType,
      GenericTypeArguments = classType.GetInterfaces().SelectMany(c => c.GenericTypeArguments).ToList(),
      Roles = RolesOfCustomAttributes(classType)
    }).ToList();
  }

  private List<List<string>> RolesOfCustomAttributes(Type classType)
  {
    var roles = classType.CustomAttributes.Select(e => {
      if (!e.NamedArguments.Any())
      {
        return null;
      }

      return e.NamedArguments.SelectMany(c => c.TypedValue.Value.ToString()
        .Split(',')
        .Select(role => role.Trim())
        .ToList()).ToList();
      }
    );

    return roles.Where(x => x is not null).ToList();
  }



  public bool IsDecorated<T>()
    => _authorizeAppDataList.Any(x => x.GenericTypeArguments.Contains(typeof(T)));
  
  public IEnumerable<IEnumerable<string>> RequiredRoles<T>()
  {
    var requiredRoles = _authorizeAppDataList
      .Where(x => x.GenericTypeArguments.Contains(typeof(T)))
      .Select(x => x.Roles)
      .FirstOrDefault();
    
    return requiredRoles;
  }

  private class AuthorizeAppData
  {
    public Type ClassType { get; set; }
    public List<Type> GenericTypeArguments { get; set; }
    public List<List<string>> Roles { get; set; }
  }
}