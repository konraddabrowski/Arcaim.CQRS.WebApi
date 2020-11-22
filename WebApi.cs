using Arcaim.CQRS.Commands;
using Arcaim.CQRS.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Arcaim.CQRS.WebApi
{
    public class WebApi : IWebApi
    {
        public IEndpointRouteBuilder Builder { get; }
        public string Pattern { get; }

        public WebApi(IEndpointRouteBuilder builder, string pattern)
        {
            Builder = builder;
            Pattern = pattern;
        }

        public IEndpointConventionBuilder Get(RequestDelegate requestDelegate)
            => Builder.MapGet(Pattern, requestDelegate);

        public IEndpointConventionBuilder Get<T, S>()
            where T : IQuery<S>
            where S : class
            => Builder.MapGet(Pattern, async ctx =>
            {
                var result = await Builder
                    .GetService<IQueryDispatcher>()
                    .DispatchAsync(await ctx.GetModel<T>());

                ctx.Return(result);
            });

        public IEndpointConventionBuilder Post(RequestDelegate requestDelegate)
            => Builder.MapPost(Pattern, requestDelegate);

        public IEndpointConventionBuilder Post<T>() where T : ICommand
            => Builder.MapPost(Pattern, async ctx => await Builder
            .GetService<ICommandDispatcher>()
            .DispatchAsync(await ctx.GetModel<T>()));

        public IEndpointConventionBuilder Put(RequestDelegate requestDelegate)
            => Builder.MapPut(Pattern, requestDelegate);

        public IEndpointConventionBuilder Put<T>() where T : ICommand
            => Builder.MapPut(Pattern, async ctx => await Builder
            .GetService<ICommandDispatcher>()
            .DispatchAsync(await ctx.GetModel<T>()));

        public IEndpointConventionBuilder Delete(RequestDelegate requestDelegate)
            => Builder.MapDelete(Pattern, requestDelegate);

        public IEndpointConventionBuilder Delete<T>() where T : ICommand
            => Builder.MapDelete(Pattern, async ctx => await Builder
            .GetService<ICommandDispatcher>()
            .DispatchAsync(await ctx.GetModel<T>()));
    }
}