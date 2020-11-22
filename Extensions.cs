using System;
using System.Threading.Tasks;
using Arcaim.CQRS.Commands;
using Arcaim.CQRS.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Arcaim.CQRS.WebApi
{
    public static class Extensions
    {
        // public static IServiceCollection AddWebApi(this IServiceCollection services)
        // {
        //     services.AddScoped<IWebApi, WebApi>();

        //     return services;
        // }

        public static IWebApi Controller(this IEndpointRouteBuilder builder, string pattern)
            => new WebApi(builder, pattern);

        public static IWebApi Action(this IWebApi api, Func<IWebApi, IEndpointConventionBuilder> requestDelegate)
        {
            requestDelegate.Invoke(api);

            return api;
        }

        public static IWebApi Action(this IWebApi api,
            Func<IWebApi, ICommandDispatcher, ICommandDispatcher, IEndpointConventionBuilder> requestDelegate)
        {
            var commandDispatcher = api.Builder.ServiceProvider.GetService<ICommandDispatcher>();
            var queryDispatcher = api.Builder.ServiceProvider.GetService<ICommandDispatcher>();
            requestDelegate.Invoke(api, commandDispatcher, queryDispatcher);

            return api;
        }

        public static IWebApi Command(this IWebApi api,
            Func<ICommandApi, ICommandDispatcher, IEndpointConventionBuilder> requestDelegate)
        {
            var dispatcher = api.Builder.ServiceProvider.GetService<ICommandDispatcher>();
            requestDelegate.Invoke(api as ICommandApi, dispatcher);

            return api;
        }

        public static IWebApi Query(this IWebApi api,
            Func<IQueryApi, IQueryDispatcher, IEndpointConventionBuilder> requestDelegate)
        {
            var dispatcher = api.Builder.ServiceProvider.GetService<IQueryDispatcher>();
            requestDelegate.Invoke(api as IQueryApi, dispatcher);

            return api;
        }

        public static T GetService<T>(this IEndpointRouteBuilder builder)
            => builder.ServiceProvider.GetService<T>();

        public static async Task<T> GetModel<T>(this HttpContext context)
            => await context.Request.ReadFromJsonAsync<T>();

        public static void Return<T>(this HttpContext context, T result) where T : class
            => context.Response.WriteAsJsonAsync(result).Wait();
    }
}