using System.Threading.Tasks;
using Arcaim.CQRS.Commands;
using Arcaim.CQRS.Queries;
using Arcaim.CQRS.WebApi.HttpFilter;
using Arcaim.CQRS.WebApi.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Arcaim.CQRS.WebApi
{
    public class WebApi : IWebApi
    {
        public IEndpointRouteBuilder EndpointRouteBuilder { get; }
        public IApplicationBuilder ApplicationBuilder { get; }
        private string _pattern;
        public string Pattern
        {
            get => string.IsNullOrWhiteSpace(this.PatternAction) ? _pattern : $"{_pattern}/{PatternAction}";
            init => _pattern = value;
        }

        public string PatternAction { get; set; }

        internal WebApi(IEndpointRouteBuilder endpointRouteBuilder, string pattern)
        {
            EndpointRouteBuilder = endpointRouteBuilder;
            Pattern = pattern;
        }

        public IEndpointConventionBuilder Get(RequestDelegate requestDelegate)
            => EndpointRouteBuilder.MapGet(Pattern, requestDelegate);

        public IEndpointConventionBuilder Get<T, S>()
            where T : IQuery<S>
            where S : class
            => EndpointRouteBuilder.MapGet(Pattern, async ctx =>
            {
                var instance = await ctx.GetModel<T>();

                await InvokeFilters(instance);
                var result = await EndpointRouteBuilder
                    .GetService<IQueryDispatcher>()
                    .DispatchAsync(instance);

                ctx.Return(result);
            });

        public IEndpointConventionBuilder Post(RequestDelegate requestDelegate)
            => EndpointRouteBuilder.MapPost(Pattern, requestDelegate);

        public IEndpointConventionBuilder Post<T>() where T : ICommand
            => EndpointRouteBuilder.MapPost(Pattern, async ctx =>
            {
                var instance = await ctx.GetModel<T>();

                await InvokeFilters(instance);
                await EndpointRouteBuilder
                    .GetService<ICommandDispatcher>()
                    .DispatchAsync(instance);
            });

        public IEndpointConventionBuilder Put(RequestDelegate requestDelegate)
            => EndpointRouteBuilder.MapPut(Pattern, requestDelegate);

        public IEndpointConventionBuilder Put<T>() where T : ICommand
            => EndpointRouteBuilder.MapPut(Pattern, async ctx =>
            {
                var instance = await ctx.GetModel<T>();

                await InvokeFilters(instance);
                await EndpointRouteBuilder
                    .GetService<ICommandDispatcher>()
                    .DispatchAsync(instance);
            });

        public IEndpointConventionBuilder Delete(RequestDelegate requestDelegate)
            => EndpointRouteBuilder.MapDelete(Pattern, requestDelegate);

        public IEndpointConventionBuilder Delete<T>() where T : ICommand
            => EndpointRouteBuilder.MapDelete(Pattern, async ctx =>
            {
                var instance = await ctx.GetModel<T>();

                await InvokeFilters(instance);
                await EndpointRouteBuilder.GetService<ICommandDispatcher>()
                    .DispatchAsync(instance);
            });

        private async Task InvokeFilters<T>(T instance)
        {
            var filterManager = EndpointRouteBuilder.GetService<IFilterManager>();
            if (filterManager is not null)
            {
                await filterManager.InvokeFiltersAsync(instance);
            }
        }
    }
}
