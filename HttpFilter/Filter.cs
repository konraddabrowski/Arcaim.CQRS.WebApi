using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Arcaim.CQRS.WebApi.HttpFilter
{
    public abstract class Filter
    {
        public virtual Task Invoke<T>(T model)
        {
            return Task.CompletedTask;
        }

        public virtual Task Invoke<T>(IServiceScopeFactory serviceScopeFactory)
        {
            return Task.CompletedTask;
        }

        public virtual Task Invoke<T>(T model, IServiceScopeFactory serviceScopeFactory)
        {
            return Task.CompletedTask;
        }
    }
}