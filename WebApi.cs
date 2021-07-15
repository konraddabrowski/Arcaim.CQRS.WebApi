using System;
using System.Threading.Tasks;
using Arcaim.CQRS.Commands;
using Arcaim.CQRS.Queries;
using Arcaim.CQRS.WebApi.Exceptions;
using Arcaim.CQRS.WebApi.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

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

        public WebApi(IEndpointRouteBuilder endpointRouteBuilder, string pattern)
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
                var model = await ctx.GetModel<T>();

                await AuthorizeAsync(model);
                await ValidateAsync(model);
                var result = await EndpointRouteBuilder
                    .GetService<IQueryDispatcher>()
                    .DispatchAsync(model);

                ctx.Return(result);
            });

        public IEndpointConventionBuilder Post(RequestDelegate requestDelegate)
            => EndpointRouteBuilder.MapPost(Pattern, requestDelegate);

        public IEndpointConventionBuilder Post<T>() where T : ICommand
            => EndpointRouteBuilder.MapPost(Pattern, async ctx =>
            {
                var model = await ctx.GetModel<T>();

                await AuthorizeAsync(model);
                await ValidateAsync(model);
                await EndpointRouteBuilder.GetService<ICommandDispatcher>()
                    .DispatchAsync(model);
            });

        public IEndpointConventionBuilder Put(RequestDelegate requestDelegate)
            => EndpointRouteBuilder.MapPut(Pattern, requestDelegate);

        public IEndpointConventionBuilder Put<T>() where T : ICommand
            => EndpointRouteBuilder.MapPut(Pattern, async ctx =>
            {
                var model = await ctx.GetModel<T>();

                await AuthorizeAsync(model);
                await ValidateAsync(model);
                await EndpointRouteBuilder.GetService<ICommandDispatcher>()
                    .DispatchAsync(await ctx.GetModel<T>());
            });

        public IEndpointConventionBuilder Delete(RequestDelegate requestDelegate)
            => EndpointRouteBuilder.MapDelete(Pattern, requestDelegate);

        public IEndpointConventionBuilder Delete<T>() where T : ICommand
            => EndpointRouteBuilder.MapDelete(Pattern, async ctx =>
            {
                var model = await ctx.GetModel<T>();

                await AuthorizeAsync(model);
                await ValidateAsync(model);
                await EndpointRouteBuilder.GetService<ICommandDispatcher>()
                    .DispatchAsync(await ctx.GetModel<T>());
            });

        private async Task AuthorizeAsync<T>(T instance)
        {
            var serviceFactory = EndpointRouteBuilder.GetService<IServiceScopeFactory>();
            var authorizeAttributeService = EndpointRouteBuilder.GetService<IAuthorizeAttributeService>();
            if (authorizeAttributeService is null || !authorizeAttributeService.IsDecorated<T>())
            {
                return;
            }

            using var scope = serviceFactory.CreateScope();
            var authorizeService = scope.ServiceProvider.GetService<IAuthorization>();
            if (authorizeService is not null)
            {
                authorizeService.SetRequiredRoles(authorizeAttributeService.RequiredRoles<T>());
                await authorizeService.AuthorizeAsync();
            }
        }

        private async Task ValidateAsync<T>(T instance)
        {
            var validatorService = EndpointRouteBuilder.GetService<IValidator>();
            var validateAttributeService = EndpointRouteBuilder.GetService<IValidateAttributeService>();
            if (validatorService is not null &&
                validateAttributeService is not null &&
                validateAttributeService.IsDecorated<T>())
            {
                await validatorService.ValidateAsync(instance);
            }
        }
    }
}
