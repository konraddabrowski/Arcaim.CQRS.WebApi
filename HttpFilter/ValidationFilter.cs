using System.Threading.Tasks;
using Arcaim.CQRS.WebApi.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Arcaim.CQRS.WebApi.HttpFilter
{
    internal sealed class ValidationFilter : Filter
    {
        public override async Task Invoke<T>(T instance, IServiceScopeFactory serviceScopeFactory)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var validatorService = scope.ServiceProvider.GetService<IValidator>();
            var validateAttributeService = scope.ServiceProvider.GetService<IValidateAttributeService>();
            if (validatorService is not null &&
                validateAttributeService is not null &&
                validateAttributeService.IsDecorated<T>())
            {
                await validatorService.ValidateAsync(instance);
            }
        }
    }
}