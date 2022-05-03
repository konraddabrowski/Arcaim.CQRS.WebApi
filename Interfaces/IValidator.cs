using System.Threading.Tasks;

namespace Arcaim.CQRS.WebApi.Interfaces;

public interface IValidator
{
  Task ValidateAsync<T>(T instance);
}