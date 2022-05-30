using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Web;
using Arcaim.CQRS.Commands;
using Arcaim.CQRS.Queries;
using Arcaim.CQRS.WebApi.HttpFilter;
using Arcaim.CQRS.WebApi.Interfaces;
using Arcaim.CQRS.WebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Arcaim.CQRS.WebApi;

public static class Extensions
{
  public static IServiceCollection AddWebApi(this IServiceCollection services)
  {
    services.AddSingleton<IValidateAttributeService, ValidateAttributeService>();
    services.AddSingleton<IAuthorizeAttributeService, AuthorizeAttributeService>();
    services.AddSingleton<IFilterManager, FilterManager>();

    services.AddCommandSeparation();
    services.AddQuerySeparation();

    return services;
  }

  public static IEndpointRouteBuilder Controller(
    this IEndpointRouteBuilder builder,
    string pattern,
    Action<IWebApi, IWebAction> webApiDelegate)
  {
    WebApi webApi = new(builder, pattern);
    webApiDelegate.Invoke(webApi, new WebAction(webApi));

    return builder;
  }

  public static T GetService<T>(this IEndpointRouteBuilder builder)
    => builder.ServiceProvider.GetService<T>();

  public static T GetModelFromQueryString<T>(this HttpContext context) where T : new()
  {
    var entity = new T();
    var properties = typeof(T).GetProperties();
    foreach (var property in properties)
    {
      var valueAsString = context.Request.Query[property.Name];
      object value = Parse(property.PropertyType, valueAsString);

      if (value == null)
      continue;

      property.SetValue(entity, value, null);
    }

    return entity;
  }

  private static object Parse(Type dataType, string ValueToConvert)
  {
    TypeConverter entity = TypeDescriptor.GetConverter(dataType);
    object value = entity.ConvertFromString(null, CultureInfo.InvariantCulture, ValueToConvert);

    return value;
  }

  public static async Task<T> GetModelFromJsonAsync<T>(this HttpContext context) where T : class
  {
    return await context.Request.ReadFromJsonAsync<T>();
  }

  public static void Return<T>(this HttpContext context, T result) where T : new()
    => context.Response.WriteAsJsonAsync(result).Wait();
}