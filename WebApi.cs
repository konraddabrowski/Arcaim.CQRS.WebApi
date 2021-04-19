using System.Threading.Tasks;
using Arcaim.CQRS.Commands;
using Arcaim.CQRS.Queries;
using Arcaim.CQRS.WebApi.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Arcaim.CQRS.WebApi
{
    public class WebApi : IWebApi
    {
        public IEndpointRouteBuilder Builder { get; }
        private string _pattern;
        public string Pattern
        {
            get => string.IsNullOrWhiteSpace(this.PatternAction) ? _pattern : $"{_pattern}/{PatternAction}";
            init => _pattern = value;
        }

        public string PatternAction { get; set; }

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
                var model = await ctx.GetModel<T>();

                await ValidateAsync(model);
                var result = await Builder
                    .GetService<IQueryDispatcher>()
                    .DispatchAsync(model);

                ctx.Return(result);
            });

        public IEndpointConventionBuilder Post(RequestDelegate requestDelegate)
            => Builder.MapPost(Pattern, requestDelegate);

        public IEndpointConventionBuilder Post<T>() where T : ICommand
            => Builder.MapPost(Pattern, async ctx =>
            {
                var model = await ctx.GetModel<T>();

                await ValidateAsync(model);
                await Builder.GetService<ICommandDispatcher>()
                    .DispatchAsync(model);
            });

        public IEndpointConventionBuilder Put(RequestDelegate requestDelegate)
            => Builder.MapPut(Pattern, requestDelegate);

        public IEndpointConventionBuilder Put<T>() where T : ICommand
            => Builder.MapPut(Pattern, async ctx =>
            {
                var model = await ctx.GetModel<T>();

                await ValidateAsync(model);
                await Builder.GetService<ICommandDispatcher>()
                    .DispatchAsync(await ctx.GetModel<T>());
            });

        public IEndpointConventionBuilder Delete(RequestDelegate requestDelegate)
            => Builder.MapDelete(Pattern, requestDelegate);

        public IEndpointConventionBuilder Delete<T>() where T : ICommand
            => Builder.MapDelete(Pattern, async ctx =>
            {
                var model = await ctx.GetModel<T>();

                await ValidateAsync(model);
                await Builder.GetService<ICommandDispatcher>()
                    .DispatchAsync(await ctx.GetModel<T>());
            });

        private async Task ValidateAsync<T>(T instance)
        {
            var validatorService = Builder.GetService<IValidatorService>();
            var validateAttributeService = Builder.GetService<IValidateAttributeService>();
            if (validatorService is not null &&
                validateAttributeService is not null &&
                validateAttributeService.IsValidateAttributeImplemented<T>())
            {
                await validatorService.ValidateAsync(instance);
            }
        }
    }
}
