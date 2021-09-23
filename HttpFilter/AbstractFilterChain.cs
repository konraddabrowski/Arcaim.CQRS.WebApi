using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Arcaim.CQRS.WebApi.HttpFilter
{
    public abstract class AbstractFilterChain
    {
        private HashSet<Filter> _filters;
        private IServiceScopeFactory _serviceScopeFactory;

        public AbstractFilterChain(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _filters = new();
            ProvideFilters();
        }

        public void AddFilter<T>() where T : Filter, new()
            => _filters.Add(new T());

        public async Task InvokeFiltersAsync<T>(T instance)
        {
            foreach(var filter in _filters)
            {
                await filter.Invoke(instance);
                await filter.Invoke<T>(_serviceScopeFactory);
                await filter.Invoke(instance, _serviceScopeFactory);
            }
        }

        private void ProvideFilters() {
            AddFilter<AuthorizationFilter>();
            AddFilter<ValidationFilter>();
            Filters();
        }

        public abstract void Filters();
    }
}