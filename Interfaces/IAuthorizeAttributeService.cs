using System.Collections.Generic;

namespace Arcaim.CQRS.WebApi
{
    internal interface IAuthorizeAttributeService
    {
        bool IsDecorated<T>();
        IEnumerable<IEnumerable<string>> RequiredRoles<T>();
    }
}