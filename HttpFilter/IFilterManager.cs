using System.Threading.Tasks;

namespace Arcaim.CQRS.WebApi.HttpFilter
{
    public interface IFilterManager
    {
        Task InvokeFiltersAsync<T>(T instance);
    }
}