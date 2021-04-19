using System.Threading.Tasks;

namespace Arcaim.CQRS.WebApi.Interfaces
{
    public interface IValidatorService
    {
        Task ValidateAsync<T>(T instance);
    }
}