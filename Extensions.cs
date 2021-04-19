using System;
using System.Threading.Tasks;
using Arcaim.CQRS.Commands;
using Arcaim.CQRS.Queries;
using Arcaim.CQRS.WebApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Arcaim.CQRS.WebApi
{
    public static class Extensions
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            services.AddSingleton<IValidateAttributeService, ValidateAttributeService>();
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

        public static async Task<T> GetModel<T>(this HttpContext context)
            => await context.Request.ReadFromJsonAsync<T>();

        public static void Return<T>(this HttpContext context, T result) where T : class
            => context.Response.WriteAsJsonAsync(result).Wait();
    }
}