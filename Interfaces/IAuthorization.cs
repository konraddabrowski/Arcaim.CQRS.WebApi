using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arcaim.CQRS.WebApi.Interfaces;

public interface IAuthorization
{
  Task AuthorizeAsync();
  void SetRequiredRoles(IEnumerable<IEnumerable<string>> enumerable);
}