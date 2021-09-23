using System.Threading.Tasks;
using Arcaim.CQRS.WebApi.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Arcaim.CQRS.WebApi.HttpFilter
{
    public class AuthorizationFilter : Filter
    {
        public override async Task Invoke<T>(IServiceScopeFactory serviceScopeFactory)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var authorizeAttributeService = scope.ServiceProvider.GetService<IAuthorizeAttributeService>();
            if (authorizeAttributeService is null || !authorizeAttributeService.IsDecorated<T>())
            {
                return;
            }

            var authorizeService = scope.ServiceProvider.GetService<IAuthorization>();
            if (authorizeService is not null)
            {
                authorizeService.SetRequiredRoles(authorizeAttributeService.RequiredRoles<T>());
                await authorizeService.AuthorizeAsync();
            }
        }
    }
}