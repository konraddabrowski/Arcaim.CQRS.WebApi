using System.Threading.Tasks;

namespace Arcaim.CQRS.WebApi.HttpFilter
{
    public class FilterManager : IFilterManager
    {
        private AbstractFilterChain _filterChain;

        public FilterManager(AbstractFilterChain filterChain)
        {
            _filterChain = filterChain;
        }

        public async Task InvokeFiltersAsync<T>(T instance)
            => await _filterChain.InvokeFiltersAsync(instance);
    }
}